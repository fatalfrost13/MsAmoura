using System.Linq;

namespace Iomer.Extensions.Array
{
    public static class ArrayUtility
    {

        /// <summary>
        /// Checks to see if a value exists in a csv
        /// </summary>
        /// <param name="value"></param>
        /// <param name="csv"></param>
        /// <returns></returns>
        public static bool ValueExistsInCsv(this string value, string csv)
        {
            var exists = false;
            if (!string.IsNullOrEmpty(csv) && !string.IsNullOrEmpty(value))
            {
                var array = csv.Split(',').ToArray();
                foreach (var item in array.Where(item => item == value))
                {
                    exists = true;
                }
            }
            return exists;
        }

        /// <summary>
        /// Checks to see if a value in csv1 exists in csv2
        /// </summary>
        /// <param name="csv1"></param>
        /// <param name="csv2"></param>
        /// <returns></returns>
        public static bool CsvValueMatch(this string csv1, string csv2)
        {
            var exists = false;
            if (!string.IsNullOrEmpty(csv1) && !string.IsNullOrEmpty(csv2))
            {
                var array = csv1.Split(',').ToArray();
                var array2 = csv2.Split(',').ToArray();

                foreach (var item1 in from item1 in array from item2 in array2.Where(item2 => item1 == item2 && item2 != "") select item1)
                {
                    exists = true;
                }
            }
            return exists;
        }
    }
}
