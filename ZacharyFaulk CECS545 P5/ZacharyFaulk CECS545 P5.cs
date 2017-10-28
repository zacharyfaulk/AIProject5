using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ZacharyFaulk_CECS545_P5
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Code needed to run the Windows Form Application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string fileName = "Random222.TSP";    //File name being tested

            //Variables to keep track of execution time
            Stopwatch stopWatch = new Stopwatch();
            float time1 = 0;
            string time2 = null;
            string timer = null;
            stopWatch.Start();      //Start Stopwatch

            //Temp float to keep track of 
            //shortest distance of each generation
            float tempDistance = float.MaxValue;
            int z = 0;              //Temp variable
            int gen = 0;            //Keeps track of current generation
            int count = 0;          //Count variable
            double split = 2;       //Split variables
            float split2 = 0;
            float distance = 0;
            float totalDistance = 0;

            globalVars globalVars = new globalVars();           //Object to hold "global" variables
            List<newChild>[] genList = new List<newChild>[2];   //Array to hold the children of each generation
            genList[0] = new List<newChild>();      //Swaps between holding the parents and children of a generation
            genList[1] = new List<newChild>();      //Swaps between holding the parents and children of a generation

            //Reads file to find # of lines
            //Stores (# of lines  - 7) to get the number of cities in the file
            string[] lines = File.ReadAllLines(fileName);
            int newLength = lines.Length - 7;

            //Array of newCity objects with size = # of cities
            newCity[] cityArray = new newCity[newLength];   

            //Stores city data
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                //Ignore first 7 lines of the file
                //8th line is where the city data begins
                if (count >= 7)
                {
                    string[] data = line.Split(' ');    //Split and store the city data in data string array

                    //Create newCity object using data pulled from data array (ID, x Coordinate, y Coordinate)
                    //Store each newCity object in newCity array
                    cityArray[count - 7] = new newCity(Int32.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
                }
                count++;
            }
            //Close File
            file.Close();

            //If input for GA is choosen to be random
            if (globalVars.random == true)
            {
                Console.WriteLine("Random Inputs");
                Random rnd = new Random();
                List<int> temp1 = new List<int>();  //Temp list

                //Create a list with all the cities in it
                for (int l = 0; l < newLength; l++)
                {
                    temp1.Add(cityArray[l].id);
                }
                temp1.RemoveAt(0);      //Remove "1" from the list

                //Create "population size" of random tours 
                for (int r = 0; r < globalVars.maxChildren; r++)
                {

                    List<int> temp2 = new List<int>(temp1);
                    int n = temp2.Count;

                    //Scramble the list containing the cities
                    while (n > 1)
                    {
                        n--;
                        int rand = rnd.Next(n + 1);
                        z = temp2[rand];
                        temp2[rand] = temp2[n];
                        temp2[n] = z;
                    }

                    //Add "1" to the start and end of the list to complete the tour
                    temp2.Insert(0, 1);
                    temp2.Add(1);

                    //Calculate the tour distance
                    for (int d = 1; d < temp2.Count; d++)
                    {
                        int xy1 = temp2[d - 1] - 1; //Location of city A in city List
                        int xy2 = temp2[d] - 1;     //Location of city B in city List

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

                    //Create the newChild object using the randomized list 
                    //and the corresponding distance and then sort the child in genList[0]
                    newChild tempChild = new newChild(totalDistance, temp2);
                    SortChild.sortChild(tempChild, ref globalVars, ref genList[0]);
                }
            }
            //If input for GA is choosen to use the tours found by the greedy algorithm
            else
            {
                Console.WriteLine("Greedy Inputs");
                //Split the map of cities at y = split and x = split
                //to generate solutions to use as the initial parents for the GA
                for (split = 2; split < 100; split += 0.5)
                {
                    split2 = Convert.ToSingle(split);
                    //Call the split function to split the map of cities at y = split
                    Split.splitY(cityArray, split2, ref globalVars, ref genList[0]);
                    //Call the split function to split the map of cities at x = split
                    Split.splitX(cityArray, split2, ref globalVars, ref genList[0]);
                }
            }

            /*
            for (int p = 0; p < genList[0].Count; p++)
            {
                Console.WriteLine(string.Join(",", (genList[0])[p].distance));
            }
            Console.WriteLine(genList[0].Count);
            for (int p = 0; p < globalVars.wisdomList.Count; p++)
            {
                Console.WriteLine(string.Join(",", globalVars.wisdomList[p].distance));
            }
            Console.WriteLine(globalVars.wisdomList.Count);
            Console.ReadKey();
            */

            //Calls GA functions until the same distance
            //of the top child is found "globalVars.sameGen" times
            Console.WriteLine("Start GA");
            for (z = 0; z < globalVars.sameGen; z++)
            {                
                gen++;      //Keeps track of the current generation

                //Calls newGen function to create the next generation
                //and return the child with the shortest distance
                newChild topChild = NewGen.newGen(cityArray, ref globalVars, ref genList);

                //Checks if the top child from the generation is more fit
                //than the top child from the last generation
                tempDistance = topChild.distance;
                //Console.WriteLine("gen " + gen + "          " + tempDistance);
                if(tempDistance < globalVars.shortDistance)
                {
                    //If the child is more fit than the previous generation
                    //set the new distance as the shorest distance
                    //and reset the GA stop condition
                    z = 0;
                    globalVars.shortDistance = tempDistance;
                    globalVars.shortList.Clear();
                    globalVars.shortList.AddRange(topChild.path);
                }
            }
            Console.WriteLine("The Old Distance is " + globalVars.shortDistance);
            Console.WriteLine("The Old Path is " + string.Join(",", globalVars.shortList));
            globalVars.shortDistance = 0;
            globalVars.shortList.Clear();

            /*for (int p = 0; p < globalVars.crowdSize; p++)
            {
                Console.WriteLine(p + "    " + globalVars.wisdomList[p].distance);
            }
            Console.WriteLine(globalVars.wisdomList.Count);
            Console.ReadKey();*/

            Console.WriteLine("Start WOC");
            Console.WriteLine("WOC List Length = " + globalVars.wisdomList.Count);
            Crowds.crowds(newLength, ref globalVars);
            //Console.WriteLine("The New Path is " + string.Join(",", globalVars.shortList));
            for (int d = 1; d < globalVars.shortList.Count; d++)
            {
                int xy1 = globalVars.shortList[d - 1] - 1; //Location of city A in city List
                int xy2 = globalVars.shortList[d] - 1;     //Location of city B in city List

                if(xy1 < 0 || xy2 < 0)
                {
                    Console.WriteLine("Bad Solution");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

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
            globalVars.shortDistance = totalDistance;


            //for (int p = 0; p < globalVars.crowdSize; p++)
            //{
            //    Console.WriteLine(p + "    " + globalVars.wisdomList[p].distance);
            //}
            //Console.WriteLine(globalVars.wisdomList.Count);
            //Console.WriteLine("The Old Path is " + string.Join(",", globalVars.shortList));
            //Console.ReadKey();

            stopWatch.Stop();   //Stop Stopwatch

            //Print distance/path/time data
            Console.WriteLine("File = " + fileName);
            Console.WriteLine("The Distance is " + globalVars.shortDistance);
            Console.WriteLine("The New Path is " + string.Join(",", globalVars.shortList));
            time1 = stopWatch.ElapsedMilliseconds;
            time1 = (float)TimeSpan.FromMilliseconds(time1).TotalSeconds;
            time2 = stopWatch.ElapsedMilliseconds.ToString();
            timer = time1.ToString();
            Console.WriteLine("Execution time took " + time2 + " Milliseconds, or " + timer + " Seconds");

            //Run Windows Form Application to graph the shortest path
            Application.Run(new GUI(cityArray, globalVars.shortList));

            Console.ReadKey();
        }
    }

    //newCity class that creates newCity objects
    //Each object has an ID, x coordinate, y coordniate
    public class newCity
    {
        public int id;
        public float xCoordinate;
        public float yCoordinate;
            
        public newCity(int id, float x, float y)
        {
            this.id = id;
            this.xCoordinate = x;
            this.yCoordinate = y;
         }
    }

    //newCorner class that creates newCorner objects
    //Each object has an ID, tempID, coordinate float
    //Used to find the left and right most cities on each side of the split map
    public class newCorner
    {
        public int id;
        public int tempID;
        public float coordinate;

        public newCorner(float x)
        {
            this.coordinate = x;
        }
    }

    //newChild class that creates newChild objects
    //Each object has a path and distance
    public class newChild
    {
        public float distance;
        public List<int> path;

        public newChild(float pathDistance, List<int> newPath)
        {
            this.distance = pathDistance;
            this.path = new List<int>(newPath);
        }

    }

    //globalVars class to create the globalVars object
    //Holds the "global" variables used by all the funtions
    public class globalVars
    {
        //GA parameters
        /////////////////////////////////////////////
        public bool random = false;         //Determines what initial parents the GA will use
        public int maxChildren = 500;       //Population size
        public int pmx = 15;                //Size of the path that will swapped with the pmx crossover
        public int sameGen = 1000;           //Stopping point of GA, stops when the same distance of the top child is found x many times
        public double mutation = 5;         //Mutation rate
        public int crowdSize = 500;
        /////////////////////////////////////////////

        public int unique = 0;              //Counts the amount of unique solutions from the Split functions
        public int genVar = 0;              //Determines which list in genList[] are the parents are which are the children 
        public float shortSplit = 0;        //Shortest split found by the Split functions
        public float shortDistance = float.MaxValue;    //holds the shortest distance found by the GA algorithm
        public List<int> shortList = new List<int>();   //holds the shortest tour found by the GA algorithm 
        public List<int> lastPath = new List<int>();    //holds the last found tour by the Split functions to prevent duplicate tours
        public List<newChild> wisdomList = new List<newChild>();
    }
}

