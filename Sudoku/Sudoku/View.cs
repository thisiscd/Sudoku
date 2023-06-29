namespace Sudoku
{
    public class View
    {
        private readonly Controller C;
        private Board B { get; }
        private int x;
        private int y;
        public Tuple<int, int>[,] coord;
        public bool run = false;
       
        public View(ref Controller c, ref Board b)
        {
            B = b;
            C = c;
            x = 0;
            y = 0;
            run = true;

            coord = new Tuple<int, int>[9, 9];
        }

        internal void Init()
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
                            var (Left, Top) = Console.GetCursorPosition();
                            coord[m * 3 + i, n * 3 + j] = new Tuple<int, int>(Left, Top);
                            var cell = B.cells[m][i][n][j];
                            if (cell.kind == 0)
                                Console.ForegroundColor = ConsoleColor.Red;
                            if (cell.value == 0)
                                Console.Write("$ ");
                            else
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


        public void ShowBoard()
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
                            if (cell.value == 0)
                                Console.Write('$');
                            else
                                Console.Write("{0} ", cell.value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }

        }

        public void Parse(ConsoleKey key)
        {
            if(key == ConsoleKey.UpArrow && x > 0) 
            {
                x--;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(key == ConsoleKey.DownArrow && x < 8) 
            {
                x++;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(key == ConsoleKey.LeftArrow && y > 0)
            {
                y--;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(key == ConsoleKey.RightArrow && y < 8) 
            {
                y++;
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
            }
            if(key > ConsoleKey.D0 && key <= ConsoleKey.D9)
            {
                WriteAt(char.ToString(((char)key)), x, y);
            }
            if (key == ConsoleKey.N)
            {
                B.NextBoard();
            }
            if (key == ConsoleKey.P)
            {
                B.PrevBoard();
            }
            if (key == ConsoleKey.F)
            {
                Refresh();
            }
            if (key == ConsoleKey.Q)
            {
                run = false;
            }
        }

        public void Refresh()
        {
            ShowBoard();
            if(C.Option["-n"])
            {
                Console.SetCursorPosition(40, 1);
                Console.Write("已生成 {0} 个游戏", C.GameNum);
                Console.SetCursorPosition(40, 3);
                if (C.Option["-m"])
                {
                    Console.Write("难度: {0} ", C.Hard);
                }
                else if(C.Option["-r"])
                {
                    Console.Write("挖空数量: {0} ~ {1}", C.MinEmptyNum, C.MaxEmptyNum);
                }
                else if (C.Option["-u"])
                {
                    Console.Write("解唯一: {0}", C.Option["-u"]);
                }
            }
            else if (C.Option["-c"])
            {
                Console.SetCursorPosition(40, 1);
                Console.Write("已生成 {0} 个终局", C.EndNum);
            }
            else if (C.Option["-s"])
            {
                Console.SetCursorPosition(40, 1);
                Console.Write("已解 {0} 个数独", B.storage.Length);
            }

            Console.SetCursorPosition(40, 5);
            Console.Write("当前棋盘： {0} / {1}", B.index + 1, B.storage.Length);
            Console.SetCursorPosition(40, 9);
            Console.Write("[n]: 下一个    [p]: 上一个 ");
            Console.SetCursorPosition(40, 11);
            Console.WriteLine("[方向键]: 移动光标    [数字键(英)]: 填空    [q]: 退出");
            Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
        }


        protected void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(coord[x, y].Item1, coord[x, y].Item2);
                Console.Write(s);
                B.SetCellVal(x / 3, x % 3, y / 3, y % 3, int.Parse(s));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
