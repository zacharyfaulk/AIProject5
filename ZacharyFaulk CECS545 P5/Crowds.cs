using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class Crowds
    {
        public static void crowds(int tourLength, ref globalVars globalVars)
        {
            int max = 0;
            int tempCity = 0;
            int tempIndex = 0;
            int tempIndex2 = 0;
            int[] crowdArray = new int[tourLength + 1];
            //globalVars.shortList.Add(1);

            //Do for all city elements
            for(int i = 0; i < tourLength; i++)
            {
                //Console.WriteLine("Thefinal  Path is " + string.Join(",", globalVars.shortList));
                Array.Clear(crowdArray, 0, crowdArray.Length);

                //Do for each tour in the wisdomList
                for(int j = 0; j < globalVars.crowdSize; j++)
                {
                    //Console.WriteLine("The before Path is " + string.Join(",", globalVars.wisdomList[j].path));
                    
                    //Console.WriteLine(tempCity = globalVars.wisdomList.Count);
                    //Console.WriteLine(globalVars.wisdomList[1].path[0]);
                    if ((globalVars.wisdomList[j].path[i]) != 0)
                    {
                        tempCity = globalVars.wisdomList[j].path[i];
                        //for(int z = 0; z < crowdArray.Length; z++)
                        //{
                        //    Console.Write(crowdArray[z] + ",");
                        //}
                        //Console.WriteLine();
                        //??????????????????????????????????????????????
                        crowdArray[tempCity] = (crowdArray[tempCity] + 1);
                    }
                }

                max = crowdArray.Max();
                tempIndex = crowdArray.ToList().IndexOf(max);
                globalVars.shortList.Add(tempIndex);

                for(int k = 0; k < globalVars.crowdSize; k++)
                {
                    tempIndex2 = globalVars.wisdomList[k].path.IndexOf(tempIndex);
                    globalVars.wisdomList[k].path.RemoveAt(tempIndex2);
                    globalVars.wisdomList[k].path.Insert(tempIndex2, 0);
                    //Console.WriteLine("The after Path is " + string.Join(",", globalVars.wisdomList[k].path));
                }
            }
            globalVars.shortList.Add(1);
        }
    }
}
