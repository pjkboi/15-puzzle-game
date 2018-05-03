//Program Name : PROG2370 - Assignment 3
//    Purpose : To create a 15 puzzle game with save, load,  and user generated game size
//    Written by : PJ & Tuan

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A3GameV1
{
    public partial class FifteenPuzzle : Form
    {
        const int DEFAULT_COLUMNS = 4;
        const int DEFAULT_ROWS = 4;
        const int TOP = 40;
        const int LEFT = 50;
        const int MAX_ROWS = 7;
        const int MIN_COLUMNS = 2;
        const int MIN_ROWS = 2;
        const int MAX_COLUMNS = 7;
        const int height = 50;
        const int width = 50;
        const int margin = 100;
        int GAME_HEIGHT = 200;
        int GAME_WIDTH = 200;
        string winString;
        Square[,] squares;
        List<char> allMoves;
        int number_rows = 4;
        int number_columns = 4;

        public int Number_rows
        {
            get { return number_rows; }
            set { number_rows = value; }
        }
        public int Number_columns
        {
            get { return number_columns; }
            set { number_columns = value; }
        }

        //Loads the game with a  randomized 4x4 game.
        public FifteenPuzzle()
        {
            InitializeComponent();

            squares = new Square[DEFAULT_ROWS, DEFAULT_COLUMNS];

            string initialOrder = "";
            for (int i = 1; i < DEFAULT_ROWS * DEFAULT_COLUMNS; i++)
            {
                initialOrder += i.ToString() + "_";
            }

            initialOrder += "-1";
            createGrid(number_rows, number_columns, initialOrder);

            Shuffle();
            
        }

        //checking the clicked square to move in a specified direction
        public void selectDirection(Square square)
        {
            char direction;
            direction = ' ';
            Square temp;
            temp = square;

            if((square.Column != number_columns - 1) && (squares[square.Row, square.Column + 1] == null))
            {
                direction = 'r';
            }
            else if ((square.Column !=0) && (squares[square.Row, square.Column -1] == null))
            {
                direction = '1';
            }
            else if((square.Row != number_rows - 1) && (squares[square.Row + 1, square.Column] == null))
            {
                direction = 'd';
            }
            else if ((square.Row !=0) && (squares[square.Row -1, square.Column] == null))
            {
                direction = 'u';
            }

            moveSquare(square, direction);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //Moving the squares in a specified direction
        private void moveSquare(Square square, char direction)
        {
            switch(direction)
            {
                case '1':
                    squares[square.Row, square.Column - 1] = square;
                    squares[square.Row, square.Column] = null;
                    square.Column--;
                    square.Left -= square.Width;
                    break;
                case 'r':
                    squares[square.Row, square.Column + 1] = square;
                    squares[square.Row, square.Column] = null;
                    square.Column++;
                    square.Left += square.Width;
                    break;
                case 'u':
                    squares[square.Row -1, square.Column] = square;
                    squares[square.Row, square.Column] = null;
                    square.Row--;
                    square.Top -= square.Height;
                    break;
                case 'd':
                    squares[square.Row + 1, square.Column] = square;
                    squares[square.Row, square.Column] = null;
                    square.Row++;
                    square.Top += square.Height;
                    break;

            }
        }

        //Checking for a empty spot
        private int[] findEmptySquare()
        {
            int[] position = new int[2];
            for(int i = 0; i < number_rows; i++)
            {
                for(int j = 0; j < number_columns; j++)
                {
                    if(squares[i,j] == null)
                    {
                        position[0] = i;
                        position[1] = j;
                    }
                }
            }

            return position;
        }

        //Creates game size from the user, validates user input for the row and column textboxes
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string rowInput = txtRowInput.Text;
            string columnInput = txtColumnInput.Text;

            int row;
            int column;

            if(!int.TryParse(rowInput, out row))
            {
                MessageBox.Show("Not a valid input. \nCannot be empty. \nMust be numerical.");
                return;
            }

            if (!int.TryParse(columnInput, out column))
            {
                MessageBox.Show("Not a valid input. \nCannot be empty. \nMust be numerical.");
                return;
            }

            number_rows = row;
            number_columns = column;


            if (number_rows <= 0 && number_columns <= 0)
            {
                MessageBox.Show("Row and Column Cannot be Zero or less.");
                return;
            }

            if (number_rows <= 0)
            {
                MessageBox.Show("Row Cannot be Zero or less.");
                return;
            }

            else if (number_columns <= 0)
            {
                MessageBox.Show("Column Cannot be Zero or less.");
                return;
            }

            if (number_rows < MIN_ROWS && number_columns < MIN_COLUMNS)
            {
                MessageBox.Show("Puzzle Size Cannot be 1x1.");
                return;

            }

            panelGameBoard.Controls.Clear();


            squares = new Square[number_rows, number_columns];

            string initialOrder = "";

            for (int i = 1; i < number_rows * number_columns; i++)
            {
                initialOrder += i.ToString() + "_";
            }

            initialOrder += "-1";
            createGrid(number_rows, number_columns, initialOrder);
           
            Shuffle();

        }

        //Creates the gamegrid
        private void createGrid(int rows, int columns, string numbers)
        {
            
            foreach (Square square in squares)
            {
                Controls.Remove(square);
            }

            squares = new Square[rows, columns];

            int x;
            int y = TOP;

            GAME_HEIGHT = number_rows * height;
            GAME_WIDTH = number_columns * width;
            panelGameBoard.Size = new System.Drawing.Size(GAME_HEIGHT+margin, GAME_WIDTH+margin);


            string[] allNumbers = numbers.Split(new char[] { '_' });

            int counter = 0;
            for (int i = 0; i < number_rows; i++)
            {
                x = LEFT;
                for (int j = 0; j < number_columns; j++)
                {
                    int number = int.Parse(allNumbers[counter]);
                    if (number > 0)
                    {
                        squares[i, j] = new Square(height, width, y, x, number.ToString(), i, j, this);
                    }
                    counter++;
                    x += width;
                }
                y += height;
            }
            foreach (Square square in squares)
            {
                panelGameBoard.Controls.Add(square);
            }

            allMoves = new List<char>();
            winString = "";
            for(int i =1; i< number_rows * number_columns; i++)
            {
                winString += i + "_";
            }
        }

        //Randomizes the the puzzle
        public void Shuffle()
        {
            int shuffleCalculation = number_rows * number_columns * 5;
            Random random = new Random();
            for(int i = 0; i < shuffleCalculation; i++)
            {
                int value = random.Next(4);
                switch(value)
                {
                    case 0:
                        if(moveByDirection("A", false))
                        {
                            allMoves.Add('D');
                        }
                        break;
                    case 1:
                        if (moveByDirection("S", false))
                        {
                            allMoves.Add('W');
                        }
                        break;
                    case 2:
                        if (moveByDirection("D", false))
                        {
                            allMoves.Add('A');
                        }
                        break;
                    case 3:
                        if (moveByDirection("W", false))
                        {
                            allMoves.Add('S');
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //Checks keyboard input(direction) 
        private bool moveByDirection(string direction, bool showMoves)
        {
            int emptySquareRow = findEmptySquare()[0];
            int emptySquareColumn = findEmptySquare()[1];


            switch(direction)
            {
                case "A":
                    if(emptySquareColumn != number_columns - 1)
                    {
                        moveSquare(squares[emptySquareRow, emptySquareColumn + 1], '1');
                        return true;
                    }
                    break;
                case "D":
                    if(emptySquareColumn != 0)
                    {
                        moveSquare(squares[emptySquareRow, emptySquareColumn - 1], 'r');
                        return true;
                    }
                    break;
                case "S":
                    if(emptySquareRow !=0)
                    {
                        moveSquare(squares[emptySquareRow - 1, emptySquareColumn], 'd');
                        return true;
                    }
                    break;
                case "W":
                    if(emptySquareRow != number_rows - 1)
                    {
                        moveSquare(squares[emptySquareRow + 1, emptySquareColumn], 'u');
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        //Check if puzzle has been solved
        public bool checkWinner()
        {
            bool result = false;
            string order = "";

            foreach(Square square in squares)
            {
                if (square != null)
                {
                    order += square.Text + "_";
                }
            }

            if (order == winString && squares[number_rows -1, number_columns -1] == null )
            {
                MessageBox.Show("Puzzle Solved. You are a Winner");
                result = true;
                allMoves = new List<char>();
                Shuffle();
            }
            return result;
        }

        //Controling keyboard input.
        private void FifteenPuzzle_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!txtRowInput.ContainsFocus && !txtColumnInput.ContainsFocus)
            {
                string key = e.KeyCode.ToString();
                if (moveByDirection(key, false))
                {
                    switch (key)
                    {
                        case "S":
                            allMoves.Add('W');
                            break;
                        case "A":
                            allMoves.Add('D');
                            break;
                        case "D":
                            allMoves.Add('A');
                            break;
                        case "W":
                            allMoves.Add('S');
                            break;
                        default: break;
                    }
                    checkWinner();

                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SaveFileDialog saving = new SaveFileDialog();
            if (saving.ShowDialog() == DialogResult.OK)
            {
                //string filename = "save.txt";
                Stream filename = File.Open(saving.FileName, FileMode.CreateNew);
                StreamWriter writer = new StreamWriter(filename);
                writer.WriteLine(number_columns);
                writer.WriteLine(number_rows);
                foreach (Square square in squares)
                {
                
                    writer.WriteLine(square);
                
                }
                MessageBox.Show("Save Successful");
                writer.Close();
            }
            
            
            return;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string loadColumn;
            string loadRow;
            int row;
            int column;
            string loadOrder = "";
            string ordering = "";
            OpenFileDialog loading = new OpenFileDialog();
            if (loading.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = loading.FileName;
                StreamReader reader = new StreamReader(filename);
                loadColumn = reader.ReadLine();
                loadRow = reader.ReadLine();
                if (!int.TryParse(loadRow, out row))
                {
                    MessageBox.Show("Something is wrong with the save");
                    return;
                }

                if (!int.TryParse(loadColumn, out column))
                {
                    MessageBox.Show("Something is wrong with the save");
                    return;
                }
                panelGameBoard.Controls.Clear();
                squares = new Square[row, column];
                for (int i = 0; i < row * column; i++)
                {
                    loadOrder = reader.ReadLine();
                    if (loadOrder == "")
                    {
                        loadOrder = "-1";
                    }
                    else
                    {
                        loadOrder = loadOrder.Remove(0, 22);
                    }


                    ordering += loadOrder + "_";
                }
                reader.Close();
                createGrid(row, column, ordering);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panelGameBoard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
