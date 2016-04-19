using System;
using System.Data;
using System.IO;
using System.Web;
using Clutch.Diagnostics.EntityFramework;

namespace LogSQLWithMVC4EF5.Logging
{
    /// <summary>
    /// Implementation of IDbTracingListener
    /// Class is used for tracing all SQL Queries to the entity framework database
    /// </summary>
    public class DbTracingListener : IDbTracingListener
    {
        public void CommandExecuting(DbTracingContext context) {
            //implementation if needed here..
        }

        public void CommandFinished(DbTracingContext context) {
            //implementation if needed here..
        }

        public void ReaderFinished(DbTracingContext context) {
            //implementation if needed here..
        }

        public void CommandFailed(DbTracingContext context) {
            var commandText = ParseParameters(context);

            commandText = "Failed - Timespan: " + context.Duration + Environment.NewLine + Environment.NewLine + commandText;

            var result = context.Result ?? "None";

            LogSql(commandText, result.ToString());
        }

        public void CommandExecuted(DbTracingContext context) {
            var commandText = ParseParameters(context);

            commandText = "Command - Timespan: " + context.Duration + Environment.NewLine + Environment.NewLine + commandText;

            var result = context.Result ?? "None";

            LogSql(commandText, result.ToString());
        }

        private string ParseParameters(DbTracingContext context) {
            var commandText = context.Command.CommandText;

            if (context.Command.Parameters.Count > 0) {

                for (var index = 0; index < context.Command.Parameters.Count; index++) {
                    var parameter = context.Command.Parameters[index];
                    var parameterName = parameter.ParameterName.StartsWith("@") ? parameter.ParameterName : "@" + parameter.ParameterName;
                    var parameterValue = String.Empty;

                    switch (parameter.DbType) {
                        case DbType.Boolean:
                            parameterValue = parameter.Value.ToString() != "0" ? "true" : "false";
                            break;
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.Date:
                        case DbType.DateTime2:
                        case DbType.DateTimeOffset:
                        case DbType.Guid:
                        case DbType.Object:
                        case DbType.DateTime:
                        case DbType.String:
                        case DbType.StringFixedLength:
                        case DbType.Time:
                        case DbType.Xml:
                            parameterValue = "'" + parameter.Value + "'";
                            break;
                        default:
                            parameterValue = parameter.Value.ToString();
                            break;
                    }
                    commandText = commandText.Replace(parameterName, parameterValue);
                }

            }

            return commandText;
        }

        public void LogSql(string commandText, string result) {
            if (HttpContext.Current == null) {
                return;
            }

            var user = HttpContext.Current.User.Identity.Name;

            if (user == String.Empty) {
                return;
            }

            var fileDirectory = HttpRuntime.AppDomainAppPath + "/Logs/"+ user.Trim();

            // Create Logs Directory If It Does Not Exist
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using (var fileWriter = File.AppendText(fileDirectory + "/debuglog.txt")) {
                fileWriter.WriteLine(commandText + Environment.NewLine + Environment.NewLine + "With Result: " + result + Environment.NewLine);
            }

        }

    }
}