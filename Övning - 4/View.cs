namespace GaragePractice
{

    public static class View
    {

        public static void PrintString(string s)
        {
            Console.WriteLine(s);
        }

        public static void PrintVehicles(List<FilterX> displayModelArray)
        {
            if (displayModelArray == null)
            {
                Console.WriteLine("No vehicles found. ");
            }
            Console.WriteLine("Amount found: " + displayModelArray.Count);
            //could format strings into columns

            foreach (FilterX filter in displayModelArray)
            {
                Console.WriteLine(filter.ToString());
            }
        }

        internal static string GetInput()
        {
            return Console.ReadLine();
        }

        internal static void Clear()
        {
            Console.Clear();
        }

        internal static void PrintStats()
        {
            throw new NotImplementedException();
        }
    }
}
