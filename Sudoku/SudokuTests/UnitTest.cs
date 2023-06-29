using Sudoku;

namespace SudokuTests
{
    public class UnitTest1
    {
        [Fact]
        public void BoardConstructTests()
        {
            Board b = new();
        }

        [Fact]
        public void BoardSetCellValTests()
        {
            Board b = new();
            
            Assert.Throws<Exception>(() => { b.SetCellVal(1, 1, 1, 1, 10); });
            b.SetCellVal(1, 1, 1, 1, 3);
            Assert.Equal(3, b.cells[1][1][1][1].value);
        }

        [Fact]
        public void BoardSetValTests()
        {
            Board b = new();

            int[] numArr = new int[81];
            b.SetVal(numArr);

            numArr = new int[]{1,2,3 };
            Assert.Throws<Exception>(() => { b.SetVal(numArr); });
        }

        [Fact]
        public void ControllorConstructTests()
        {
            Board b = new Board();
            Controller c = new Controller(ref b);
            Assert.True(c.B == b);
        }

        [Fact]
        public void ArgumentsTests()
        {
            Board b = new Board();
            Controller c = new Controller(ref b);

            
            string[][] args = { new string[]{ "-n", "0" },
                                new string[]{ "-n", "1000000" },
                                new string[]{ "-c", "0" },
                                new string[]{ "-n", "10", "-m", "0"},
                                new string[]{ "-n", "10", "-m", "4"},
                                new string[]{ "-n", "10", "-r", "0~1"},
                                new string[]{ "-n", "10", "-r", "25~100"},
                                new string[]{ "-n", "10", "-m", "1", "-u"},
                                };

            foreach (string[] str in args)
            {
                Assert.Throws<Exception>(() => { c.Parse(str); });
            }
        }

        [Fact]
        public void GenerateEndsTests()
        {
            Board b = new Board();
            Controller c = new Controller(ref b);

            c.GenerateKEnds();
        }

        [Fact]
        public void GenerateGamesTests()
        {
            Board b = new Board();
            Controller c = new Controller(ref b);

            c.GenerateKGames();
        }

        [Fact]
        public void SoluteKGamesTests()
        {
            Board b = new Board();
            Controller c = new Controller(ref b);

            c.SoluteKGames();
        }

        [Fact]
        public void ProcessTests()
        {
            string[][] args = { new string[]{ "-n", "100" },
                                new string[]{ "-c", "1000" },
                                new string[]{ "-s", "games.txt"},
                                new string[]{ "-n", "10", "-m", "1"},
                                new string[]{ "-n", "10", "-m", "3"},
                                new string[]{ "-n", "10", "-r", "30~50"},
                                new string[]{ "-n", "10", "-u"},
                                };

            foreach (string[] str in args)
            {
                Board b = new Board();
                Controller c = new Controller(ref b);
                c.Parse(str);
                c.Process();
            }

        }

        [Fact]
        public void ViewParseTests()
        {
            Board b = new();
            Controller c = new(ref b);
            View v = new(ref c, ref b);

            ConsoleKey[] ck = new ConsoleKey[] { ConsoleKey.UpArrow,
                                                 ConsoleKey.LeftArrow,
                                                 ConsoleKey.N,
                                                 ConsoleKey.P,
                                                 ConsoleKey.Q,
                                                };

            foreach (ConsoleKey k in ck)
            {
                v.Parse(k);
            }
        }
    }
}