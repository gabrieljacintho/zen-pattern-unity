using System.Collections.Generic;

namespace FireRingStudio.Helpers
{
    public static class ListHelper
    {
        public static List<T> Merge<T>(List<T> list1, List<T> list2)
        {
            List<T> result = new List<T>(list1.Count + list2.Count);
            result.AddRange(list1);
            result.AddRange(list2);

            return result;
        }

        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}