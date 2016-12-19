using System;
using System.Text;
using System.Xml;

namespace Riversand.JobService.Interfaces
{
    using MDM.Utility;

	/// <summary>
	/// This class facilitates the manipulation of the Xml describing Multi-step jobs
	/// </summary>
	public class JobXmlWrapper
	{
		public JobXmlWrapper(XmlDocument jobXmlDoc)
		{
			this._jobXmlDoc = jobXmlDoc;
		}

		public static JobXmlWrapper BuildJobXml()
		{
			XmlDocument result = new XmlDocument();
			string jobText =	"<Job>" +
				"	<ConfigInfo/>" +
				"	<ProcessSteps/>" +
				"	<CleanupSteps/>" +
				"	<ContextInformation/>" +
				"	<Messages/>" +
				"   <JobDetails/>" +
				"	<Exceptions/>" +				
				"</Job>";
			result.LoadXml(jobText);
			return new JobXmlWrapper(result);
		}

        public static XmlDocument BuildJobXmlDoc(XmlDocument profileXmlDoc, int fileId)
        {
            return BuildJobXmlDoc(profileXmlDoc, fileId, "Import");
        }

        public static XmlDocument BuildJobXmlDoc(XmlDocument profileXmlDoc, int fileId, string jobType)
        {
            #region sample_XML for JobType = "Import"
            // <Job>
            // <ProcessSteps>
            //    <Step name="ImportLoop" nextElement="0">
            //       <Adapter type="columnarData" fileType="excel" location="localFileSystem" name="D:\\Share\\AmerCable   Data\\MarineProducts1.xls" >
            //          <State>
            //          </State>
            //          <ImportProfilename="AmerCableMarineComponents" Domain="">
            //             ***
            //          </ImportProfile>
            //       </Adapter>
            //    </Step>
            // </ProcessSteps>
            // <CleanupSteps>
            //    <Step name="DeleteLocalFilename"/>
            // </CleanupSteps>
            // <ContextInformation/>
            // <!-- This section is meant for theunderlying process to record non-fatal errors -->
            // <Messages>
            // </Messages>
            // </Job>
            #endregion

            #region Sample Xml for EntityImport JobType
            // <Job>
            // <ProcessSteps>
            //    <Step name="ImportLoop" nextElement="0">
            //       <Adapter type="rsxml" fileType="xml" location="localFileSystem" name="D:\\Share\\AmerCableData\\MarineProducts1.xml" >
            //          <State>
            //          </State>
            //          <ImportProfile name="AmerCableMarineComponents" Domain="" RSXMLSchema="">
            //             ***
            //          </ImportProfile>
            //       </Adapter>
            //    </Step>
            // </ProcessSteps>
            // <CleanupSteps>
            //    <Step name="DeleteLocalFilename"/>
            // </CleanupSteps>
            // <ContextInformation/>
            // <!-- This section is meant for theunderlying process to record non-fatal errors -->
            // <Messages>
            // </Messages>
            // </Job>
            #endregion

            XmlDocument xmlDoc = new XmlDocument();
            // <Job>
            XmlElement jobNode = xmlDoc.CreateElement("Job");
            xmlDoc.AppendChild(jobNode);

            // <ProcessSteps>
            XmlElement processStepsNode = xmlDoc.CreateElement("ProcessSteps");
            jobNode.AppendChild(processStepsNode);

            //get the steps from the profile
            XmlNodeList ProfileNodeList = profileXmlDoc.SelectNodes("//Steps/Step");
            for (int i = 0; i < ProfileNodeList.Count; i++)
            {
                XmlNode ProfileNode = ProfileNodeList[i];
                XmlElement ProfileElement = (XmlElement)ProfileNode;

                switch (ProfileElement.GetAttribute("Name"))
                {
                    case "Cleanup":
                        // <CleanupSteps>
                        XmlElement cleanupStepsNode = xmlDoc.CreateElement("CleanupSteps");
                        jobNode.AppendChild(cleanupStepsNode);
                        //    <Step name="releaseFileReference" fileId="">
                        XmlElement releaseFileReference = xmlDoc.CreateElement("Step");
                        cleanupStepsNode.AppendChild(releaseFileReference);
                        releaseFileReference.SetAttribute("name", "releaseFileReference");
                        if (fileId > 0) { releaseFileReference.SetAttribute("fileId", fileId.ToString()); }
                        break;

                    default:
                        //handles Riversand InitFileCheck, check if file exists, 
                        //PreProcess, PostProcess, Custom Validate, or other stuff						

                        XmlElement DefaultStep = xmlDoc.CreateElement("Step");
                        processStepsNode.AppendChild(DefaultStep);
                        DefaultStep.SetAttribute("name", ProfileElement.GetAttribute("Name"));
                        DefaultStep.SetAttribute("assemblyName", ProfileElement.GetAttribute("AssemblyName"));
                        DefaultStep.SetAttribute("fileName", ProfileElement.GetAttribute("FileName"));
                        DefaultStep.SetAttribute("required", ProfileElement.GetAttribute("Required"));
                        DefaultStep.SetAttribute("AbortOnError", ProfileElement.GetAttribute("AbortOnError"));
                        DefaultStep.SetAttribute("nextElement", ProfileElement.GetAttribute("nextElement"));
                        DefaultStep.SetAttribute("status", ProfileElement.GetAttribute("status"));
                        DefaultStep.SetAttribute("StartTime", "");
                        DefaultStep.SetAttribute("EndTime", "");
                        DefaultStep.SetAttribute("remainingMilliseconds", "");
                        DefaultStep.SetAttribute("totalMilliseconds", "");
                        DefaultStep.SetAttribute("totalElements", "");
                        DefaultStep.SetAttribute("progressComplete", "");

                        if (ProfileElement.GetAttribute("Name") == "StageData")
                        {
                            //Add the additional information for stage data
                            //stage data loads the file and requires the mapping specs
                            #region File_Mapping_And_Handling

                            XmlElement adapterNode = xmlDoc.CreateElement("Adapter");
                            DefaultStep.AppendChild(adapterNode);

                            string adapterType = "columnarData";
                            
                            if (jobType == "EntityImport")
                            {
                                adapterType = "RSXml";
                            }
                            
                            adapterNode.SetAttribute("type", adapterType);   

                            try
                            {
                                string fileType = "xml";

                                if (jobType == "Import")
                                {
                                    fileType = profileXmlDoc.SelectSingleNode("/ImportProfile/InputSpecification/SavedEntry[@selected='true']").Attributes.GetNamedItem("fileType").Value;
                                }
    
                                adapterNode.SetAttribute("fileType", fileType);
                                
                            }
                            catch
                            {    
                                if (profileXmlDoc.SelectSingleNode("//ImportProfile/Steps/Step[@Name='StageData']").Attributes.GetNamedItem("AssemblyName").Value == AppConfigurationHelper.GetAppConfig<String>("Jobs.Imports.Assembly.RS.Columnar.NS.Method"))
                                    throw;
                                else
                                    adapterNode.SetAttribute("fileType", "unknown");
                            }
                            adapterNode.SetAttribute("location", "database");

                            if (fileId > 0) { adapterNode.SetAttribute("fileId", fileId.ToString()); }

                            //          <State>
                            XmlElement stateNode = xmlDoc.CreateElement("State");
                            adapterNode.AppendChild(stateNode);

                            XmlElement importProfileNode = ((XmlElement)(xmlDoc.ImportNode(profileXmlDoc.SelectSingleNode("/ImportProfile"), true)));

                            //Jobsteps have been created, remove profile steps and riversand nodes... 
                            XmlNode RSNode = importProfileNode.SelectSingleNode("Riversand");
                            importProfileNode.RemoveChild(RSNode);
                            XmlNode StepNode = importProfileNode.SelectSingleNode("Steps");
                            importProfileNode.RemoveChild(StepNode);
                            XmlNode PluginNode = importProfileNode.SelectSingleNode("PlugIns");
                            importProfileNode.RemoveChild(PluginNode);

                            adapterNode.AppendChild(importProfileNode);
                            #endregion
                        }
                        break;
                }
            }

            // <ContextInformation>
            XmlElement contextInformationNode = xmlDoc.CreateElement("ContextInformation");
            jobNode.AppendChild(contextInformationNode);
            // <Messages>
            XmlElement messagesNode = xmlDoc.CreateElement("Messages");
            jobNode.AppendChild(messagesNode);

            // <Job Details> for containing JobID 
            XmlElement JobDetailsNode = xmlDoc.CreateElement("JobDetails");
            jobNode.AppendChild(JobDetailsNode);
            JobDetailsNode.SetAttribute("name", "");
            JobDetailsNode.SetAttribute("id", "");

            return xmlDoc;
        }

