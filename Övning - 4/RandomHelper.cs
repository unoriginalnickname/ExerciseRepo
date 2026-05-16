using System;
using System.Collections.Generic;
using System.Text;

namespace Övning___4
{
    public static class RandomHelper
    {
        private static Random rnd = new();

        public static T Pick<T>(IList<T> list) => list[rnd.Next(list.Count)];
    }
}
