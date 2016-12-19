using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Xml;

namespace DbBuilder
{
    class Program
    {
        static string BatchFileName = "FullScript.bat";
        static string BasePath = ConfigurationManager.AppSettings["BasePath"];
        static string LogFileName = "Errors.log";
        static string TempLogFileName = "ErrorLog.log";
        static string ExecuteMigrationScript = ConfigurationManager.AppSettings["ExecuteMigrationScript"];
        static string DBConfiguration = ConfigurationManager.AppSettings["DBConfiguration"];
		static string AuthenticationType = ConfigurationManager.AppSettings["AuthType"];
        static string[] CustomUserDetails = null;

        #region DisableCloseOptions
        // Declaring references to external procedures that are in user32.dll.
        [DllImport("user32.dll", EntryPoint = "DeleteMenu", CharSet = CharSet.Ansi)]
        public static extern bool DeleteMenu(Int32 hMenu, Int32 uPosition, Int32 uFlags);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = CharSet.Ansi)]
        public static extern Int32 GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu", CharSet = CharSet.Ansi)]
        public static extern Int32 GetSystemMenu(Int32 hWnd, bool bRevert);

        [DllImport("user32.dll", EntryPoint = "GetWindow", CharSet = CharSet.Ansi)]
        public static extern Int32 GetWindow(Int32 hWnd, Int32 uCmd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Ansi)]
        public static extern Int32 GetWindowText(Int32 hWnd, [OutAttribute()] StringBuilder strNewWindowName, Int32 maxCharCount);

        private static int ObtainWindowHandle(string lpstrCaption)
        {
            // To store the handle to a window.
            int hWnd = 0;
            // Maximum number of characters in the GetWindowText method.
            int nMaxCount = 0;
            // Actual number of characters copied in the GetWindowText method.
            int nCopiedLength = 0;
            // To store the text of the title bar of the window.
            StringBuilder lpString;

            nMaxCount = 255;
            // Obtain a handle to the first window.
            hWnd = GetForegroundWindow();

            // Loop through the various windows until you encounter the console application window, _
            // or there are no more windows.
            while (hWnd != 0)
            {

                // Fill lpString with spaces.
                lpString = new StringBuilder(nMaxCount);
                // Get the text of the window title bar in lpString.
                nCopiedLength = GetWindowText(hWnd, lpString, nMaxCount);

                // Verify that lpString is neither empty, nor NULL.
                if (lpString.ToString().Trim().Length != 0)
                {
                    // Verify that the title of the retrieved window is the same as the title of the console application window.
                    if (lpString.ToString().Contains(lpstrCaption))
                    {
                        // Return hWnd to the Main method.
                        return hWnd;
                    }
                }
                // Get the next window.
                hWnd = GetWindow(hWnd, 2);
            }
            // If no corresponding windows are found, return 0.
            return 0;
        }
        #endregion

        static int Main(string[] args)
        {
            try
            {
                #region DisableCloseOptions
                // Obtain a handle to the console application window by passing the title of your application.
                int hWnd = ObtainWindowHandle("DbBuilder");
                // Obtain a handle to the console application system menu.
                int hMenu = GetSystemMenu(hWnd, false);
                // Delete the Close menu item from the console application system menu.
                // This will automatically disable the Close button on the console application title bar.
                // 6 indicates the position of the Close menu item.
                // 1024 indicates that the second parameter is a positional indicator.
                DeleteMenu(hMenu, 6, 1024);
                #endregion
                
                string[] BuildItemsOnce = ConfigurationManager.AppSettings["BuildItemsOnce"].Split(',');
                //string[] BuildItemsTwice = ConfigurationManager.AppSettings["BuildItemsTwice"].Split(',');
                bool GenerateOnly = (ConfigurationManager.AppSettings["GenerateOnly"] == "True");
                bool LogErrors = (ConfigurationManager.AppSettings["CreateErrorLog"] == "True");
                //make it false when Basepath is provided as command line argument
                bool overrideBasePath = true;
                
                string[] ConnStrs = null;
                if (args.Length < 1)
                {
                    ConnStrs = ConfigurationManager.AppSettings["ConnectionString"].Split(';');
                }
                else if (args.Length == 1)
                {
                    ConnStrs = args[0].Split(';');
                }
                else if (args.Length == 7) //following configuration entry for calling db builder from Installer
                {
                    ConnStrs = args[0].Split(';');
                    BasePath = args[1];
                    DBConfiguration = args[2];
                    LogErrors = (args[3] == "True");
                    ExecuteMigrationScript = args[4];
                    AuthenticationType = args[5];
                    LogFileName = DBConfiguration + "_Errors.log";
                    BatchFileName = DBConfiguration + "_FullScript.bat";
                    CustomUserDetails = args[6].Split(';');

                    if (!string.IsNullOrEmpty(BasePath))
                    {
                        overrideBasePath = false;
                    }
                }
                if (!string.IsNullOrEmpty(DBConfiguration) && DBConfiguration != "PC_Dev")
                {
                    if (overrideBasePath)
                    {
                        BasePath = ConfigurationManager.AppSettings["BasePath" + "_" + DBConfiguration];
                    }

                    BuildItemsOnce = ConfigurationManager.AppSettings["BuildItemsOnce" + "_" + DBConfiguration].Split(',');
                }

                if (DBConfiguration == "CreateDB")
                {
                    DBConfiguration = "PC_Dev";
                }

                using (StreamWriter SwFullScript = new StreamWriter(BatchFileName, false))
                {
                    if (CustomUserDetails[0] != "Service User" || AuthenticationType == "SQL Server Authentication")
                    {
                        SwFullScript.WriteLine("@echo off");
                        SwFullScript.WriteLine();
                        SwFullScript.WriteLine(@"SET ErrFile={0}", TempLogFileName);
                        SwFullScript.WriteLine();
                        SwFullScript.WriteLine("SET LogFile={0}", LogFileName);
                        SwFullScript.WriteLine();
                    }

                    // not using SqlCommand because will have to check 
                    // for BATCH separator in the file which requires parsing the file
                    //Storing log for each object in a temporary file
                    string StrOsqlErr = string.Empty;
                    string StrOsqlClean = string.Empty;

                    if (AuthenticationType == "SQL Server Authentication")
                    {
                        StrOsqlErr = string.Format(@"SQLCMD -o " + TempLogFileName + " -m11 -f 65001 -S {0} -d \"{1}\" -U \"{2}\" -P \"{3}\" -i ",
                            ConnStrs[0],
                            ConnStrs[1],
                            ConnStrs[2],
                            ConnStrs[3]);
                    }
                    else //Windows Authentication
                    {
                        StrOsqlErr = string.Format(@"SQLCMD -o " + TempLogFileName + " -m11 -f 65001 -S {0} -d \"{1}\" -E -i ",
                            ConnStrs[0],
                            ConnStrs[1]);
                    }

                    if (BuildItemsOnce.Length > 0)
                    {
                        parseItems(BuildItemsOnce, SwFullScript, StrOsqlErr, true);
                    }
                }

                if (GenerateOnly)
                    return 0;
                
                if (CustomUserDetails[0] == "Service User" && AuthenticationType == "Windows Authentication")
                {
                    string[] fileLines = File.ReadAllLines(BatchFileName);
                    using (new Impersonator(CustomUserDetails[1], CustomUserDetails[2], CustomUserDetails[3]))
                    {
                        foreach (string line in fileLines)
                        {
                            Process Proc = new Process();
                            Proc.StartInfo.FileName = "SQLCMD.EXE";
                            Proc.StartInfo.Arguments = line;
                            Proc.StartInfo.UseShellExecute = false;
                            //Proc.StartInfo.RedirectStandardOutput = true;
                            Proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            Proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

                            string sqlFileName = line.Substring(line.LastIndexOf("\\") + 1);
                            sqlFileName = sqlFileName.Substring(0, sqlFileName.Length - 1);
                            Console.WriteLine("Executing Script : " + sqlFileName);

                            Proc.Start();

                            Proc.WaitForExit();

                            if (File.Exists(Environment.CurrentDirectory + @"\ErrorLog.log"))
                            {
                                //read content of file and append to original log file.
                                string log = File.ReadAllText(Environment.CurrentDirectory + @"\ErrorLog.log");
                                if (!string.IsNullOrWhiteSpace(log))
                                {
                                    WriteToFile("Error occurred in " + sqlFileName, Environment.CurrentDirectory + @"\" + LogFileName);
                                    WriteToFile(log, Environment.CurrentDirectory + @"\" + LogFileName);
                                    WriteToFile("", Environment.CurrentDirectory + @"\" + LogFileName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ProcessStartInfo PsiBatchFile = new ProcessStartInfo(BatchFileName);
                    PsiBatchFile.UseShellExecute = false;

                    Process ProcBatchFile = Process.Start(PsiBatchFile);
                    ProcBatchFile.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.EventLog.WriteEntry("Riversand MDMCenter Installation", "Error executing SQL objects\n" + exp.Message);
            }
            
            return 1;
        }

        static void parseItems(string[] BuildItems, StreamWriter Sw, string StrOsql, bool logerror)
        {
            try
            {
                int cnt = 0;
                string orgBasePath = BasePath;

                foreach (string BuildItem in BuildItems)
                {
                    String filepath = string.Empty;

                    if (BuildItem.StartsWith("CoreSQLObjects"))
                    {
                        filepath = BasePath + BuildItem.Replace("CoreSQLObjects", @"CoreSQLObjects\dbo");
                    }
                    else
                    {
                        filepath = BasePath + BuildItem;
                    }

                    if ((BuildItem.StartsWith("Scripts") && BuildItem.EndsWith(".sql")) ||
                        (BuildItem.StartsWith("dbo\\Scripts") && BuildItem.EndsWith(".sql")) ||
                        (BuildItem.EndsWith(".CLR.sql")) || (BuildItem.EndsWith("FTCatalog.sql")) || (BuildItem.EndsWith("FTCatalog_Category.sql")) ||
                        (BuildItem.StartsWith("Replication") && (BuildItem.EndsWith(".sql"))))
                    {
                        string filename = BuildItem;
                        if (DBConfiguration.ToLower() == "workflow")
                        {
                            filepath = orgBasePath + @"Schema Objects\Schemas\dbo\" + BuildItem;
                        }

                        if (DBConfiguration.ToLower() == "vendorportal" || DBConfiguration.ToLower() == "vendorportal_subupdate")
                        {
                            filepath = BasePath + "dbo\\";
                        }
                        //else if ((DBConfiguration.ToLower() == "pc_dev" || DBConfiguration.ToLower() == "subscriber" || DBConfiguration.ToLower() == "subscriberonlytrigger") && !BuildItem.EndsWith(".CLR.sql"))
                        else if ((DBConfiguration.ToLower() == "pc_dev" || DBConfiguration.ToLower() == "subscriber" || DBConfiguration.ToLower() == "pc_dev_subupdate") && !BuildItem.EndsWith(".CLR.sql"))
                        {
                            filepath = BasePath + @"Schemas\dbo\";
                        }
                        else if (DBConfiguration.ToLower() == "replication")
                        {
                            filepath = BasePath + BuildItem;
                        }
                        else if ((DBConfiguration.ToLower() == "stagingdb" || DBConfiguration.ToLower() == "oneworldsync" || DBConfiguration.ToLower() == "dqmdb") && (BuildItem.EndsWith("ChangeScript_DDL.sql") || BuildItem.EndsWith("ChangeScript_DML.sql")))
                        {
                            filepath = BasePath;
                        }

                        if (BuildItem.StartsWith("Scripts\\Migration_"))
                        {
                            if (ExecuteMigrationScript == "True")
                            {
                                filepath = filepath + BuildItem;
                            }
                            else
                            {
                                filepath = filepath + BuildItem.Replace("Migration_", "Exclude_");
                            }
                        }

                        if (BuildItem.StartsWith("Scripts\\ChangeScript_"))
                        {
                            filepath = filepath + BuildItem;
                        }
                        else if (BuildItem.EndsWith("_SubscriberChangeScript_DDL.sql"))
                        {
                            filepath = filepath + "Scripts\\" + BuildItem;
                        }
                        else if ((DBConfiguration.ToLower() == "pc_dev" || DBConfiguration.ToLower() == "subscriber" || DBConfiguration.ToLower() == "pc_dev_subupdate") &&
                                (BuildItem == "Scripts\\ApplicationConfigurationDataScript.sql" || BuildItem == "Scripts\\AppConfig.sql" || BuildItem == "Scripts\\CoreBusinessRules.sql"))
                        {
                            filename = BuildItem;
                            filepath = filepath + BuildItem;
                        }

                        if (!filepath.EndsWith(".sql"))
                        {
                            filepath = filepath + filename.Replace("dbo\\", "");
                        }

                        if (filename.StartsWith("dbo\\Scripts"))
                        {
                            filename = filename.Replace("dbo\\Scripts", "Scripts");
                        }
                        else if (BuildItem.EndsWith("FTCatalog.sql") || BuildItem.EndsWith("FTCatalog_Category.sql"))
                        {
                            filename = BuildItem;
                            filepath = BasePath + @"Database Level Objects\Storage\Full Text Catalogs\" + BuildItem;
                        }

                        if (File.Exists(filepath))
                        {
                            if (CustomUserDetails[0] == "Service User" && AuthenticationType == "Windows Authentication")
                            {
                                string command = StrOsql + "\"" + filepath + "\"";
                                command = command.Replace("SQLCMD", "");

                                Sw.WriteLine(command);
                            }
                            else
                            {
                                Sw.WriteLine("echo Executing : " + filename);
                                Sw.WriteLine(StrOsql + "\"" + filepath + "\"");

                                Sw.WriteLine("SET SQLFile=" + filename);
                                Sw.WriteLine("CALL LogError.bat %ErrFile% %SQLFile% %LogFile%");
                                Sw.WriteLine();
                            }
                        }
                        else
                        {
                            //StreamWriter ErrWr = new StreamWriter(LogFileName, true);
                            //ErrWr.WriteLine(string.Format("File not found : {0}", BuildItem));
                            //ErrWr.WriteLine();
                            //ErrWr.Close();
                        }
                    }
                    else
                    {
                        if (DBConfiguration.ToLower() == "workflow" && BuildItem == "Tables")
                        {
                            string WFPath = @"Schema Objects\Schemas\System.Activities.DurableInstancing\";
                            if (cnt == 0)
                            {
                                BasePath = BasePath + WFPath;
                            }

                            if (BuildItem == "Tables" && cnt == 1)
                            {
                                WFPath = @"Schema Objects\Schemas\dbo\";
                                BasePath = orgBasePath + WFPath;
                            }
                            cnt = 1;

                            filepath = BasePath + BuildItem;
                        }

                        string BuildConfig = BuildItem;

                        if ((DBConfiguration.ToLower() == "vendorportal" || DBConfiguration.ToLower() == "vendorportal_subupdate") && BuildConfig.StartsWith("dbo"))
                        {
                            BuildConfig = BuildConfig.Replace("dbo\\", "");
                        }

                        string BuildObject = string.Empty;
                        string XMLFile = BasePath + BuildItem;

                        if ((DBConfiguration.ToLower() == "vendorportal" || DBConfiguration.ToLower() == "vendorportal_subupdate") && BuildConfig.StartsWith("CoreSQLObjects"))
                        {
                            XMLFile = filepath;
                        }
                        else if (DBConfiguration.ToLower() == "pc_dev" || DBConfiguration.ToLower() == "subscriber" || DBConfiguration.ToLower() == "pc_dev_subupdate")
                        {
                            if (BuildItem.StartsWith("Table") || BuildItem.StartsWith("Views"))
                            {
                                XMLFile = BasePath + @"Schemas\dbo\" + BuildItem;
                            }
                            else
                            {
                                XMLFile = BasePath + @"Schemas\dbo\Programmability\" + BuildItem;
                            }

                            filepath = XMLFile;
                        }

                        switch (BuildConfig)
                        {
                            case "Tables":
                                XMLFile = XMLFile + "\\Tables.xml";
                                BuildObject = "Creating/Updating Table : ";
                                break;

                            case "Tables_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Tables.xml";
                                BuildObject = "Creating/Updating Table : ";
                                break;

                            case "Tables\\Constraints":
                                XMLFile = XMLFile + "\\Constraints.xml";
                                BuildObject = "Creating/Updating Constraints : ";
                                break;

                            case "Tables\\Indexes":
                                XMLFile = XMLFile + "\\Indexes.xml";
                                BuildObject = "Creating/Updating Index : ";
                                break;

                            case "Tables\\Indexes_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Indexes.xml";
                                BuildObject = "Creating/Updating Index : ";
                                break;

                            case "Tables\\Triggers":
                                XMLFile = XMLFile + "\\Triggers.xml";
                                BuildObject = "Creating/Updating Trigger : ";
                                break;

                            case "Tables\\Triggers_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Triggers.xml";
                                BuildObject = "Creating/Updating Trigger : ";
                                break;

                            case "Tables\\InsertionScripts":
                                XMLFile = XMLFile + "\\InsertionScripts.xml";
                                BuildObject = "Populating data for ";
                                break;

                            case "Tables\\InsertionScripts_Workflow":
                                XMLFile = XMLFile + "\\Workflow_InsertionScripts.xml";
                                BuildObject = "Populating data for ";
                                break;

                            case "Functions\\CLR Functions":
                                XMLFile = XMLFile + "\\CLR_Functions.xml";
                                BuildObject = "Creating/Updating Function : ";
                                break;

                            case "Functions":
                            case "Programmability\\Functions":
                                XMLFile = XMLFile + "\\Functions.xml";
                                BuildObject = "Creating/Updating Function : ";
                                break;

                            case "Functions_Workflow":
                            case "Programmability\\Functions_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Functions.xml";
                                BuildObject = "Creating/Updating Function : ";
                                break;

                            case "Views":
                                XMLFile = XMLFile + "\\Views.xml";
                                BuildObject = "Creating/Updating Views : ";
                                break;

                            case "Views_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Views.xml";
                                BuildObject = "Creating/Updating Views : ";
                                break;

                            case "Stored Procedures":
                            case "Programmability\\Stored Procedures":
                                XMLFile = XMLFile + "\\Procedures.xml";
                                BuildObject = "Creating/Updating Stored Procedure : ";
                                break;

                            case "Stored Procedures_Workflow":
                            case "Programmability\\Stored Procedures_Workflow":
                                XMLFile = XMLFile + "\\Workflow_Procedures.xml";
                                BuildObject = "Creating/Updating Stored Procedure : ";
                                break;

                            case "Types\\User Defined Table Types":
                            case "Programmability\\Types\\User Defined Table Types":
                                XMLFile = XMLFile + "\\Types.xml";
                                BuildObject = "Creating/Updating User Defined Table Type : ";
                                break;

                            case "Scripts\\Pre-Deployment":
                                XMLFile = XMLFile + "\\Scripts.xml";
                                BuildObject = "Executing Script : ";
                                break;

                            case "Scripts":
                                XMLFile = XMLFile + "\\Scripts.xml";
                                BuildObject = "Executing Script : ";
                                break;
                        }

                        #region checking and removing '_Workflow'
                        if ((DBConfiguration.ToLower() == "pc_dev" || DBConfiguration.ToLower() == "pc_dev_subupdate") && BuildItem.ToLower().EndsWith("_workflow"))
                        {
                            XMLFile = XMLFile.Replace("_Workflow", "");
                            filepath = filepath.Replace("_Workflow", "");
                        }
                        #endregion

                        #region Fetching files from XML
                        if (File.Exists(XMLFile))
                        {
                            XmlTextReader reader = new XmlTextReader(XMLFile);
                            while (reader.Read())
                            {
                                XmlNodeType nodeType = reader.NodeType;
                                switch (nodeType)
                                {
                                    case XmlNodeType.Text:
                                        string SQLFilePath = string.Format("{0}\\{1}", filepath, reader.Value);

                                        if (File.Exists(SQLFilePath))
                                        {
                                            if (CustomUserDetails[0] == "Service User" && AuthenticationType == "Windows Authentication")
                                            {
                                                string command = string.Format("{0}\"{1}\\{2}\"",
                                                                            StrOsql,
                                                                            filepath,
                                                                            reader.Value
                                                                            );
                                                command = command.Replace("SQLCMD", "");

                                                Sw.WriteLine(command);
                                            }
                                            else
                                            {
                                                Sw.WriteLine(string.Format("echo {0}{1}", BuildObject, reader.Value));
                                                Sw.WriteLine(string.Format("{0}\"{1}\\{2}\"",
                                                                            StrOsql,
                                                                            filepath,
                                                                            reader.Value
                                                                            )
                                                            );

                                                Sw.WriteLine(string.Format("SET SQLFile={0}", reader.Value));
                                                Sw.WriteLine("CALL LogError.bat %ErrFile% %SQLFile% %LogFile%");
                                                Sw.WriteLine();
                                            }
                                        }
                                        else
                                        {
                                            StreamWriter ErrWr = new StreamWriter(LogFileName, true);
                                            ErrWr.WriteLine(string.Format("File not found : {0}\\{1}", BuildConfig, reader.Value));
                                            ErrWr.WriteLine();
                                            ErrWr.Close();
                                        }

                                        break;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.EventLog.WriteEntry("Riversand MDMCenter Installation", "Error executing SQL objects\n" + exp.Message);
            }
        }

        public static void WriteToFile(string msg, string FilePath)
        {
            try
            {
                FileStream fs1 = new FileStream(FilePath.ToString(), FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write(msg);
                sw.Write(sw.NewLine);
                sw.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Riversand WriteErrorLogToFile failed for DB Build: ", ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