		public void ReBuildJobXml(string xml)
		{
			this._jobXmlDoc.InnerXml = xml;
		}

		public XmlElement AddProcessStep(string name)
		{
			XmlElement jobStep = _jobXmlDoc.CreateElement("Step");
			jobStep.SetAttribute("name", name);
			jobStep.SetAttribute("status", "pending");
			_jobXmlDoc.SelectSingleNode("/Job/ProcessSteps").AppendChild(jobStep);
			return jobStep;
		}

		public void UpdateProcessStep(string name)
		{
			XmlElement jobStep = _jobXmlDoc.CreateElement("Step");
			jobStep.SetAttribute("name", name);
			jobStep.SetAttribute("status", "Completed");
			//_jobXmlDoc.SelectSingleNode("/Job/ProcessSteps").AppendChild(jobStep);
			return;
		}

        public void UpdateProcessStepStatus(string name,string StartTime, string EndTime, int nextElement, int totalElements, string status ,string dateTimeMask )
        {
            XmlElement jobStep = (XmlElement) _jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='"+name+"']");
            
            jobStep.SetAttribute("nextElement",nextElement.ToString());
            jobStep.SetAttribute("totalElements", totalElements.ToString());

            if ((status != null) && (status.Length > 0))
            {
                if (nextElement >= totalElements)
                    jobStep.SetAttribute("status", "Completed");
                else
                    jobStep.SetAttribute("status", "Running");
            }            
            jobStep.SetAttribute("status", status);
            if (jobStep.GetAttribute("StartTime").Length == 0)
                jobStep.SetAttribute("StartTime", StartTime);
            else
                StartTime = jobStep.GetAttribute("StartTime");

