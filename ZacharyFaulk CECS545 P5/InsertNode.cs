using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class InsertNode
    {
        //Start with finalList that has the left and right most cities in it
        //Find the middle of an edge that each startList city is clostest to
        //and insert the city between the cities that create the edge
        public static void insertNode(newCity[] cityArray, List<int> startList, ref List<int> finalList)
        {
            float tempDistance = 0;
            //newCorner object that stores data about the current startList city
            newCorner tempNode = new newCorner(float.MaxValue);

            //For each startList city
            for (int i = 0; i < startList.Count; i++)
            {
                tempNode.coordinate = float.MaxValue;

                //Check the distance between the center of each
                //finalList edge and the startList city
                for (int j = 1; j < (finalList.Count); j++)
                {
                    int xy1 = finalList[j - 1] - 1;     //City A in finalList
                    int xy2 = finalList[j] - 1;         //City B in finalList
                    int xy3 = startList[i] - 1;         //current startList city

                    //Finding x and y coordinates for each city
                    float x1 = cityArray[xy1].xCoordinate;
                    float x2 = cityArray[xy2].xCoordinate;
                    float x3 = cityArray[xy3].xCoordinate;
                    float y1 = cityArray[xy1].yCoordinate;
                    float y2 = cityArray[xy2].yCoordinate;
                    float y3 = cityArray[xy3].yCoordinate;

                    //Find middle between city A and B
                    float _x1 = ((x1 + x2) / 2);
                    float _y1 = ((y1 + y2) / 2);

                    //Calculate distance between the startList city and the middle between A and B
                    tempDistance = (float)Math.Sqrt((((x3 - _x1)) * (x3 - _x1)) + ((y3 - _y1) * (y3 - _y1)));

                    //Keep track of the which edge center was
                    //closest to the startList city
                    if (tempDistance < tempNode.coordinate)
                    {
                        tempNode.coordinate = tempDistance;
                        tempNode.id = j;
                        tempNode.tempID = cityArray[xy3].id;
                    }
                }

                //Add the startList city between the cities it was closest to
                if (tempNode.coordinate < float.MaxValue)
                {
                    finalList.Insert(tempNode.id, tempNode.tempID);
                }
            }
        }
    }
}
