namespace Filtr.Exceptions
{
    /// <summary>
    /// Exception thrown when setting does not present in filtrator dictionary
    /// </summary>
    public class FilterSettingNotFoundException : BaseFilterException
    {
        /// <summary>
        /// Exception thrown when setting does not present in filtrator dictionary
        /// </summary>
        public FilterSettingNotFoundException(string message) : base(message) { }
    }
}
