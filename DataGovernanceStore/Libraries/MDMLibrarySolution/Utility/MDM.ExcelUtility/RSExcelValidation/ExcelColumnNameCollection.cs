using System;
using System.Collections.Generic;

namespace MDM.ExcelUtility.RSExcelValidation
{
    internal class ExcelColumnNameCollection
    {
        private const int DefaultColumnsCount = 702;
        private static ExcelColumnNameCollection _instance;

        private readonly Dictionary<int, string> _indexNameMappings;

        private ExcelColumnNameCollection()
        {
            _indexNameMappings = new Dictionary<int, string>();
            DefineColumnsCount(DefaultColumnsCount);
        }

        /// <summary>
        /// Singleton class instance
        /// </summary>
        public static ExcelColumnNameCollection Instance
        {
            get
            {
                return _instance ?? (_instance = new ExcelColumnNameCollection());
            }
        }

        /// <summary>
        /// Returns current column name
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The string column name <see cref="string"/>. </returns>
        public String GetColumnName(int index)
        {
            return _indexNameMappings[index];
        }

        /// <summary>
        /// Fills the name collection
        /// </summary>
        /// <param name="columnsCount">The columns Count.</param>
        public void DefineColumnsCount(int columnsCount)
        {
            if (columnsCount < _indexNameMappings.Count)
            {
                return;
            }

            for (int i = _indexNameMappings.Count; i <= columnsCount; i++)
            {
                _indexNameMappings.Add(i, GetColumnNameByNumber(i));
            }
        }

        private static string GetColumnNameByNumber(int number)
        {
            String[] letters = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            Int32 quotient = number / 26;
            if (quotient > 0)
            {
                return GetColumnNameByNumber(quotient - 1) + letters[number % 26];
            }

            return letters[number % 26];
        }
    }
}
