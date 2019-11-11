using Filtr.Enums;
using System;

namespace Filtr.Models
{
    /// <summary>
    /// Setting for filtering object
    /// </summary>
    public class FilterSetting
    {
        /// <summary>
        /// Type of filtred object
        /// </summary>
        public Type OperandType { get; set; }

        /// <summary>
        /// Type of property by which the object is filtered
        /// </summary>
        public Type ParameterType { get; set; }

        /// <summary>
        /// Full name of input property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of operator between conditions
        /// </summary>
        public OperatorBetween OperatorBetween { get; set; }

        /// <summary>
        /// Method to call for property
        /// </summary>
        public Method MethodName { get; set; }

        /// <summary>
        /// Path to the property of filtered object
        /// </summary>
        public string[] PropertyPath { get; set; }

        /// <summary>
        /// Flag if filter contains single value
        /// </summary>
        public bool IsSingleFilter => OperatorBetween == OperatorBetween.None;

        /// <summary>
        /// Type of sorting
        /// </summary>
        public SortingDirection SortingDirection { get; set; } = SortingDirection.Asc;
    }
}
