namespace Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Board b = new("000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            Controller c = new(ref b);
            View v = new(ref c, ref b);
            c.Parse(args);
            c.Process();
            v.Refresh();
            do
            {
                v.Parse(Console.ReadKey(true));
                v.Refresh();
            } while (true);
        }
    }
}