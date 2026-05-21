namespace Övning___4.Misc
{
    public static class RandomHelper
    {
        private static Random rnd = new();

        public static T Pick<T>(IEnumerable<T> list) => list.ElementAt(rnd.Next(list.Count()));

    }
}
