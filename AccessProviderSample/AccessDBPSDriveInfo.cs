using System.Data.Odbc;
using System.Management.Automation;

namespace AccessProviderSample
{
    /// <summary>
    /// Any state associated with the drive should be held here.
    /// In this case, it's the connection to the database.
    /// </summary>
    internal class AccessDBPSDriveInfo : PSDriveInfo
    {
        private OdbcConnection connection;

        /// <summary>
        /// ODBC connection information.
        /// </summary>
        public OdbcConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        /// <summary>
        /// Constructor that takes one argument
        /// </summary>
        /// <param name="driveInfo">Drive provided by this provider</param>
        public AccessDBPSDriveInfo(PSDriveInfo driveInfo)
            : base(driveInfo)
        { }

    }
}