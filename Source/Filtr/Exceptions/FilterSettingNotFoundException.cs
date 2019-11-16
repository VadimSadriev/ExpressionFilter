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
        public FilterSettingNotFoundException(string settingName) : base($"Configuration" +
                        $" for property {settingName} does not present in dictionary") { }
    }
}
