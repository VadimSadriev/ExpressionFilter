using System;

namespace Filtr.Exceptions
{
    /// <summary> Base exception for all filter exceptions.
    /// thrown when something wrong happened during filtering
    /// </summary>
    public class BaseFilterException : Exception
    {
        /// <summary> Base exception for all filter exceptions.
        /// thrown when something wrong happened during filtering
        /// </summary>
        public BaseFilterException(string message) : base(message) { }
    }
}