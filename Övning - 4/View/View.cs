using Övning___4.ViewModel;

namespace Övning___4.View
{

    public static class View
    {

        public static void PrintString(string s)
        {
            Console.WriteLine(s);
        }
        static private int AskGarageSize()
        {
            View.PrintString($"Garage setup, enter garage size(max 100), or press enter for default size 15");
            string input = View.GetInput();
            if (int.TryParse(input, out int size))
                return Math.Max(Math.Min(size, 100), 15); // clamp 15–100
            return 15; // default

        }
        static private void AskAutoPopulate()
        {
            View.PrintString("\nAutopopulate garage with vehicles? Y/N");
            if (string.Equals(View.GetInput(), "Y", StringComparison.InvariantCultureIgnoreCase))
            {
                
            }
        }
        public static void PrintIEnumerable(IEnumerable<string> s)
        {
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
        }

        public static void PrintVehicles(List<Filter> displayModelArray)
        {
            if (displayModelArray == null)
            {
                Console.WriteLine("No vehicles found. ");
            }
            Console.WriteLine("Amount found: " + displayModelArray.Count);
            //could format strings into columns

            foreach (Filter filter in displayModelArray)
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

        internal static void PrintGarage(IEnumerable<Filter> filteredGarage, string garageName)
        {
            Console.WriteLine("\n Garage: " + garageName + "\n");
            Console.WriteLine(Filter.Header());
            foreach (Filter filter in filteredGarage)
            {
                Console.WriteLine(filter.ToString());
            }
        }

        internal static void PrintVehicle(Filter filter, string v)
        {
            Console.WriteLine(v + filter.ToString());
        }
    }
}
