using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class View
    {
        private Controller C;
        private Board B { get; }
        private int x;
        private int y;
        public Tuple<int, int>[,] coord;

        public View(ref Controller c, ref Board b)
        {
            B = b;
            C = c;
            x = 0;
            y = 0;
            coord = new Tuple<int, int>[9, 9];

            init();

        }

        private void init()
        {
            for (int m = 0; m < 3; m++)
            {
                if (m == 0) Console.WriteLine("".PadLeft(25, '-'));
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        if (n == 0) Console.Write("| ");
                        for (int j = 0; j < 3; j++)
                        {
                            var coor = Console.GetCursorPosition();
                            coord[m * 3 + i, n * 3 + j] = new Tuple<int, int>(coor.Left, coor.Top);
                            var cell = B.cells[m][i][n][j];
                            if (cell.kind == 0)
                                Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("{0} ", cell.value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write("| ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("".PadLeft(25, '-'));
            }
            Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
        }


        public void showBoard()
        {
            for (int m = 0; m < 3; m++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            var cell = B.cells[m][i][n][j];
                            Console.SetCursorPosition(coord[m * 3 + i, n * 3 + j].Item1, coord[m * 3 + i, n * 3 + j].Item2);
                            if (cell.kind == 0)
                                Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("{0} ", cell.value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }

        }

        internal void parse(ConsoleKeyInfo consoleKeyInfo)
        {
            if(consoleKeyInfo.Key == ConsoleKey.UpArrow && x > 0) 
            {
                x--;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
                //Console.SetCursorPosition(origCol + x, origRow + y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.DownArrow && x < 9) 
            {
                x++;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(consoleKeyInfo.Key == ConsoleKey.LeftArrow && y > 0)
            {
                y--;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(consoleKeyInfo.Key == ConsoleKey.RightArrow && y < 9) 
            {
                y++;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(consoleKeyInfo.Key > ConsoleKey.D0 && consoleKeyInfo.Key <= ConsoleKey.D9)
            {
                WriteAt(Char.ToString(consoleKeyInfo.KeyChar), x, y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.H)
            {
                help();
            }
            if (consoleKeyInfo.Key == ConsoleKey.S)
            {
                showBoard();
            }
        }

        private void help()
        {
            Console.WriteLine("help: ");
            // throw new NotImplementedException();
        }

        protected void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
                Console.Write(s);
                B.setCell(x / 3, x % 3, y / 3, y % 3, int.Parse(s));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
