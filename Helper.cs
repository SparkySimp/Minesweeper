using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTP_20230119_Minesweeper
{
    /// <summary>
    /// Contains helper utilities
    /// </summary>
    public class Helper
    {
        public static void DumpArray(object[,] array, int? nRows = null, int? nCols = null, string format = "{0}")
        {
            for (int i = 0; i < nRows.GetValueOrDefault(array.GetLength(0)); i++)
            {
                for (int j = 0; j < nCols.GetValueOrDefault(array.GetLength(1)); j++)
                {
                    Debug.Write(String.Format(format, array[i, j]));
                    Debug.Write(" ");
                }
                Debug.Write("\n\r");
            }
        }

        public static void DumpArray(object[] array, string format = "{0}")
        {
            foreach (var item in array)
            {
                Debug.Write(String.Format(format, item));
                Debug.Write(" ");
            }
            Debug.Write("\n\r");
        }
    }

    /// <summary>
    /// Contains methods for passing output from a function to another one idiomatically.
    /// </summary>
    public static class Piper
    {
        /// <summary>
        /// Feeds one function's input with another function's output.
        /// </summary>
        /// <typeparam name="TIn">Type for this value, which will be piped down.</typeparam>
        /// <typeparam name="TOut">Type for the operation result.</typeparam>
        /// <param name="self">The object to pipe down.</param>
        /// <param name="pipe">The function which will process this object.</param>
        /// <returns>The result of the applied function.</returns>
        public static TOut Pipe<TIn, TOut>(this TIn self, Func<TIn, TOut> pipe) => pipe(self);
    }
}