            if (status.ToLower() == "pending")
            {
                jobStep.SetAttribute("StartTime", "");
                jobStep.SetAttribute("EndTime", "");
            }

            //expected format is MMM dd yyyy HH:mm:ss:fff            
            System.TimeSpan elapsedTime = new TimeSpan();
            try 
            {                
                if (dateTimeMask != null)

                    elapsedTime = DateTime.ParseExact(EndTime, dateTimeMask, null).Subtract(DateTime.ParseExact(StartTime, dateTimeMask, null));
                else
                    elapsedTime = DateTime.Parse(EndTime).Subtract(DateTime.Parse(StartTime));
            }
            catch
            {
            }


            decimal progressComplete = 0;
            double remainingProgress = 1;
            if (totalElements > 0)
            {
                decimal d1 = (decimal) nextElement;
                decimal d2 = (decimal) totalElements;
                d2 = decimal.Divide(d1, d2);

                progressComplete = Math.Round(d2, 2);
                if (progressComplete < 1)
                    remainingProgress = (double)(1 - progressComplete);
            }
            jobStep.SetAttribute("progressComplete",progressComplete.ToString());
            TimeSpan _timeRemaining = new TimeSpan();            
            if ((elapsedTime.Ticks > 0) && (nextElement != totalElements) && (nextElement < totalElements) && (StartTime != null) && (StartTime.Length > 0))
            {
                try
                {
                    _timeRemaining = new TimeSpan(0, 0, (int)((elapsedTime.TotalSeconds * (remainingProgress))));
                    if (dateTimeMask != null)
                        EndTime = DateTime.ParseExact(StartTime, dateTimeMask, null).Add(_timeRemaining).ToString();
                    else
                        EndTime = DateTime.Parse(StartTime).Add(_timeRemaining).ToString();                    
                }
                catch
                { 
                }
            }

            jobStep.SetAttribute("EndTime", EndTime);
            jobStep.SetAttribute("remainingMilliseconds", _timeRemaining.TotalMilliseconds.ToString());
            jobStep.SetAttribute("totalMilliseconds", elapsedTime.TotalMilliseconds.ToString());
            return;
        }

