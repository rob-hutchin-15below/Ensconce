using System.Data.SqlClient;

namespace Ensconce.Console
{
    internal static class DatabaseInteraction
    {
        internal static void DoDeployment()
        {
            SqlConnectionStringBuilder connStr = null;

            if (!string.IsNullOrEmpty(Arguments.ConnectionString))
            {
                connStr = new SqlConnectionStringBuilder(Arguments.ConnectionString.Render());
            }
            else if (!string.IsNullOrEmpty(Arguments.DatabaseName))
            {
                connStr = Database.GetLocalConnectionStringFromDatabaseName(Arguments.DatabaseName.Render());
            }

            Logging.Log("Deploying scripts from {0} using connection string {1}", Arguments.DeployFrom, connStr.ConnectionString);

            var database = new Database(connStr, new LegacyFolderStructure(), Arguments.WarnOnOneTimeScriptChanges)
            {
                WithTransaction = Arguments.WithTransaction,
                OutputPath = Arguments.RoundhouseOutputPath
            };
            
            database.Deploy(Arguments.DeployFrom, Arguments.DatabaseRepository.Render(), Arguments.DropDatabase, Arguments.DatabaseCommandTimeout);
        }
    }
}