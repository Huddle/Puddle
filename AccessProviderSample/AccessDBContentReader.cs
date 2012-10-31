using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Management.Automation.Provider;

namespace AccessProviderSample
{
    /// <summary>
    /// Content reader used to retrieve data from this provider.
    /// </summary>
    public class AccessDBContentReader : IContentReader
    {
        // A provider instance is required so as to get "content"
        private AccessDBProvider provider;
        private string path;
        private long currentOffset;

        internal AccessDBContentReader(string path, AccessDBProvider provider)
        {
            this.path = path;
            this.provider = provider;
        }

        /// <summary>
        /// Read the specified number of rows from the source.
        /// </summary>
        /// <param name="readCount">The number of items to 
        /// return.</param>
        /// <returns>An array of elements read.</returns>
        public IList Read(long readCount)
        {
            // Read the number of rows specified by readCount and increment
            // offset
            string tableName;
            int rowNumber;
            PathType type = provider.GetNamesFromPath(path, out tableName, out rowNumber);

            Collection<DatabaseRowInfo> rows =
                provider.GetRows(tableName);
            Collection<DataRow> results = new Collection<DataRow>();

            if (currentOffset < 0 || currentOffset >= rows.Count)
            {
                return null;
            }

            int rowsRead = 0;

            while (rowsRead < readCount && currentOffset < rows.Count)
            {
                results.Add(rows[(int)currentOffset].Data);
                rowsRead++;
                currentOffset++;
            }

            return results;
        } // Read

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
                throw new ArgumentException("Path specified must represent a table or a row :" + path);
            }

            if (type == PathType.Table)
            {
                Collection<DatabaseRowInfo> rows = provider.GetRows(tableName);

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
            } // if (type...
            else
            {
                // for row, the offset will always be set to 0
                currentOffset = 0;
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