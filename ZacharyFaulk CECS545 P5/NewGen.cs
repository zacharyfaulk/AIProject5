using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P5
{
    class NewGen
    {
        //Sets up for the pmx crossover thats implemented in the NewGenChild class
        public static newChild newGen(newCity[] cityArray, ref globalVars globalVars, ref List<newChild>[] genList)
        {
            //Random variables/functions
            int rand = 0;
            Random rnd = new Random();
            Random rnd2 = new Random(Guid.NewGuid().GetHashCode());

            //Temp lists to feed the newGenChild function to execute the pmx crossover
            List<int> temp1 = new List<int>();
            List<int> temp2 = new List<int>();
            List<int> pmx1 = new List<int>();
            List<int> pmx2 = new List<int>();
            List<int> tempFinal = new List<int>();

            //Copy the best parent from the current generation to the next generation
            if (globalVars.genVar == 0)
            {
                SortChild.sortChild(((genList[0])[0]), ref globalVars, ref genList[1]);
            }
            else
            {
                SortChild.sortChild(((genList[1])[0]), ref globalVars, ref genList[0]);
            }

            //Do for each parent in the current generation
            for (int i = 0; i < genList[globalVars.genVar].Count; i++)
            {
                //Variable to stop an infinte loop when trying
                //to find unique substrings from the parents
                int help = 0;

                //Clear temp lists
                temp1.Clear();
                temp2.Clear();
                tempFinal.Clear();
                pmx1.Clear();
                pmx2.Clear();

                //Parent 1, which will be all the parents
                //in the current generation
                temp1.AddRange(((genList[globalVars.genVar])[i]).path);
                temp1.RemoveAt(temp1.Count - 1);

                //Randomly pick parent 2 while checking that
                //it's not parent 1
                rand = rnd.Next(0, genList[globalVars.genVar].Count);
                while (((rand = rnd.Next(0, genList[globalVars.genVar].Count)) == i)) { } 
                temp2.AddRange(((genList[globalVars.genVar])[rand]).path);
                temp2.RemoveAt(temp2.Count - 1);

                //Generate the substrings starting at a random point
                //in the lists with globalVars.pmx length
                rand = rnd.Next(0, temp1.Count - globalVars.pmx);
                for (int p = rand; p < (rand + globalVars.pmx); p++)
                {
                    pmx1.Add(temp1[p]);
                    pmx2.Add(temp2[p]);
                }

                //Checks to make sure the substrings are not equal
                //to prevent the crossover from doing nothing
                //Will only attempt 100 times before exiting
                //to prevent infine loops when both parents are equal
                while ((pmx1.SequenceEqual(pmx2) == true) && (help < 100))
                {
                    pmx1.Clear();
                    pmx2.Clear();
                    rand = rnd.Next(1, temp1.Count - globalVars.pmx);
                    for (int p = rand; p < (rand + globalVars.pmx); p++)
                    {
                        pmx1.Add(temp1[p]);
                        pmx2.Add(temp2[p]);
                    }
                    help++;
                }
                
                //Call the newGenChild function to perform the pmx crossover
                //on both parents with the other parents subsections to create 2 new children
                //rand2 is used to determine if mutation will occur within the function
                double rand2 = rnd2.NextDouble();
                NewGenChild.newGenChild(cityArray, ref globalVars, ref genList, rand, temp1, pmx2, rand2);
                rand2 = rnd2.NextDouble();
                NewGenChild.newGenChild(cityArray, ref globalVars, ref genList, rand, temp2, pmx1, rand2);

            }
            
            //Once all the children have been created
            //Swap the globalVar.genVar so that the new
            //children become the new parents and the current
            //parents are cleared to make space for the next generation of children
            //The best child from the generation is then returned to determine
            //if the generation inproved fitness
            if (globalVars.genVar == 0)
            {
                globalVars.genVar = 1;
                genList[0].Clear();
                return ((genList[1])[0]);
            }
            else
            {
                globalVars.genVar = 0;
                genList[1].Clear();
                return ((genList[0])[0]);
            }

        }

    }
}

