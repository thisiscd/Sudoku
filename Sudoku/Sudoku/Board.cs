using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Board
    {
        public Cell[][][][] cells;
        public string[] storage;
        public int index;

        public Board()
        {
            int[] numArr = new int[81];
            cells = InitBoard();
            SetVal(numArr);
            storage = Array.Empty<string>();
            index = 0;
        }

        private static Cell[][][][] InitBoard()
        {
            Cell[][][][] pArr = new Cell[3][][][];
            for (int m = 0; m < 3; m++)
            {
                pArr[m] = new Cell[3][][];
                for (int n = 0; n < 3; n++)
                {
                    pArr[m][n] = new Cell[3][];
                    for (int i = 0; i < 3; i++)
                    {
                        pArr[m][n][i] = new Cell[3];
                        for (int j = 0; j < 3; j++)
                        {
                            pArr[m][n][i][j] = new Cell() { value = 0, kind = 0 };
                        }
                    }
                }
            }
            return pArr;
        }

        public void SetVal(int[] defVal)
        {
            if (defVal.Length != 81)
            {
                throw new Exception("defVal.Length != 81");
            }
            int c = 0;
            for (int m = 0; m < 3; m++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int val = defVal[c++];
                            cells[m][i][n][j].value = val;
                            if (val > 0)
                                cells[m][i][n][j].kind = 1;
                            else
                                cells[m][i][n][j].kind = 0;
                        }
                    }
                }
            }
        }

        public void SetCellVal(int m, int i, int n, int j, int val)
        {
            if(val <= 0 || val >=10)
            {
                throw new Exception("Val should be in [1, 9]");
            }
            cells[m][i][n][j].value = val;
        }

        public struct Cell
        {
            public int value;
            public int kind;
        }

        public void NextBoard()
        {
            if(index < storage.Length - 1)
            {
                index++;
                int[] numArr = new int[81];
                for (int i = 0; i < storage[index].Length; i++)
                {
                    numArr[i] = storage[index][i] - '0';
                }
                SetVal(numArr);
            }
        }

        public void PrevBoard()
        {
            if (index > 0)
            {
                index--;
                int[] numArr = new int[81];
                for (int i = 0; i < storage[index].Length; i++)
                {
                    numArr[i] = storage[index][i] - '0';
                }
                SetVal(numArr);
            }
        }
    }
}
