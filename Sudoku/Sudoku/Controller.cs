using System;

namespace Sudoku
{
    public class Controller
    {
        // gen
        readonly int RESULT_NUM = 5;
        int[,] board;
        readonly bool[,] line;
        readonly bool[,] column;
        readonly bool[,,] block;
        readonly bool[,] isEmpty;
        bool finished = false;
        int outputNum;
        int resNum;

        // sol
        readonly int SOLUTE_NUM = 5;
        int[,] s_board;
        readonly bool[,] s_line;
        readonly bool[,] s_column;
        readonly bool[,,] s_block;
        readonly bool[,] s_isEmpty;
        bool s_finished = false;
        int s_outputNum;

        Dictionary<string, bool> option;
        int endNum;
        string gamePath;
        string solutionPath;
        string endPath;
        int gameNum; // 10 default
        int hard;
        int minEmptyNum;
        int maxEmptyNum;

        public Dictionary<string, bool> Option { get => option; set => option = value; }
        public int EndNum { get => endNum; set => endNum = value; }
        public string GamePath { get => gamePath; set => gamePath = value; }
        public string SolutionPath { get => solutionPath; set => solutionPath = value; }
        public string EndPath { get => endPath; set => endPath = value; }
        public int GameNum { get => gameNum; set => gameNum = value; }
        public int Hard { get => hard; set => hard = value; }
        public int MinEmptyNum { get => minEmptyNum; set => minEmptyNum = value; }
        public int MaxEmptyNum { get => maxEmptyNum; set => maxEmptyNum = value; }
        public Board B { get; }

        public Controller(ref Board b)
        {
            // gen
            board = new int[10, 10];
            line = new bool[10, 10];
            column = new bool[10, 10];
            block = new bool[4, 4, 10];
            isEmpty = new bool[10, 10];
            gameNum = 10; // 10 default
            finished = false;
            outputNum = 1;
            resNum = 1;

            // sol
            s_board = new int[10, 10];
            s_line= new bool[10, 10];
            s_column = new bool[10, 10];
            s_block = new bool[4, 4, 10];
            s_isEmpty = new bool[10, 10];
            s_finished = false;
            s_outputNum = 1;

            option = new Dictionary<string, bool>() { { "-c", false }, { "-s", false }, { "-n", false }, { "-m", false }, { "-r", false }, { "-u", false }, };
            gamePath = "game.txt";
            solutionPath = "sudoku.txt";
            endPath = "final.txt";
            endNum = 0;    // 终局数量
            gameNum = 10; // 游戏数量 10 default
            hard = 1;   // 难度
            minEmptyNum = 20;   // 
            maxEmptyNum = 55;
            B = b;
        }

        public void Parse(string[] args)
        {
            if (((System.Collections.IList)args).Contains("-n") && ((System.Collections.IList)args).Contains("-m") && args.Length == 4)
            {
                option["-n"] = true;
                option["-m"] = true;
                gameNum = int.Parse(args[Array.IndexOf(args, "-n") + 1]);
                hard = int.Parse(args[Array.IndexOf(args, "-m") + 1]);
                if (gameNum <= 0 || gameNum >= 10001) throw new Exception("wrong params!");
                if (hard <= 0 || hard >= 4) throw new Exception("wrong params!");
            }
            else if(((System.Collections.IList)args).Contains("-n") && ((System.Collections.IList)args).Contains("-r") && args.Length == 4)
            {
                option["-n"] = true;
                option["-r"] = true;
                gameNum = int.Parse(args[Array.IndexOf(args, "-n") + 1]);
                string[] num = args[Array.IndexOf(args, "-r") + 1].Split('~');
                minEmptyNum = int.Parse(num[0]);
                maxEmptyNum = int.Parse(num[1]);
                if(gameNum <= 0 || gameNum >= 10001) throw new Exception("wrong params!");
                if (minEmptyNum <= 19 || minEmptyNum >= 56) throw new Exception("wrong params!");
                if (maxEmptyNum <= 19 || maxEmptyNum >= 56) throw new Exception("wrong params!");
            }
            else if (((System.Collections.IList)args).Contains("-n") && ((System.Collections.IList)args).Contains("-u") && args.Length == 3)
            {
                option["-n"] = true;
                option["-u"] = true;
                gameNum = int.Parse(args[Array.IndexOf(args, "-n") + 1]);
                if (gameNum <= 0 || gameNum >= 10001) throw new Exception("wrong params!");
            }
            else if (((System.Collections.IList)args).Contains("-n") && args.Length == 2)
            {
                option["-n"] = true;
                gameNum = int.Parse(args[Array.IndexOf(args, "-n") + 1]);
                if (gameNum <= 0 || gameNum >= 10001) throw new Exception("wrong params!");
            }
            else if (((System.Collections.IList)args).Contains("-s") && args.Length == 2)
            {
                option["-s"] = true;
                gamePath = args[Array.IndexOf(args, "-s") + 1];
            }
            else if (((System.Collections.IList)args).Contains("-c") && args.Length == 2)
            {
                option["-c"] = true;
                endNum = int.Parse(args[Array.IndexOf(args, "-c") + 1]);
                if (endNum <= 0 || endNum >= 1000001) throw new Exception("wrong params!");
            }
            else
            {
                throw new Exception("wrong params!");
            }
        }

