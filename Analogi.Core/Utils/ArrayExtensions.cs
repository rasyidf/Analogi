/*
 * MIT
 */

using System;
using System.Linq;

namespace Analogi.Core.Utils
{
    internal static class ArrayExtensions
    {
        #region Methods

        internal static T[] WithPadding<T>(this T[] source, int finalLength, T paddingValue = default)
        {
            if (finalLength < source.Length)
            {
                throw new InvalidOperationException("Final length must be greater than or equal to current length.");
            }

            if (finalLength == source.Length)
            {
                return source;
            }

            T[] result = new T[finalLength];
            T[] padding = Enumerable.Repeat(paddingValue, finalLength - source.Length).ToArray();

            Array.Copy(source,
                       sourceIndex: 0,
                       destinationArray: result,
                       destinationIndex: 0,
                       length: source.Length);

            Array.Copy(padding,
                       sourceIndex: 0,
                       destinationArray: result,
                       destinationIndex: source.Length,
                       length: padding.Length);

            return result;
        }

        #endregion Methods
    }
}