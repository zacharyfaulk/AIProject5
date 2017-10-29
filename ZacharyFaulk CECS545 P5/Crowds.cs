using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class Crowds
    {
        //Looks through each tour in the wisdomList and finds
        //the most common city at each index for all the tours
        //to generate the final tour
        public static void crowds(int tourLength, ref globalVars globalVars)
        {
            //Temp variables for finding the most used city at each index
            int max = 0;
            int tempCity = 0;
            int tempIndex = 0;
            int tempIndex2 = 0;
            //Array to store the count of each city for each index
            int[] crowdArray = new int[tourLength + 1];     

            //Look through each index of the tours
            for(int i = 0; i < tourLength; i++)
            {
                //Reset the crowdArray after each index search
                Array.Clear(crowdArray, 0, crowdArray.Length);

                //Find the city of each tour at index i
                for(int j = 0; j < globalVars.crowdSize; j++)
                {
                    //If the city != 0, increment the count for
                    //that city in the crowdArray
                    if ((globalVars.wisdomList[j].path[i]) != 0)
                    {
                        tempCity = globalVars.wisdomList[j].path[i];
                        crowdArray[tempCity] = (crowdArray[tempCity] + 1);
                    }
                }

                //Find the city that was most used at
                //index i and add it to the final tour's path
                max = crowdArray.Max();
                tempIndex = crowdArray.ToList().IndexOf(max);
                globalVars.shortList.Add(tempIndex);

                //Replace the city that was just added in the final tour
                //with a 0 in the wisdomList tours
                for(int k = 0; k < globalVars.crowdSize; k++)
                {
                    tempIndex2 = globalVars.wisdomList[k].path.IndexOf(tempIndex);
                    globalVars.wisdomList[k].path.RemoveAt(tempIndex2);
                    globalVars.wisdomList[k].path.Insert(tempIndex2, 0);
                }
            }
            //Add a 1 to the end of the list to complete the tour
            globalVars.shortList.Add(1);
        }
    }
}
