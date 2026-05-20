namespace Övning___4.View
{
    public static class View
    {
        public static void PrintString(string s)
        {
            Console.WriteLine(s);
        }
        public static void PrintIEnumerable(IEnumerable<string> s)
        {
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
        }
    }
}
