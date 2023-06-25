using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class Board
    {
        public cell[][][][] cells;

        public Board(string numStr)
        {
            string s = numStr.Replace(",", "");
            if (s.Length != 81)
            {
                throw new Exception("初始数独错误，必须是81个数字");
            }
            int[] numArr = new int[81];
            for (int i = 0; i < s.Length; i++)
            {
                numArr[i] = Convert.ToInt32(s[i].ToString());
            }
            cells = initBoard();
            setVal(numArr);
        }


        private cell[][][][] initBoard()
        {
            cell[][][][] pArr = new cell[3][][][];
            for (int m = 0; m < 3; m++)
            {
                pArr[m] = new cell[3][][];
                for (int n = 0; n < 3; n++)
                {
                    pArr[m][n] = new cell[3][];
                    for (int i = 0; i < 3; i++)
                    {
                        pArr[m][n][i] = new cell[3];
                        for (int j = 0; j < 3; j++)
                        {
                            pArr[m][n][i][j] = new cell() { value = 0, kind = 0 };
                        }
                    }
                }
            }
            return pArr;
        }

        public void setVal(int[] defVal)
        {
            int c = 0;
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int val = defVal[c++];
                            cells[m][n][i][j].value = val;
                            if (val > 0)
                                cells[m][n][i][j].kind = 1;
                        }
                    }
                }
            }
        }

        public void setCell(int m, int i, int n, int j, int val)
        {
            cells[m][i][n][j].value = val;
        }

        public struct cell
        {
            public int value;
            public int kind;
        }
    }
}
