using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZacharyFaulk_CECS545_P1
{
    class Program 
    {
        [STAThread]
        static void Main(string[] args)
        {  
            //Code needed to run the Windows Form Application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string path = "Random4.TSP";    //File name being tested

            //Variables to keep track of execution time
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();  //Start Stopwatch
            float time1 = 0;
            string time2 = null;
            string timer = null;

            int count = 0;
            int count2 = 0;
            string shortPath = null;                //Stores a string of the shortest path
            float shortDistance = float.MaxValue;   //Keeps track of the shortest distance

            //Reads file to find # of lines
            //Stores (# of lines  - 7) to get the number of cities in the file
            string[] lines = File.ReadAllLines(path);
            int newLength = lines.Length - 7;

            List<int> iList = new List<int>();              //List to store the city IDs
            List<int> shortList = new List<int>();          //List to store the shortest city path
            newCity[] cityArray = new newCity[newLength];   //Array of newCity objects with size = # of cities

            //Start reading the file again line by line
            //Keep reading until a null line is found (end of file)
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                //Ignore first 7 lines of the file
                //8th line is where the city data begins
                if (count >= 7)
                {
                    //s is used to keep track of the city IDs to be stored in the List
                    //Starting with the 2nd city
                    int s = count - 6;
                    string[] data = line.Split(' ');    //Split and store the city data in data string array
                    int j = 0;
                    //Create newCity object using data pulled from data array (ID, x Coordinate, y Coordinate)
                    //Store each newCity object in newCity array
                    cityArray[count - 7] = new newCity(Int32.Parse(data[j]), float.Parse(data[j + 1]), float.Parse(data[j + 2]));

                    if (count >= 8)
                    {
                        //Add city ID to List starting with the 2nd City
                        iList.Add(s);
                        count2++;
                    }
                }
                count++;
            }
            //Close File
            file.Close();

            //Call permute function to start finding permutations and distances
            permute(cityArray, iList, 0, iList.Count - 1, ref shortDistance, ref shortList);

            stopWatch.Stop();   //Stop Stopwatch

            //Print time/distance/path data
            shortPath = string.Join(",", shortList);
            Console.WriteLine("File = " + path);
            Console.WriteLine("The Final Shortest Distance is " + shortDistance);
            Console.WriteLine("The Final Shortest Path is " + shortPath);
            time1 = stopWatch.ElapsedMilliseconds;
            time1 = (float)TimeSpan.FromMilliseconds(time1).TotalSeconds;
            time2 = stopWatch.ElapsedMilliseconds.ToString();
            timer = time1.ToString();
            Console.WriteLine("Execution time took " + time2 + " Milliseconds, or " + timer + " Seconds");

            //Run Windows Form Application to graph the shortest path
            Application.Run(new P1_GUI(cityArray, shortList, newLength));

            Console.ReadKey();
        }

        static void permute(newCity[] cityArray, List<int> list, int i, int n, ref float shortDistance, ref List<int> shortList)
        {
            List<int> iList = new List<int>(list);

            //Variables to help find permutations and distances
            int j;
            float distance = 0;         //Stores the distance between 2 cities
            float totalDistance = 0;    //Stores the total distance between all the cities

            //If New permutation has been found
            if (i == n)
            {
                //Insert a "1" at the beginning and end of the List of city IDs
                iList.Insert(0, 1);
                iList.Add(1);
                //Application.Run(new P1_GUI(cityArray, iList, n));

                //Calculate the distance between city A and city B
                //Until the distance between all the cities in the permutation have been found
                for (int d = 1; d < iList.Count; d++)
                {
                    int xy1 = iList[d - 1] - 1; //Location of city A in city List
                    int xy2 = iList[d] - 1;     //Location of city B in city List

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

                //If new totalDistance < shortDistance
                //Store new distance as the shortest distance
                //As well as the permutation that resulted in the shortest distance
                if (totalDistance < shortDistance)
                {
                    shortDistance = totalDistance;
                    shortList = iList;
                    //Console.WriteLine("The New Shortest Distance is " + shortDistance);
                    //Console.WriteLine("The New Shortest Path is " + shortPath);
                }
            }
            
            //If new permutation has not been found
            //Swap IDs in List of cities
            else
            {
                for (j = i; j <= n; j++)
                {
                    iList = swap(iList, i, j);
                    permute(cityArray, iList, i + 1, n, ref shortDistance, ref shortList);
                    iList = swap(iList, i, j);
                }
            }
        }

        //Swap function
        static List<int> swap(List<int> list, int i, int j)
        {
            List<int> newList = new List<int>(list);
            int tmp;
            tmp = newList[i];
            newList[i] = newList[j];
            newList[j] = tmp;
            return newList;
        }
    }

    //newCity class that creates newCity objects
    //Each object has an ID, x Coordinate, and y Coordinate
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
}

