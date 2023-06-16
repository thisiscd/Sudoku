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
        private int origCol;
        private int origRow;
        private int x;
        private int y;

        public View(Controller c, Board b)
        {
            B = b;
            C = c;
            x = 0;
            y = 0;
            origCol = 0;
            origRow = 0;
        }


        public void showBoard(Board b)
        {
            b.show();
        }

        internal void parse(ConsoleKeyInfo consoleKeyInfo)
        {
            if(consoleKeyInfo.Key == ConsoleKey.UpArrow && y > 0) 
            {
                y--;
                Console.SetCursorPosition(origCol + x, origRow + y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.DownArrow) 
            {
                y++;
                Console.SetCursorPosition(origCol + x, origRow + y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.LeftArrow && x > 0)
            {
                x--;
                Console.SetCursorPosition(origCol + x, origRow + y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.RightArrow) 
            {
                x++;
                Console.SetCursorPosition(origCol + x, origRow + y);
            }
            if(consoleKeyInfo.Key > ConsoleKey.D0 && consoleKeyInfo.Key <= ConsoleKey.D9)
            {
                WriteAt(Char.ToString(consoleKeyInfo.KeyChar), x, y);
            }
            if(consoleKeyInfo.Key == ConsoleKey.H)
            {
                help();
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
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
    }
}
