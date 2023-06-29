namespace Sudoku
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Board b = new();
            Controller c = new(ref b);
            View v = new(ref c, ref b);
            c.Parse(args);
            c.Process();
            Run(ref v);
        }

        public static void Run(ref View v)
        {
            v.Init();
            v.Refresh();
            do
            {
                v.Parse(Console.ReadKey(true).Key);
                v.Refresh();
            } while (v.run);
            Console.SetCursorPosition(1, 15);
        }
    }
}