        public void createStageDataLoadList(string [] files, string [] tables)
        {
            XmlElement LoadNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='StageData']/Load");
            if (LoadNode == null)
            {
                LoadNode = _jobXmlDoc.CreateElement("Load");
                _jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='StageData']").AppendChild(LoadNode);
            }	

            for (int i = 0; i < files.Length; i++)
            {
                XmlElement loadNode = _jobXmlDoc.CreateElement("LoadList");
                loadNode.SetAttribute("file", files[i]);
                loadNode.SetAttribute("table", tables[i]);
                _jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='StageData']/Load").AppendChild(loadNode);
            }
        }

        public string[] GetStageDataLoadList(string attributename)
        {
            
            XmlNodeList mapNodeList = _jobXmlDoc.SelectNodes("/Job/ProcessSteps/Step[@name='StageData']/Load/LoadList");
            string[] list = new string[mapNodeList.Count];
            int i = 0;
            if (mapNodeList != null)
            {
                
                foreach (XmlNode MapNode in mapNodeList)
                {                    
                    list[i] = MapNode.Attributes.GetNamedItem(attributename).Value;
                    i = i + 1;
                }
            }
            return list;
        }

        public void RemoveStageDataLoadList()
        {
            //removes all the exist LoadList             

            XmlElement node = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='StageData']/Load");
            if (node != null)
            {
                node.InnerXml = "";
            }
        }

		public XmlElement AddCleanupStep(string name)
		{
			XmlElement CleapupNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/CleanupSteps");
			if (CleapupNode == null)
			{
				CleapupNode = _jobXmlDoc.CreateElement("CleanupSteps");
				_jobXmlDoc.SelectSingleNode("/Job").AppendChild(CleapupNode);
			}		

			XmlElement jobStep = _jobXmlDoc.CreateElement("Step");
			jobStep.SetAttribute("name", name);
			_jobXmlDoc.SelectSingleNode("/Job/CleanupSteps").AppendChild(jobStep);
			return jobStep;
		}

		public XmlNodeList GetCleanupSteps()
		{
			return _jobXmlDoc.SelectNodes("//Job/CleanupSteps/Step");
		}

		public void CleanupStepSetAttributes(string stepname,string filename, string fileid, string directoryname )
		{			
			XmlElement jobstep = AddCleanupStep(stepname);
			jobstep.SetAttribute("file",filename);
			jobstep.SetAttribute("fileId",fileid);
            jobstep.SetAttribute("directory",directoryname);
		}

        public void CleanupStepAddList(string[] list, string stepname)
        {
            string step = stepname.ToLower();
            for (int i = 0; i < list.Length; i++)
            {
                switch (step)
                {
                    case "deletedirectory":
                        CleanupStepSetAttributes(step, "", "", list[i]);                        
                        break;
                    case "deletefile":
                        CleanupStepSetAttributes(step, list[i], "", "");
                        break;
                    case "releasefilereference":
                        CleanupStepSetAttributes(step, "", list[i], "");
                        break;
                    default:
                        string message = "expects values: deletedirectory, deletefile, deletefilereference";
                        throw new Exception("Unexpected Cleanup Step stepname; " + message);                        
                }                
            }
        }

		public void AddJobDetails(string name, string thevalue)
		{
			XmlElement jobdetailsNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/JobDetails");
			if (jobdetailsNode == null)
			{
				jobdetailsNode = _jobXmlDoc.CreateElement("JobDetails");
				_jobXmlDoc.SelectSingleNode("/Job").AppendChild(jobdetailsNode);
			}							
			
			XmlElement jobStep = (XmlElement) _jobXmlDoc.SelectSingleNode("/Job/JobDetails");
			jobStep.SetAttribute(name, thevalue);
			
			
		}

        public string GetJobDetails(string name)
        {
            string thevalue = "";
            XmlElement jobdetailsNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/JobDetails");
            if (jobdetailsNode != null)
            {
                thevalue = jobdetailsNode.Attributes.GetNamedItem(name).Value;
            }

            return thevalue;

        }

        public void AddMessageID(int jobId)
        {
            XmlElement jobdetailsNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/MessageId");
            if (jobdetailsNode == null)
            {
                jobdetailsNode = _jobXmlDoc.CreateElement("MessageId");
                _jobXmlDoc.SelectSingleNode("/Job").AppendChild(jobdetailsNode);
            }

            _jobXmlDoc.SelectSingleNode("/Job/MessageId").InnerText = jobId.ToString();            
        }

		public void AddContextInformationNode(XmlElement contextInformationXmlNode)
		{
			_jobXmlDoc.SelectSingleNode("/Job/ContextInformation").AppendChild(contextInformationXmlNode);
		}

		public XmlElement AddContextInformationNode(string name)
		{
			XmlElement result = _jobXmlDoc.CreateElement(name);
			_jobXmlDoc.SelectSingleNode("/Job/ContextInformation").AppendChild(result);
			return result;
		}

		public XmlElement GetContextInformationNode(string name)
		{
			return (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/ContextInformation/" + name);
		}

		public void LogException(string message, Exception e)
		{
            XmlElement exceptionsNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/Exceptions");
            if (exceptionsNode == null)
            {
                exceptionsNode = _jobXmlDoc.CreateElement("Exceptions");
                _jobXmlDoc.SelectSingleNode("/Job").AppendChild(exceptionsNode);
            }

            XmlElement exNode = _jobXmlDoc.CreateElement("Exception");
            if (e.ToString().IndexOf("Errorlog") > 0)
            {
                exNode.SetAttribute("customerErrorMessage", "Click to see details.");
            }
            else
            {
                exNode.SetAttribute("customerErrorMessage", "Configuration error. Please contact administrator for details.");
            }

            StringBuilder exceptionDetails = new StringBuilder();
            exceptionDetails.Append("<b>Error Type: </b>Unhandled Exception<br/>");
            exceptionDetails.AppendFormat("<b>Error Message: </b>{0}<br/>", e.Message);

            if (e.InnerException != null && !String.IsNullOrEmpty(e.InnerException.Message) && String.Compare(e.Message, e.InnerException.Message) != 0)
		    {
		        exceptionDetails.AppendFormat("<b>Additional error Message: </b>{0}<br/>", e.InnerException.Message);
		    }

            exceptionDetails.AppendFormat("<b>Error In: </b>{0}<br/>", e.TargetSite);
            exceptionDetails.AppendFormat("<b>Call Stack: </b>{0}", e.StackTrace);
             
            exNode.SetAttribute("message", message);
            exNode.SetAttribute("exception", exceptionDetails.ToString());       
            exceptionsNode.AppendChild(exNode);
		}

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="error">Indicates the error</param>
        /// <param name="errorDetails">Indicates the error message</param>
        public void LogException(String error, String errorDetails)
        {
            XmlElement exceptionsNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/Exceptions");
            if (exceptionsNode == null)
            {
                exceptionsNode = _jobXmlDoc.CreateElement("Exceptions");
                _jobXmlDoc.SelectSingleNode("/Job").AppendChild(exceptionsNode);
            }

            XmlElement exNode = _jobXmlDoc.CreateElement("Exception");
            exNode.SetAttribute("message", error);
            exNode.SetAttribute("exception", errorDetails);    
            exNode.SetAttribute("customerErrorMessage", errorDetails);
            exceptionsNode.AppendChild(exNode);
        }

		public void ClearExceptions()
		{
			ClearNode("Exceptions");
		}

		public XmlNodeList GetSteps()
		{
			return _jobXmlDoc.SelectNodes("/Job/ProcessSteps/Step");
		}

		public XmlElement GetStep(string name)
		{
			return (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='" + name + "']");
		}

		public XmlElement GetScopeNode()
		{
			return (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/ContextInformation/ExportProfile/CatalogSpecification/SavedEntry/EntryData/Scope");
		}

		public XmlElement GetMessagesNode()
		{
			XmlElement messagesNode = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/Messages");
			if (messagesNode == null)
			{
				messagesNode = _jobXmlDoc.CreateElement("Messages");
				_jobXmlDoc.SelectSingleNode("/Job").AppendChild(messagesNode);
			}
			return messagesNode;
		}
		public void createMessageNodes(string message,string path)
		{
			XmlElement messageNode = _jobXmlDoc.CreateElement("Message");
			messageNode.SetAttribute("title",message);
			messageNode.SetAttribute("description", path);
			_jobXmlDoc.SelectSingleNode("/Job/Messages").AppendChild(messageNode);
		}
		
		public void ClearMessages()
		{
			ClearNode("Messages");
		}

        public System.Data.SqlTypes.SqlXml sqlxml
        {
            get
            {
                XmlDocument xml = _jobXmlDoc;
                System.Data.SqlTypes.SqlXml sx;
                using (XmlNodeReader xnr = new XmlNodeReader(xml))
                {
                    sx = new System.Data.SqlTypes.SqlXml(xnr);
                }
                return sx;
            }
        }

		public string xml
		{
			get
			{
				return _jobXmlDoc.OuterXml;
			}
		}

		public string innerxml
		{
			get 
			{
				return _jobXmlDoc.InnerXml;
			}
		}

		private void ClearNode(string name)
		{
			XmlElement node = (XmlElement)_jobXmlDoc.SelectSingleNode("/Job/" + name);
			if (node != null)
			{
				node.InnerXml = "";
				//				XmlNodeList children = node.ChildNodes;
				//				if (children != null)
				//				{
				//					foreach(XmlElement child in children)
				//					{
				//						node.RemoveChild(child);
				//					}
				//				}
			}
		}

		public XmlDocument JobXmlDoc
		{
			get
			{
				return _jobXmlDoc;
			}
			set
			{
				_jobXmlDoc = value;
			}
		}
        
		private XmlDocument _jobXmlDoc;
	}
}
