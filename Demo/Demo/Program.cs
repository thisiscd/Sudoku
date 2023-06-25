namespace Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Board b = new Board("123456789987654321123456789987654321000000000987654321000000000987654321000000000");
            Controller c = new Controller();
            View v = new View(ref c, ref b);
            do
            {
                v.parse(Console.ReadKey(true));
            }while (true);
        }
    }
}