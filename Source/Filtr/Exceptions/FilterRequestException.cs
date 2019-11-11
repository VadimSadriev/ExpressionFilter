namespace Filtr.Exceptions
{
    /// <summary> Exception thrown when filter request is invalid </summary>
    public class FilterRequestException : BaseFilterException
    {
        /// <summary> Exception thrown when filter request is invalid </summary>
        public FilterRequestException(string message) : base(message) { }
    }
}