        public void Process()
        {
            if (option["-n"])
            {
                StreamWriter sw = new(gamePath, false);
                sw.Close();
                sw = new(endPath, false);
                sw.Close();
                GenerateKGames();
                B.storage = File.ReadAllLines(gamePath);
            }
            else if (option["-c"])
            {
                StreamWriter sw = new(endPath, false);
                sw.Close();
                GenerateKEnds();
                B.storage = File.ReadAllLines(endPath);
            }
            else if (option["-s"])
            {
                StreamWriter sw = new(solutionPath, false);
                sw.Close();
                SoluteKGames();
                B.storage = File.ReadAllLines(solutionPath);
            }
            B.NextBoard();
            B.PrevBoard();
        }

        static void PrintResult(ref int[,] board, string filePath, bool option = true)
        {
            try
            {
                StreamWriter sw = new(filePath, option);
                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        sw.Write(board[i, j]);
                    }
                }
                sw.Write(Environment.NewLine); // 换行
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        void Dfs(ref int[,] board, int x, int y)
        {
            if (finished == true) return;
            if (x < 10 && isEmpty[x, y] == false)
            {
                Dfs(ref board, (9 * (x - 1) + y) / 9 + 1, y % 9 + 1);
            }
            else
            {
                if (x == 10 && y == 1)
                {
                    PrintResult(ref board, endPath);
                    outputNum--;
                    if (outputNum <= 0)
                    {
                        finished = true;
                    }
                }
                else
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        if (line[x, i] == false && column[y, i] == false && block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] == false)
                        {
                            line[x, i] = column[y, i] = block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] = true;
                            board[x, y] = i;
                            //isEmpty[x, y] = false;
                            Dfs(ref board, (9 * x + y - 9) / 9 + 1, y % 9 + 1);
                            if (finished == false)
                            {
                                board[x, y] = 0;
                                //isEmpty[x, y] = true;
                                line[x, i] = column[y, i] = block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] = false;
                            }
                        }
                    }
                }
            }
        }

        public static void Shuffle<T>(Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        void Generate()
        {
            do
            {
                for (int i = 1; i <= 3; i++)    
                {
                    int[] randDigits = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    Random rng = new();
                    Shuffle(rng, randDigits);
                    int pos = 0;
                    for (int j = 1 + ((i - 1) * 3); j <= 3 + ((i - 1) * 3); j++)
                    {
                        for (int k = 1 + ((i - 1) * 3); k <= 3 + ((i - 1) * 3); k++)
                        {
                            board[j, k] = randDigits[pos++];
                            line[j, board[j, k]] = column[k, board[j, k]] = block[(j - 1) / 3 + 1, (k - 1) / 3 + 1, board[j, k]] = true;
                            isEmpty[j, k] = false;
                        }
                    }
                }
                Dfs(ref board, 1, 1);
            } while (board[9, 6] == 0);
        }

        public void GenerateKGames()
        {
            Random ra = new();
            // generate a game board
            for (int n = 0; n < gameNum; n++)
            {
                // init
                finished = false;
                Array.Clear(line, 0, line.Length);
                Array.Clear(column, 0, column.Length);
                Array.Clear(board, 0, board.Length);
                Array.Clear(block, 0, block.Length);
                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        isEmpty[i, j] = true;
                    }
                }

                Generate();

                // only one solution option
                if (option["-u"] == true)
                {
                    // 挖空后求解
                    // 若解不止一种，结束挖空或者换个位置挖空
                    // 若解唯一，继续挖空或者停止
                    if (resNum > 1)
                        Generate();
                }


                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        isEmpty[i, j] = false;
                    }
                }

                // set empty
                int emptyNum = 20;
                if (Option["-r"])
                    emptyNum = ra.Next(minEmptyNum, maxEmptyNum);
                if (Option["-m"])
                    emptyNum = 15 + hard * 10;
                for (int i = 0; i < emptyNum; i++)
                {
                    int x = ra.Next(1, 10);
                    int y = ra.Next(1, 10);
                    if (isEmpty[x, y] == true)
                        continue;
                    else
                    {
                        board[x, y] = 0;
                        isEmpty[x, y] = true;
                    }
                }

                // generate input file
                PrintResult(ref board, gamePath);
            }
        }

        void Solute(ref int[,] board, int x, int y)
        {
            if (s_finished == true) return;
            if (x < 10 && s_isEmpty[x, y] == false)
            {
                Solute(ref board, (9 * (x - 1) + y) / 9 + 1, y % 9 + 1);
            }
            else
            {
                if (x == 10 && y == 1)
                {
                    PrintResult(ref board, solutionPath);
                    s_outputNum--;
                    if (s_outputNum <= 0)
                    {
                        s_finished = true;
                    }
                }
                else
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        if (s_line[x, i] == false && s_column[y, i] == false && s_block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] == false)
                        {
                            board[x, y] = i;
                            s_line[x, i] = s_column[y, i] = s_block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] = true;
                            Solute(ref board, (9 * x + y - 9) / 9 + 1, y % 9 + 1);
                            if (s_finished == false)
                            {
                                board[x, y] = 0;
                                s_line[x, i] = s_column[y, i] = s_block[(x - 1) / 3 + 1, (y - 1) / 3 + 1, i] = false;
                            }
                        }
                    }
                }
            }
        }

        public void SoluteKGames()
        {
            string[] lines = File.ReadAllLines(gamePath);

            foreach (string l in lines)
            {
                s_outputNum = SOLUTE_NUM;
                s_finished = false;

                Array.Clear(s_line, 0, s_line.Length);
                Array.Clear(s_column, 0, s_column.Length);
                Array.Clear(s_block, 0, s_block.Length);

                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        s_board[i, j] = l[(i - 1) * 9 + (j - 1)] - '0';
                    }
                }

                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        if (s_board[i, j] != 0)
                        {
                            s_isEmpty[i, j] = false;
                            s_line[i, s_board[i, j]] = s_column[j, s_board[i, j]] = s_block[(i - 1) / 3 + 1, (j - 1) / 3 + 1, s_board[i, j]] = true;
                        }
                        else
                        {
                            s_isEmpty[i, j] = true;
                        }

                    }
                }

                Solute(ref s_board, 1, 1);
            }
        }

        public void GenerateKEnds()
        {
            for (int n = 0; n < endNum; n++)
            {
                // init
                outputNum = 1;
                finished = false;
                Array.Clear(line, 0, line.Length);
                Array.Clear(column, 0, column.Length);
                Array.Clear(board, 0, board.Length);
                Array.Clear(block, 0, block.Length);
                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        isEmpty[i, j] = true;
                    }
                }

                // generate a game board
                Generate();
            }
        }

    }
}
