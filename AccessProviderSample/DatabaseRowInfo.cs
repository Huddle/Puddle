using System.Data;

namespace AccessProviderSample
{
    /// <summary>
    /// Contains information specific to an individual table row.
    /// Analogous to the FileInfo class.
    /// </summary>
    public class DatabaseRowInfo
    {
        /// <summary>
        /// Row data information.
        /// </summary>
        public DataRow Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        private DataRow data;

        /// <summary>
        /// The row index.
        /// </summary>
        public string RowNumber
        {
            get
            {
                return rowNumber;
            }
            set
            {
                rowNumber = value;
            }
        }
        private string rowNumber;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">The row information.</param>
        /// <param name="name">The row index.</param>
        public DatabaseRowInfo(DataRow row, string name)
        {
            RowNumber = name;
            Data = row;
        } // DatabaseRowInfo
    }
}