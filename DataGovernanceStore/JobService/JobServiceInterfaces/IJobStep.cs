using System;
using System.Xml;

namespace Riversand.JobService.Interfaces
{
	/// <summary>
	/// Summary description for IJobStep.
	/// </summary>
	public interface IJobStep
	{
		void Run(XmlElement step, IJob owner);
		//void Run(string step, JobXmlWrapper jobXmlWrapper, int jobId);		
	}
}
