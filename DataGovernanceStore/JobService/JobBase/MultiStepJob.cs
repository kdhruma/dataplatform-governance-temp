using System;
using System.Reflection;
using System.Xml;
using System.Configuration;
using System.Net.Mail;
using System.Data;
using System.Net.Configuration;

namespace Riversand.JobService
{
    using MDM.Utility;
    using MDM.Core;
    using MDM.BusinessObjects.Diagnostics;

	/// <summary>
	/// This class assumes the Job XML has a number of steps. Initially we had multiple job classes 
	/// that used the same code and decided to pull all the common code into this class
	/// </summary>
	public abstract class MultiStepJob : Job
	{
        private static string datetimeMask = "MMM dd yyyy HH:mm:ss:fff";

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = new TraceSettings();

		public MultiStepJob(XmlElement jobXmlNode, int jobId) : base(jobXmlNode, jobId)
		{
            _currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
			//Run();
		}

		/// <summary>
		/// Begins the job execution, will stop if the job is cancelled
		/// </summary>
				
		public override void Run()
		{
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

		    try
		    {
		        if (_currentTraceSettings.IsBasicTracingEnabled)
		        {
		            diagnosticActivity.LogInformation("Trying Multistep job");
		        }
		        
		        //Iterate through the regular job steps
		        XmlNodeList steps = jobXmlWrapper.GetSteps();

		        //get JobId														
		        XmlDocument JobXmlDoc = jobXmlWrapper.JobXmlDoc;
		        XmlElement JobIdElement = (XmlElement) JobXmlDoc.SelectSingleNode("//Job/JobDetails");
		        int jobId = int.Parse(JobIdElement.GetAttribute("id"));
		        string StartTime = System.DateTime.Now.ToString(datetimeMask);

		        string directoryPath = AppConfigurationHelper.GetAppConfig<String>("Integration.PluginPath");
		        if ((directoryPath.EndsWith(@"\") == false) && (directoryPath.EndsWith("/") == false))
		            directoryPath = directoryPath + @"\";
		        int fileId = 0;
		        try
		        {
		            Int32.Parse(JobXmlDoc.SelectSingleNode("//Adapter").Attributes.GetNamedItem("fileId").Value);
		        }
		        catch (Exception ex)
		        {
                    diagnosticActivity.LogError("MultiStepJob doesn't have job fileId. (ex.Message = " + ex.ToString() + ")");
                   
		        }

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Trying Reset job steps.");
                }

		        Riversand.StoredProcedures.JobBase.ImportProductCleanup(jobId);
		        foreach (XmlElement stepNode in steps)
		        {
		            //starting job, new or retry, reset status and values to new 
		            string stepName = stepNode.GetAttribute("name");
		            jobXmlWrapper.UpdateProcessStepStatus(stepName, null, null, 0, 0, "Pending", datetimeMask);
		        }

		        jobXmlWrapper.RemoveStageDataLoadList();
		        Riversand.StoredProcedures.JobBase.UpdateJobData(jobId, jobXmlWrapper.sqlxml);

		        steps = jobXmlWrapper.GetSteps();
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Starting job steps.");
                }
		        
		        int stepcount = 0;
		        int totalStepCount = steps.Count;
		        System.Data.SqlTypes.SqlDateTime jobStartTime = new System.Data.SqlTypes.SqlDateTime(System.DateTime.Now);


		        System.Data.SqlTypes.SqlDateTime jobEndTime = new System.Data.SqlTypes.SqlDateTime();
		        EMailMessage("OnBegin", JobXmlDoc, jobId);
		        foreach (XmlElement stepNode in steps)
		        {
		            stepcount++;
		            string stepName = stepNode.GetAttribute("name");
		            string path = "/Job/ProcessSteps/Step[@name=" + "\"" + stepName + "\"" + "]";

                    if (_currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogInformation("Running Job step" + stepName);
                    }

		            if (cancelJob)
		            {
		                break;
		            }

		            if (stepNode.GetAttribute("status").ToLower() != "completed")
		            {
		                try
		                {

		                    string description = "Running " + stepName + " Step [ " + stepcount.ToString() + " of " +
		                                         totalStepCount.ToString() + " ]";
		                    Riversand.StoredProcedures.JobBase.UpdateJobInformation(jobId, fileId, description, "Running", 0,
		                        0, 0, jobStartTime, jobEndTime);
		                    string assemblyName = stepNode.GetAttribute("assemblyName");
		                    string fileName = stepNode.GetAttribute("fileName");
		                    if ((assemblyName.Length > 0) && (fileName.Length > 0))
		                    {
		                        //this is a import job with external assemblies 

		                        fileName = fileName.Replace("/", @"\");

		                        //  Split the File Name and Path -- this should only be filename but just incase, use plugin path for directory
		                        string strFilePath = "";
		                        string strFileName = "";
		                        if (fileName.Contains(@"\"))
		                        {
		                            strFilePath = fileName.Substring(0, fileName.LastIndexOf(@"\"));
		                            strFileName = fileName.Substring(strFilePath.Length + 1);
		                            fileName = directoryPath + strFileName;
		                        }
		                        else
		                            fileName = directoryPath + fileName;

		                        // allow configuration for external dll to run a step
		                        //Interfaces.IJobStep externalStep = GetExternalStep(stepNode);												
		                        //if (externalStep != null) {

		                        Assembly externalAssembly = Assembly.LoadFile(fileName);
		                        object instanceObject = externalAssembly.CreateInstance(assemblyName);

		                        MethodInfo callMethod = instanceObject.GetType().GetMethod("RunStep");

		                        if (_currentTraceSettings.IsBasicTracingEnabled)
		                        {
		                            diagnosticActivity.LogInformation("Calling external assembly" + assemblyName);
		                        }

		                        object result = callMethod.Invoke(instanceObject,
		                            new object[] {stepName, jobXmlWrapper, jobId});

		                        if (_currentTraceSettings.IsBasicTracingEnabled)
		                        {
		                            diagnosticActivity.LogInformation("Completed external assembly step" + assemblyName);
		                        }

		                        jobXmlWrapper.ReBuildJobXml(Riversand.StoredProcedures.JobBase.GetJobData(jobId));
		                    }
		                    else
		                    {
		                        RunStep(stepNode);
		                    }
		                }
		                catch (Exception e)
		                {
		                    String message = "Step Execution Failed: [" + stepNode.Attributes.GetNamedItem("name").Value + "]";
		                    if (e.InnerException != null)
		                    {
		                        message = String.Concat(message, " (ex.Message = ", e.InnerException.Message, ")");
		                    }
		                    else
		                    {
		                        message = String.Concat(message, " (ex.Message =  ", e.Message, ")");
		                    }
		                    // Dont need to Log/save, since the logging is done in the outer most wrapper 						
		                    diagnosticActivity.LogError(String.Concat(message, " (ex.Message = ", e.ToString(), ")"));
		                    string description = "Failed: " + stepName + " Step [ " + stepcount.ToString() + " of " +
		                                         totalStepCount.ToString() + " ]";
		                    Riversand.StoredProcedures.JobBase.UpdateJobInformation(jobId, fileId, description, "Aborted", 0,
		                        0, 0, jobStartTime, jobEndTime);
		                    CleanupFilesAndDB(jobId);
		                    EMailMessage("OnFailure", JobXmlDoc, jobId);
		                    throw new Exception(message, e);
		                }
		            }
		            jobXmlWrapper.UpdateProcessStepStatus(stepName, StartTime, System.DateTime.Now.ToString(datetimeMask), 1,
		                1, "Completed", datetimeMask);
		            Riversand.StoredProcedures.JobBase.UpdateJobData(jobId, jobXmlWrapper.sqlxml);
		        }
		        if (IsComplete())
		        {
		            jobEndTime = new System.Data.SqlTypes.SqlDateTime(System.DateTime.Now);
		            int recordcount = 0;
		            try
		            {
		                recordcount = int.Parse(jobXmlWrapper.GetJobDetails("recordCount"));
		            }
		            catch
		            {
		            }
		            description = "Processed " + recordcount + " records.";
		            Riversand.StoredProcedures.JobBase.UpdateJobInformation(jobId, fileId, description, "Completed", 0, 0,
		                recordcount, jobStartTime, jobEndTime);
		            CleanupFilesAndDB(jobId);
		            EMailMessage("OnComplete", JobXmlDoc, jobId);
		        }
		    }
		    catch (Exception ex)
		    {
		        string message = "MultiStepJob.Run() failed.";
                diagnosticActivity.LogError(String.Concat(message, " (ex.Message =  ", ex.ToString(), ")"));
		        throw new Exception(String.Concat(message, " (ex.Message =  ", ex.Message, ")"), ex);
		    }
		    finally
		    {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
		    }
		}

        private void EMailMessage(string messageType, XmlDocument JobXmldoc, int jobId)
        {
            #region "Notifications"
            /*
            string OnBegin = "";
            string OnComplete = "";
            string OnSuccess = "";
            string OnFailure = "";
            */
            string emailToAddress = "";
            string profileName = "";
            try 
            {
                XmlElement notification = (XmlElement)JobXmldoc.SelectSingleNode("//Notifications/Element[@name='Email']");
                /*OnBegin = notification.GetAttribute("OnBegin");
                OnComplete = notification.GetAttribute("OnComplete");
                OnSuccess = notification.GetAttribute("OnSuccess");
                OnFailure = notification.GetAttribute("OnFailure");                
                */
                emailToAddress = notification.GetAttribute(messageType);                
                try 
                {
                    if (JobXmldoc.SelectSingleNode("//ImportProfile") != null)
                    {
                        profileName = JobXmldoc.SelectSingleNode("//ImportProfile").Attributes.GetNamedItem("name").Value;
                    }
                    else if (JobXmldoc.SelectSingleNode("//ContextInformation/Profile") != null)
                    {
                        profileName = JobXmldoc.SelectSingleNode("//ContextInformation/Profile").Attributes.GetNamedItem("Name").Value;
                    }
                }

                catch
                {   
                    profileName = "[No Profile Name Found]";
                }
            }
            catch
            {
            }
            #endregion "Notifications"

            if ((emailToAddress != null) && (emailToAddress.Length > 0))
            {
                string message = "";
                switch (messageType)
                {
                    case "OnBegin":  
                        message = "This is an automated mailer.  Job started at "+ System.DateTime.Now.ToString() + ".  Profile Name: "+profileName;
                        break;
                    case "OnComplete":
                        message = "This is an automated mailer.  Job completed with non-fatal errors at " + System.DateTime.Now.ToString() + ".  Profile Name: " + profileName;
                        break;
                    case "OnSuccess":
                        message = "This is an automated mailer.  Job successfully completed at " + System.DateTime.Now.ToString() + ".  Profile Name: " + profileName;
                        break;
                    case "OnFailure":
                        message = "This is an automated mailer.  Job failed at " + System.DateTime.Now.ToString() + ".  Profile Name: " + profileName;
                        break;
                    default:
                        message = "This is an automated mailer.  There is an error in Job services Notifications with Message Type.";
                        break;
                }

                // email sending process will consider from email address in below sequence
                // 1. SmtpClient will get it from smtp config of mailSettings > smtp > network - "username"
                // or 2. SmtpSection will get it from smtp config of mailsettings > smtp - "from"
                // or 3. automailer@riversand.com
                string emailfrom = String.Empty;

                SmtpSection smtpConfig = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                if (smtpConfig != null && smtpConfig.From != null && !String.IsNullOrEmpty(smtpConfig.From))
                {
                    emailfrom = smtpConfig.From;
                }
                else
                {
                    emailfrom = "automailer@riversand.com";
                }
               
                string Body = message;
                string Subject = "Riversand Nofication" + jobId.ToString() +", "+ profileName + ", " + messageType;
                try 
                {
                    //.Net 2.0 System.Net.Mail
                    MailMessage mail = new MailMessage(emailfrom,emailToAddress, Subject, Body);
                    /*
                    DataSet joberrorlog = Riversand.StoredProcedures.ImportExport.ImportJobErrorLogExists(jobId.ToString());
                    if (joberrorlog.Tables[0].Rows.Count > 0)
                    {
                        DataSet ds = new DataSet();                        
                        DataTable dt = JobDBShell.FindImportJobLog(jobId);
                        ds.Tables.Add(dt);
                        string[] files = { "errors"};
                        string fileNameList = "";
                        try
                        {
                            Riversand.BulkWriter.BulkWriter bw = new Riversand.BulkWriter.BulkWriter(files);
                            string FieldTerminator = "|";
                            string RowTerminator = "\n";
                            fileNameList = bw.BulkWriteDataSet(ds, jobId, FieldTerminator, RowTerminator, files);
                            Attachment data = new Attachment();
                            mail.Attachments.Add(attachment);
                        }
                        catch (Exception ex) { }                
                    }
                    */
                    System.Net.Mail.SmtpClient client = new SmtpClient();
                    string enablessl = "";
                    try
                    {
                        enablessl = ConfigurationManager.AppSettings.Get("enablessl");
                        if (enablessl.ToLower() == "true")
                            client.EnableSsl = true;
                    }
                    catch
                    { }
                    

                    client.Send(mail);

                    //SmtpMail.SmtpServer = "127.0.0.1";
                    /*
                    System.Net.Mail.MailAddress
                    SmtpClient server = new SmtpClient("127.0.0.1");
                    server.Credentials = new NetworkCredential("user", "pass");
                    server.Send(email);
                     */

                } catch
                {
                }
            }

        }

        private void CleanupFilesAndDB(int jobId)
        {
            bool debugmode = false;
            try
            {
                debugmode = bool.Parse(AppConfigurationHelper.GetAppConfig<String>("Jobs.DebugMode"));
            }
            catch (Exception ex)
            {
                string message = "LoadStagingTablesFromDSFiles, Debug Mode = false, (ex.Message = " + ex.ToString() + ")";

                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.LogError(message);
            }

            if (debugmode == false)
            {
                Riversand.StoredProcedures.JobBase.ImportProductCleanup(jobId);
                CleanUp();
            }
            SaveJob();
        }

		/// <summary>
		/// Indicates if the job has completed all its steps
		/// </summary>
		/// <returns>True if completed, false otherwise</returns>
		public override bool IsComplete()
		{
			//Iterate through the regular job steps
			XmlNodeList steps = jobXmlWrapper.GetSteps();

			foreach(XmlElement stepNode in steps)
			{
				if ( stepNode.GetAttribute("status").ToLower() != "completed" )
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Check whether the job is ignored or not
		/// </summary>
		/// <returns>True if ignored, false otherwise</returns>
        public override Boolean IsIgnored()
        {
            //Iterate through the regular job steps
            XmlNodeList steps = jobXmlWrapper.GetSteps();

            foreach (XmlElement stepNode in steps)
            {
                if (String.Compare(stepNode.GetAttribute("status"), JobStatus.Ignored.ToString(), true) != 0)
                {
                    return false;
                }
            }
            return true;
        }


		/// <summary>
		/// Adds the to the total time of a job
		/// </summary>
		/// <param name="step"></param>
		/// <param name="startDate"></param>
		/// <returns>The number of milliseconds from startDate</returns>
		protected double AddStepTotalTime(XmlElement step, DateTime startDate)
		{
			DateTime endDate = DateTime.Now;
			TimeSpan span = endDate.Subtract(startDate);

			double totalMilliseconds = 0;
			if (step.GetAttribute("totalMilliseconds") == null || step.GetAttribute("totalMilliseconds") == "")
			{
				step.SetAttribute("totalMilliseconds", "0");
			}
			else
			{
				totalMilliseconds = Double.Parse(step.GetAttribute("totalMilliseconds"));
			}

			totalMilliseconds += span.TotalMilliseconds;

			step.SetAttribute("totalMilliseconds", totalMilliseconds.ToString());

			return span.TotalMilliseconds;
		}

		protected Interfaces.IJobStep GetExternalStep(XmlElement step)
		{
			string fileName = step.GetAttribute("fileName");
			if (fileName == null || fileName=="")
				return null;

			//try to load the assembly 
			string stepName = step.GetAttribute("assemblyName");
			Assembly externalAssembly = Assembly.LoadFrom(fileName);						
											
			return externalAssembly.CreateInstance(stepName) as Interfaces.IJobStep;
		}

		protected abstract void RunStep(XmlElement stepNode);
	}
}
