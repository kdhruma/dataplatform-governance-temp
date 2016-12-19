using System;
using System.Xml;

namespace Riversand.JobService.Interfaces
{
	/// <summary>
	/// Summary description for IImportEventHandler.
	/// </summary>
	public interface IImportEventHandler
	{
		void OnBegin();
		void OnProcessBatch(XmlDocument catalogXmlDoc, XmlDocument cNodesXmlDoc);
		void OnEnd();
	}
}
