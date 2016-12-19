using System;
using System.Xml;

namespace Riversand.JobService
{
	/// <summary>
	/// This class facilitates the manipulation of the Xml describing Multi-step jobs
	/// </summary>
	public class JobXmlWrapper
	{
		public JobXmlWrapper(XmlDocument jobXmlDoc)
		{
			this.jobXmlDoc = jobXmlDoc;
		}

		public static JobXmlWrapper BuildJobXml()
		{
			XmlDocument result = new XmlDocument();
	        string jobText =	"<Job>" +
								"	<ProcessSteps/>" +
            					"	<CleanupSteps/>" +
            					"	<ContextInformation/>" +
            					"	<Messages/>" +
            					"	<Exceptions/>" +
								"</Job>";
			result.LoadXml(jobText);
			return new JobXmlWrapper(result);
		}

		public void ReBuildJobXml(string xml)
		{
			this.jobXmlDoc.InnerXml = xml;
		}
		public XmlElement AddProcessStep(string name)
		{
			XmlElement jobStep = jobXmlDoc.CreateElement("Step");
			jobStep.SetAttribute("name", name);
			jobStep.SetAttribute("status", "pending");
			jobXmlDoc.SelectSingleNode("/Job/ProcessSteps").AppendChild(jobStep);
			return jobStep;
		}

		public XmlElement AddCleanupStep(string name)
		{
			XmlElement jobStep = jobXmlDoc.CreateElement("Step");
			jobStep.SetAttribute("name", name);
			jobXmlDoc.SelectSingleNode("/Job/CleanupSteps").AppendChild(jobStep);
			return jobStep;
		}

		public void AddContextInformationNode(XmlElement contextInformationXmlNode)
		{
			jobXmlDoc.SelectSingleNode("/Job/ContextInformation").AppendChild(contextInformationXmlNode);
		}

		public XmlElement AddContextInformationNode(string name)
		{
			XmlElement result = jobXmlDoc.CreateElement(name);
			jobXmlDoc.SelectSingleNode("/Job/ContextInformation").AppendChild(result);
			return result;
		}

		public XmlElement GetContextInformationNode(string name)
		{
			return (XmlElement)jobXmlDoc.SelectSingleNode("/Job/ContextInformation/" + name);
		}

		public void LogException(string message, Exception e)
		{
			XmlElement exceptionsNode = (XmlElement)jobXmlDoc.SelectSingleNode("/Job/Exceptions");
			if (exceptionsNode == null)
			{
				exceptionsNode = jobXmlDoc.CreateElement("Exceptions");
				jobXmlDoc.SelectSingleNode("/Job").AppendChild(exceptionsNode);
			}

			XmlElement exNode = jobXmlDoc.CreateElement("Exception");
			if (e.ToString().IndexOf("Errorlog")>0)
			{
				exNode.SetAttribute("customerErrorMessage", "Click to see details.");
			}
			else
			{
				exNode.SetAttribute("customerErrorMessage", "Configuration error. Please contact administrator for details.");
			}
			exNode.SetAttribute("message", message);
			exNode.SetAttribute("exception", e.ToString());
			exceptionsNode.AppendChild(exNode);
		}

		public void ClearExceptions()
		{
			ClearNode("Exceptions");
		}

		public XmlNodeList GetSteps()
		{
			return jobXmlDoc.SelectNodes("/Job/ProcessSteps/Step");
		}

		public XmlElement GetStep(string name)
		{
			return (XmlElement)jobXmlDoc.SelectSingleNode("/Job/ProcessSteps/Step[@name='" + name + "']");
		}
		public XmlElement GetScopeNode()
		{
			return (XmlElement)jobXmlDoc.SelectSingleNode("/Job/ContextInformation/ExportProfile/CatalogSpecification/SavedEntry/EntryData/Scope");
		}

		public XmlElement GetMessagesNode()
		{
			XmlElement messagesNode = (XmlElement)jobXmlDoc.SelectSingleNode("/Job/Messages");
			if (messagesNode == null)
			{
				messagesNode = jobXmlDoc.CreateElement("Messages");
				jobXmlDoc.SelectSingleNode("/Job").AppendChild(messagesNode);
			}
			return messagesNode;
		}
		public void createMessageNodes(string message,string path)
		{
			XmlElement messageNode = jobXmlDoc.CreateElement("Message");
			messageNode.SetAttribute("title",message);
			messageNode.SetAttribute("description", path);
			jobXmlDoc.SelectSingleNode("/Job/Messages").AppendChild(messageNode);
		}
		
		public void ClearMessages()
		{
			ClearNode("Messages");
		}

		public string xml
		{
			get
			{
				return jobXmlDoc.OuterXml;
			}
		}

		public string innerxml
		{
			get 
			{
				return jobXmlDoc.InnerXml;
			}
		}
		private void ClearNode(string name)
		{
			XmlElement node = (XmlElement)jobXmlDoc.SelectSingleNode("/Job/" + name);
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

		private XmlDocument jobXmlDoc;
	}
}
