using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class NewGenChild
    {
        public static void newGenChild(newCity[] cityArray, ref globalVars globalVars, ref List<newChild>[] genList, int rand, List<int> _tempList, List<int> _altPmx, double rand2)
        {
            //Temp variables
            int index;
            int tempVar;
            float distance = 0;
            float totalDistance = 0;

            //Random function and temp lists
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<int> tempList= new List<int>(_tempList);
            List<int> altPmx = new List<int>(_altPmx);
            List<int> tempMut = new List<int>();

            //Performs the pmx crossover by swapping the cities in the
            //parent's list according to the sublist from the other parent
            for (int j = rand; j < rand + globalVars.pmx; j++)
            {
                tempVar = tempList[j];
                index = tempList.IndexOf(altPmx[j - rand]);         
                tempList.RemoveAt(index);
                tempList.Insert(index, tempVar);
                tempList.RemoveAt(j);
                tempList.Insert(j, altPmx[j - rand]);
            }

            //Performs inversion mutation if rand2 was less than
            //the globalVars.mutation/100
            if (rand2 < (globalVars.mutation / 100))
            {
                tempMut.Clear();
                rand = rnd.Next(1, tempList.Count - globalVars.pmx);
                for (int k = rand; k < rand + globalVars.pmx; k++)
                {
                    tempMut.Add(tempList[k]);
                    tempList.RemoveAt(k);
                    tempList.Insert(rand, tempMut[k - rand]);
                }
            }

            tempList.Add(tempList[0]);

            //Calculate the tour distance
            for (int d = 1; d < tempList.Count; d++)
            {
                int xy1 = tempList[d - 1] - 1; //Location of city A in city List
                int xy2 = tempList[d] - 1;     //Location of city B in city List

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

            //Create newChild with tempList path and corresponding distance
            newChild tempChild = new newChild(totalDistance, tempList);

            //Sort the tempChild node in the not current parent genList
            if (globalVars.genVar == 0)
            {
                SortChild.sortChild(tempChild, ref globalVars, ref genList[1]);
            }
            else
            {
                SortChild.sortChild(tempChild, ref globalVars, ref genList[0]);
            }
            SortChild.sortChild(tempChild, ref globalVars, ref globalVars.wisdomList);

        }

    }
}