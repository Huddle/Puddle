using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Odbc;
using System.Management.Automation.Provider;

namespace AccessProviderSample
{
    /// <summary>
    /// Content writer used to write data in this provider.
    /// </summary>
    public class AccessDBContentWriter : IContentWriter
    {
        // A provider instance is required so as to get "content"
        private AccessDBProvider provider;
        private string path;
        private long currentOffset;

        internal AccessDBContentWriter(string path, AccessDBProvider provider)
        {
            this.path = path;
            this.provider = provider;
        }

        /// <summary>
        /// Write the specified row contents in the source
        /// </summary>
        /// <param name="content"> The contents to be written to the source.
        /// </param>
        /// <returns>An array of elements which were successfully written to 
        /// the source</returns>
        /// 
        public IList Write(IList content)
        {
            if (content == null)
            {
                return null;
            }

            // Get the total number of rows currently available it will 
            // determine how much to overwrite and how much to append at
            // the end
            string tableName;
            int rowNumber;
            PathType type = provider.GetNamesFromPath(path, out tableName, out rowNumber);

            if (type == PathType.Table)
            {
                OdbcDataAdapter da = provider.GetAdapterForTable(tableName);
                if (da == null)
                {
                    return null;
                }

                DataSet ds = provider.GetDataSetForTable(da, tableName);
                DataTable table = provider.GetDataTable(ds, tableName);

                string[] colValues = (content[0] as string).Split(',');

                // set the specified row
                DataRow row = table.NewRow();

                for (int i = 0; i < colValues.Length; i++)
                {
                    if (!String.IsNullOrEmpty(colValues[i]))
                    {
                        row[i] = colValues[i];
                    }
                }

                //table.Rows.InsertAt(row, rowNumber);
                // Update the table
                table.Rows.Add(row);
                da.Update(ds, tableName);

            }
            else
            {
                throw new InvalidOperationException("Operation not supported. Content can be added only for tables");
            }

            return null;
        } // Write

        /// <summary>
        /// Moves the content reader specified number of rows from the 
        /// origin
        /// </summary>
        /// <param name="offset">Number of rows to offset</param>
        /// <param name="origin">Starting row from which to offset</param>
        public void Seek(long offset, System.IO.SeekOrigin origin)
        {
            // get the number of rows in the table which will help in
            // calculating current position
            string tableName;
            int rowNumber;

            PathType type = provider.GetNamesFromPath(path, out tableName, out rowNumber);

            if (type == PathType.Invalid)
            {
                throw new ArgumentException("Path specified should represent either a table or a row : " + path);
            }

            Collection<DatabaseRowInfo> rows =
                provider.GetRows(tableName);

            int numRows = rows.Count;

            if (offset > rows.Count)
            {
                throw new
                    ArgumentException(
                    "Offset cannot be greater than the number of rows available"
                    );
            }

            if (origin == System.IO.SeekOrigin.Begin)
            {
                // starting from Beginning with an index 0, the current offset
                // has to be advanced to offset - 1
                currentOffset = offset - 1;
            }
            else if (origin == System.IO.SeekOrigin.End)
            {
                // starting from the end which is numRows - 1, the current
                // offset is so much less than numRows - 1
                currentOffset = numRows - 1 - offset;
            }
            else
            {
                // calculate from the previous value of current offset
                // advancing forward always
                currentOffset += offset;
            }

        } // Seek

        /// <summary>
        /// Closes the content reader, so all members are reset
        /// </summary>
        public void Close()
        {
            Dispose();
        } // Close

        /// <summary>
        /// Dispose any resources being used
        /// </summary>
        public void Dispose()
        {
            Seek(0, System.IO.SeekOrigin.Begin);

            GC.SuppressFinalize(this);
        } // Dispose
    }
}