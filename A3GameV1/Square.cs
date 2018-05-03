//Program Name : PROG2370 - Assignment 3
//    Purpose : Class to model the squares in the puzzle.
//    Written by : PJ & Tuan

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A3GameV1
{
    public class Square : Button
    {
        private FifteenPuzzle game;

        private int row;
        public int Row
        {
            get { return row;  }
            set { row = value; }
        }

        private int column;
        public int Column
        {
            get { return column;  }
            set { column = value; }
        }

        //Constructor for Squares
        //height - height of the square
        //width - width of the square
        //Top - distance of the square from the top of the form
        //text - display text on square
        //row - number of rows in puzzle
        //columns - number of columns
        //game - the game form
        public Square(int height, int width, int top, int left, string text, int row, int column, FifteenPuzzle game)
        {
            this.Height = height;
            this.Width = width;
            this.Top = top;
            this.Left = left;
            this.Text = text;
            this.Click += Square_Click;

            this.row = row;
            this.Column = column;
            this.game = game;    
        }
        public void Square_Click(object sender, EventArgs e)
        {
            game.selectDirection(this);
            game.checkWinner();
        }
    }
}
