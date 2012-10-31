using System;
using System.Data;

namespace AccessProviderSample
{
    /// <summary>
    /// Contains information specific to the database table.
    /// Similar to the DirectoryInfo class.
    /// </summary>
    public class DatabaseTableInfo
    {
        /// <summary>
        /// Row from the "tables" schema
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
        /// The table name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private String name;

        /// <summary>
        /// The number of rows in the table.
        /// </summary>
        public int RowCount
        {
            get
            {
                return rowCount;
            }
            set
            {
                rowCount = value;
            }
        }
        private int rowCount;

        /// <summary>
        /// The column definitions for the table.
        /// </summary>
        public DataColumnCollection Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }
        private DataColumnCollection columns;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="row">The row definition.</param>
        /// <param name="name">The table name.</param>
        /// <param name="rowCount">The number of rows in the table.</param>
        /// <param name="columns">Information on the column tables.</param>
        public DatabaseTableInfo(DataRow row, string name, int rowCount,
                                 DataColumnCollection columns)
        {
            Name = name;
            Data = row;
            RowCount = rowCount;
            Columns = columns;
        } // DatabaseTableInfo
    }
}