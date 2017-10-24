using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZacharyFaulk_CECS545_P5
{
    public partial class GUI : Form
    {
        //New List to keep track permutation that needs to be drawn
        //new city array to keep track of city data
        List<int> _iList = new List<int>();
        newCity[] _cityArray;

        public GUI(newCity[] cityArray, List<int> iList)
        {
            _iList = iList;
            _cityArray = cityArray;
            InitializeComponent();
        }

        public void P1_GUI_Paint(object sender, PaintEventArgs e)
        {
            //Pen/Brush creation
            Pen black = new Pen(Color.Black, 3);
            SolidBrush green = new SolidBrush(Color.Green);

            //For loop that draws the shortest path
            //Similar to loop that calculates distance
            for (int d = 1; d < _iList.Count; d++)
            {
                int xy1 = _iList[d - 1] - 1;    //Location of city A in city List
                int xy2 = _iList[d] - 1;        //Location of city B in city List

                //Find x and y coordinates of city A and B
                //Multiplied by 8 to create a larger GUI
                float x1 = _cityArray[xy1].xCoordinate * 8;
                float x2 = _cityArray[xy2].xCoordinate * 8;
                float y1 = _cityArray[xy1].yCoordinate * 8;
                float y2 = _cityArray[xy2].yCoordinate * 8;
                PointF point1 = new PointF(x1, y1);     //City A location
                PointF point2 = new PointF(x2, y2);     //City b location

                //RectangleF rect = new RectangleF(x1 - 10, y1 - 10, 20, 20);
                int x1Int = Convert.ToInt32(x1);
                int y1Int = Convert.ToInt32(y1);

                //Create Label object for city A
                //Set parameters for new Label object
                Label cityLabel = new Label();
                cityLabel.Location = new Point (x1Int - 7, y1Int - 7);
                cityLabel.Text = _cityArray[xy1].id.ToString();
                cityLabel.BackColor = System.Drawing.Color.Green;
                cityLabel.AutoSize = true;
                cityLabel.MaximumSize = new Size(45, 15);
                this.Controls.Add(cityLabel);   //Draw Label in application

                //Draw Line from city A location to city B location
                e.Graphics.DrawLine(black, point1, point2);

            }
            e.Graphics.Dispose();
            //Application.Exit();
        }

    }
}
