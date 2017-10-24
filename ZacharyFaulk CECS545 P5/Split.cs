using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class Split
    {
        //Splits the map at y coordinate split, finds the cities closest to (0, split) and (100, split)
        //above and below the split line and uses them as the starting and ending cities for the greedy insertNode algorithm
        public static void splitY(newCity[] cityArray, float split, ref globalVars globalVars, ref List<newChild> childList)
        {
            //Distance variables
            float distance = 0;
            float totalDistance = 0;

            float distanceTL = 0;       //Top Left Distance
            float distanceTR = 0;       //Top Right Distance
            float distanceBR = 0;       //Bottom Right Distance
            float distanceBL = 0;       //Bottom Left Distance

            //newCorner objects to find the 4 corners
            newCorner topLeft = new newCorner(float.MaxValue);
            newCorner topRight = new newCorner(float.MaxValue);
            newCorner bottomLeft = new newCorner(float.MaxValue);
            newCorner bottomRight = new newCorner(float.MaxValue);

            //Lists to store the cities above and below the split point
            List<int> topList = new List<int>();
            List<int> topFinal = new List<int>();
            List<int> bottomList = new List<int>();
            List<int> bottomFinal = new List<int>();

            //Check each city in the cityArray to see if it's
            //above or below the split point
            for (int c = 1; c < cityArray.Length + 1; c++)
            {
                //Above the split point
                if (cityArray[c - 1].yCoordinate > split)
                {
                    topList.Add(c);     //Add to the topList

                    //Find cities closest to (0, split) and (100, split) 
                    float x1 = cityArray[c - 1].xCoordinate;
                    float y1 = cityArray[c - 1].yCoordinate;
                    float x2 = 0;
                    float x3 = 100;
                    float y2 = split;
                    distanceTL = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    distanceTR = (float)Math.Sqrt((((x3 - x1)) * (x3 - x1)) + ((y2 - y1) * (y2 - y1)));

                    //Check if the city is closest to (0, split) and (100, split) 
                    if (distanceTL < topLeft.coordinate)
                    {
                        topLeft.id = c;
                        topLeft.coordinate = distanceTL;
                    }
                    if (distanceTR < topRight.coordinate)
                    {
                        topRight.id = c;
                        topRight.coordinate = distanceTR;
                    }
                }

                //At or below the split point
                else
                {
                    bottomList.Add(c);  //Add to the bottom list

                    //Find cities closest to (0, split) and (100, split) 
                    float x1 = cityArray[c - 1].xCoordinate;
                    float y1 = cityArray[c - 1].yCoordinate;
                    float x2 = 0;
                    float x3 = 100;
                    float y2 = split;
                    distanceBL = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    distanceBR = (float)Math.Sqrt((((x3 - x1)) * (x3 - x1)) + ((y2 - y1) * (y2 - y1)));

                    //Check if the city is closest to (0, split) and (100, split) 
                    if (distanceBL < bottomLeft.coordinate)
                    {
                        bottomLeft.id = c;
                        bottomLeft.coordinate = distanceBL;
                    }
                    if (distanceBR < bottomRight.coordinate)
                    {
                        bottomRight.id = c;
                        bottomRight.coordinate = distanceBR;
                    }
                }
            }

            //Discard current split if it resulted in a single city being
            //both bottomRight/bottomLeft or topRight/topLeft
            if ((topList.Count <= 1) || (bottomList.Count <= 1))
            {
                return;
            }

            //Adding the corner cities to the top/bottomFinal lists
            //Removing the corner cities from the top/bottom lists
            topFinal.Add(topLeft.id);
            topFinal.Add(topRight.id);
            bottomFinal.Add(bottomRight.id);
            bottomFinal.Add(bottomLeft.id);
            topList.Remove(topLeft.id);
            topList.Remove(topRight.id);
            bottomList.Remove(bottomLeft.id);
            bottomList.Remove(bottomRight.id);

            //Call insertNode function to generate the new greedy paths
            InsertNode.insertNode(cityArray, topList, ref topFinal);
            InsertNode.insertNode(cityArray, bottomList, ref bottomFinal);

            //Combine the top and bottom paths to complete the tour
            topFinal.AddRange(bottomFinal);
            topFinal.Add(topFinal[0]);

            //If the path's distance wasn't the most recent path to be computed
            //find the distance of the path
            if (topFinal.SequenceEqual(globalVars.lastPath) == false)
            {
                globalVars.unique++;
                for (int d = 1; d < topFinal.Count; d++)
                {
                    int xy1 = topFinal[d - 1] - 1; //Location of city A in city List
                    int xy2 = topFinal[d] - 1;     //Location of city B in city List

                    //Find x and y coordinates of city A and B
                    float x1 = cityArray[xy1].xCoordinate;
                    float x2 = cityArray[xy2].xCoordinate;
                    float y1 = cityArray[xy1].yCoordinate;
                    float y2 = cityArray[xy2].yCoordinate;

                    //Use distance equation to find the distance between city A and B
                    //Add distance to totalDistance
                    distance = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    totalDistance = totalDistance + distance;
                }

                //If the new path distance is smaller than the
                //current smallest distance then replace
                //the smallest distance with the new distance
                if (totalDistance < globalVars.shortDistance)
                {
                    globalVars.shortDistance = totalDistance;  //Store the new shorter distance
                    globalVars.shortList = topFinal;           //Store the corresponding path
                    globalVars.shortSplit = split;             //Store the current split value
                }

                //Store the most recent path to avoid
                //computing the same path multiple times
                globalVars.lastPath = topFinal;

                //Create newChild with topFinal path and corresponding distance
                newChild tempChild = new newChild(totalDistance, topFinal);

                //Rearange the tempChilds path so that it starts and ends with "1"
                tempChild.path.RemoveAt(topFinal.Count - 1);
                for (int i = 0; i < tempChild.path.Count; i++)
                {
                    i = 0;
                    if (tempChild.path[0] == 1)
                    {
                        break;
                    }
                    else
                    {
                        tempChild.path.Add(tempChild.path[0]);
                        tempChild.path.RemoveAt(0);
                    }
                }
                tempChild.path.Add(tempChild.path[0]);

                //Sort the tempChild
                SortChild.sortChild(tempChild, ref globalVars, ref childList);
            }

        }

        //Same the Split function about but splits the map at x coordinates instead of y coordinates
        public static void splitX(newCity[] cityArray, float split, ref globalVars globalVars, ref List<newChild> childList)
        {
            //Distance variables
            float distance = 0;
            float totalDistance = 0;

            float distanceTL = 0;       //Top Left distance
            float distanceTR = 0;       //Top Right distance
            float distanceBR = 0;       //Bottom Right Distance
            float distanceBL = 0;       //Bottom Left distance

            //newCorner objects to find the 4 corners
            newCorner topLeft = new newCorner(float.MaxValue);
            newCorner topRight = new newCorner(float.MaxValue);
            newCorner bottomLeft = new newCorner(float.MaxValue);
            newCorner bottomRight = new newCorner(float.MaxValue);

            //Lists to store the cities above and below the split point
            List<int> topList = new List<int>();
            List<int> topFinal = new List<int>();
            List<int> bottomList = new List<int>();
            List<int> bottomFinal = new List<int>();

            //Check each city in the cityArray to see if it's
            //above or below the split point
            for (int c = 1; c < cityArray.Length + 1; c++)
            {
                //Above the split point
                if (cityArray[c - 1].xCoordinate > split)
                {
                    topList.Add(c);     //Add to the topList

                    //Find cities closest to (split, 0) and (split, 100) 
                    float x1 = cityArray[c - 1].xCoordinate;
                    float y1 = cityArray[c - 1].yCoordinate;
                    float x2 = split;
                    float y2 = 0;
                    float y3 = 100;
                    distanceBR = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    distanceTR = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y3 - y1) * (y3 - y1)));

                    //Check if the city is closest to (split, 0) and (split, 100)
                    if (distanceTL < topLeft.coordinate)
                    {
                        topLeft.id = c;
                        topLeft.coordinate = distanceTL;
                    }
                    if (distanceTR < topRight.coordinate)
                    {
                        topRight.id = c;
                        topRight.coordinate = distanceTR;
                    }
                }

                //At or below the split point
                else
                {
                    bottomList.Add(c);  //Add to the bottom list

                    //Find cities closest to (split, 0) and (split, 100) 
                    float x1 = cityArray[c - 1].xCoordinate;
                    float y1 = cityArray[c - 1].yCoordinate;
                    float x2 = split;
                    float y2 = 0;
                    float y3 = 100;
                    distanceBL = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    distanceTL = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y3 - y1) * (y3 - y1)));

                    //Check if the city is closest to (split, 0) and (split, 100)
                    if (distanceBL < bottomLeft.coordinate)
                    {
                        bottomLeft.id = c;
                        bottomLeft.coordinate = distanceBL;
                    }
                    if (distanceBR < bottomRight.coordinate)
                    {
                        bottomRight.id = c;
                        bottomRight.coordinate = distanceBR;
                    }
                }
            }

            //Discard current split if it resulted in a single city being
            //both bottomRight/bottomLeft or topRight/topLeft
            if ((topList.Count <= 1) || (bottomList.Count <= 1))
            {
                return;
            }

            //Adding the corner cities to the top/bottomFinal lists
            //Removing the corner cities from the top/bottom lists
            topFinal.Add(topLeft.id);
            topFinal.Add(topRight.id);
            bottomFinal.Add(bottomRight.id);
            bottomFinal.Add(bottomLeft.id);
            topList.Remove(topLeft.id);
            topList.Remove(topRight.id);
            bottomList.Remove(bottomLeft.id);
            bottomList.Remove(bottomRight.id);

            //Call insertNode function to generate the new greedy paths
            InsertNode.insertNode(cityArray, topList, ref topFinal);
            InsertNode.insertNode(cityArray, bottomList, ref bottomFinal);

            //Combine the top and bottom paths to complete the tour
            topFinal.AddRange(bottomFinal);
            topFinal.Add(topFinal[0]);

            //If the path's distance wasn't the most recent path to be computed
            //find the distance of the path
            if (topFinal.SequenceEqual(globalVars.lastPath) == false)
            {
                globalVars.unique++;
                for (int d = 1; d < topFinal.Count; d++)
                {
                    int xy1 = topFinal[d - 1] - 1; //Location of city A in city List
                    int xy2 = topFinal[d] - 1;     //Location of city B in city List

                    //Find x and y coordinates of city A and B
                    float x1 = cityArray[xy1].xCoordinate;
                    float x2 = cityArray[xy2].xCoordinate;
                    float y1 = cityArray[xy1].yCoordinate;
                    float y2 = cityArray[xy2].yCoordinate;

                    //Use distance equation to find the distance between city A and B
                    //Add distance to totalDistance
                    distance = (float)Math.Sqrt((((x2 - x1)) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                    totalDistance = totalDistance + distance;
                }

                //If the new path distance is smaller than the
                //current smallest distance then replace
                //the smallest distance with the new distance
                if (totalDistance < globalVars.shortDistance)
                {
                    globalVars.shortDistance = totalDistance;  //Store the new shorter distance
                    globalVars.shortList = topFinal;           //Store the corresponding path
                    globalVars.shortSplit = split;             //Store the current split value
                }

                //Store the most recent path to avoid
                //computing the same path multiple times
                globalVars.lastPath = topFinal;

                //Create newChild with topFinal path and corresponding distance
                newChild tempChild = new newChild(totalDistance, topFinal);

                //Rearange the tempChilds path so that it starts and ends with "1"
                tempChild.path.RemoveAt(topFinal.Count - 1);
                for (int i = 0; i < tempChild.path.Count; i++)
                {
                    i = 0;
                    if (tempChild.path[0] == 1)
                    {
                        break;
                    }
                    else
                    {
                        tempChild.path.Add(tempChild.path[0]);
                        tempChild.path.RemoveAt(0);
                    }
                }
                tempChild.path.Add(tempChild.path[0]);

                //Sort the tempChild
                SortChild.sortChild(tempChild, ref globalVars, ref childList);
            }

        }
    }
}







