using System; 
using System.Data.SqlClient; 
using System.Diagnostics; 
using System.Threading; 
using System.Xml; 
using System.Windows.Forms; 
//using EPDM.ApplicationServices; 
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;

namespace Riversand.JobService 
{
    using MDM.Core;
    using MDM.JobManager.Business;
    
	public class JobDB
    {
        public JobDB() 
		{             
		}

        public DataTable GetJobsDT(string type, string username, string SQL)
        {
            return Riversand.StoredProcedures.JobBase.GetJobsDT(type, username, SQL);
            /*
            InternalData dataInternal = new InternalData(); 
            dataInternal.AddInputParameter( "@nvchrType", SqlDbType.NVarChar, 50, type ); 
            dataInternal.AddInputParameter( "@nvchrUsername", SqlDbType.NVarChar, 50, username ); 
            return dataInternal.ExecuteSpGetDT( "usp_N_getJobServiceItems" ); 
            */
        }

        /// <summary>
        /// Returns the job XML
        /// </summary>
        /// <param name="bIncludeInactive">Specifies whether to include inactive jobs</param>
        /// <param name="jobServiceName">Specifies the job service instance name</param>
        /// <returns>A String representing the job XML</returns>
        public String GetJobsXml(Boolean bIncludeInactive, String jobServiceName) 
		{
            return GetJobsXml(bIncludeInactive, JobServiceType.All, jobServiceName);
			/*
			InternalData dataInternal = new InternalData(); 
			return dataInternal.ExecuteSpGetXML( "usp_N_getJobService_XML" ); 
			*/
		}

        /// <summary>
        /// Returns the job XML
        /// </summary>
        /// <param name="bIncludeInactive">Specifies whether to include inactive jobs</param>
        /// <param name="jobServiceType">Specifies the job service instance type</param>
        /// <param name="jobServiceName">Specifies the job service instance name</param>
        /// <returns>A String representing the job XML</returns>
        public String GetJobsXml(Boolean bIncludeInactive, JobServiceType jobServiceType, String jobServiceName)
        {
            return Riversand.StoredProcedures.JobBase.GetJobsXml(bIncludeInactive, (Int16)jobServiceType, jobServiceName);
        }

		public string GetJobItemXml( int jobId ) 
		{
            return Riversand.StoredProcedures.JobBase.GetJobItemXml(jobId);
			/*
			InternalData dataInternal = new InternalData(); 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			return dataInternal.ExecuteSpGetXML( "usp_N_getJobServiceItem_XML" ); 
			*/
		}

        public string GetJobItemXml(int jobId, string jobType, MDMCenterApplication application)
        {
            JobBL.LegacyJobMethods jobBL = new JobBL.LegacyJobMethods();
            return jobBL.GetJobItem(jobId, jobType,application);
        }

		public int UpdateJobInformation( int jobId, string description, string jobData ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.UpdateJobInformation(jobId, description, jobData, out ret);
			return ret.Value;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.AddInputParameter( "@nvchrDescription", SqlDbType.NVarChar, 200, description ); 
			dataInternal.AddInputParameter( "@ntextJobData", SqlDbType.NText, jobData.Length, jobData ); 
			dataInternal.ExecuteSP( "usp_N_updateJobInformation", ref ret ); 
			return ret; 
			*/
		} 
        
		public bool UpdateJobStatus( int jobId, JobStatus status ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.UpdateJobStatus(jobId, status.ToString(), SqlString.Null, out ret);
			return ret.Value > 0;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.AddInputParameter( "@nvchrStatus", SqlDbType.NVarChar, 50, status.ToString() ); 
			dataInternal.AddInputParameter( "@nvchrUserAction", SqlDbType.NVarChar, 50, "" ); 
			dataInternal.ExecuteSP( "usp_N_updateJobStatus", ref ret ); 
			return ret > 0; 
			*/
		} 
        
		public bool UpdateJobStatus( int jobId, JobStatus status, JobAction userAction ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.UpdateJobStatus(jobId, status.ToString(), userAction.ToString(), out ret);
			return ret.Value > 0;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.AddInputParameter( "@nvchrStatus", SqlDbType.NVarChar, 50, status.ToString() ); 
			dataInternal.AddInputParameter( "@nvchrUserAction", SqlDbType.NVarChar, 50, userAction.ToString() ); 
			dataInternal.ExecuteSP( "usp_N_updateJobStatus", ref ret ); 
			return ret > 0; 
			*/
		} 
        
		public bool UpdateJobUserAction( int jobId, JobAction userAction ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.UpdateJobUserAction(jobId, userAction.ToString(), out ret);
			return ret.Value > 0;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.AddInputParameter( "@nvchrUserAction", SqlDbType.NVarChar, 50, userAction.ToString() ); 
			dataInternal.ExecuteSP( "usp_N_updateJobUserAction", ref ret ); 
			return ret > 0; 
			*/
		} 
        
        
		public bool ResetJobUserAction( int jobId ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.ResetJobUserAction(jobId, out ret);
			return ret.Value > 0;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.ExecuteSP( "usp_N_resetJobUserAction", ref ret ); 
			return ret > 0; 
			*/
		} 
        
		public int AddJob( string type, string subtype, string profileName, string name, string description, string jobData, string username ) 
		{
            JobBL.LegacyJobMethods jobBL = new JobBL.LegacyJobMethods();
           return jobBL.Add(type, subtype, profileName, name, description, jobData, JobStatus.Queued.ToString(), MDMCenterApplication.JobService);
		}
        
		public bool DeleteJob( int jobId ) 
		{ 
			SqlInt32 ret;
            Riversand.StoredProcedures.JobBase.DeleteJob(jobId, out ret);
			return ret.Value > 0;
			/*
			InternalData dataInternal = new InternalData(); 
			int ret = 0; 
			dataInternal.AddInputParameter( "@intId", jobId ); 
			dataInternal.ExecuteSP( "usp_N_deleteJobServiceItem", ref ret ); 
			return ret > 0; 
			*/
		} 
        
        
		public bool AddUCCnetReponseJob( string statusXml ) 
		{ 
            
			XmlDocument xmlDoc = new XmlDocument(); 
			XmlDocument statusXmlDoc = new XmlDocument(); 
			statusXmlDoc.LoadXml( statusXml ); 
			string userName = statusXmlDoc.SelectSingleNode( "/Supplier" ).Attributes[ "Name" ].Value; 
			XmlNodeList CNodeList = null; 
            
			if ( statusXmlDoc.HasChildNodes ) 
			{ 
				CNodeList = statusXmlDoc.SelectNodes( "//CNode" ); 
				// create a job for each response
				foreach ( System.Xml.XmlNode node in CNodeList ) 
				{ 
					// create job xml
					xmlDoc = CreateJobXml( node ); 
					// add new job
					AddJob( "TradingCenter", "xml", "UCCnet CIC", "TradingCenter Job", "UCCnet CIC", xmlDoc.OuterXml, "cfadmin" ); 
                    
					// clean
					xmlDoc = null; 
				}
                
			} 
            
			return true; 
		} 
        
		private XmlDocument CreateJobXml( XmlNode node ) 
		{ 
            
			// <Job>
			// 	<ProcessSteps>
			//         <Step name="AcceptResponse">
			//            <Adapter totalElements="100" msgBuilder="query" msgGroup="aa" orgName="ABC" gtin="" toGLN="" fromGLN="4595266440029" pwd="xriversandx.01" userId="riversanddemand"/>
			//         </Step>
			// 	</ProcessSteps>
			// </Job>
            
			XmlDocument xmlDoc = new XmlDocument(); 
			string status = node.Attributes[ "Status" ].Value; 
			string cnodeId = node.Attributes[ "PK_SCNode" ].Value; 
            
			// GetCoreProperties(cnodeId, gtin, gln, targetMarket)
            
            
			// <Job>
			XmlElement jobNode = xmlDoc.CreateElement( "Job" ); 
			xmlDoc.AppendChild( jobNode ); 
			// 	<ProcessSteps>
			XmlElement processStepsNode = xmlDoc.CreateElement( "ProcessSteps" ); 
			jobNode.AppendChild( processStepsNode ); 
            
			// 	<Step>
			XmlElement StepNode = xmlDoc.CreateElement( "Step" ); 
			processStepsNode.AppendChild( StepNode ); 
			// <Step name="AcceptResponse" totalElements="100" msgBuilder="query" msgGroup="aa" orgName="ABC" gtin="" toGLN="" fromGLN="4595266440029" pwd="xriversandx.01" userId="riversanddemand"/>
            
			switch ( ( status ) ) 
			{
				case "Approved":
					StepNode.SetAttribute( "name", "AcceptResponse" ); 
					break;
				case "Rejected":
					StepNode.SetAttribute( "name", "RejectResponse" ); 
					break;
				case "Deleted":
                    
					break;
			}
            
            
			XmlElement AdapterNode = xmlDoc.CreateElement( "Adapter" ); 
            
			AdapterNode.SetAttribute( "type", "UCCnet" ); 
			AdapterNode.SetAttribute( "nextElement", "0" ); 
			AdapterNode.SetAttribute( "status", "pending" ); 
			AdapterNode.SetAttribute( "msgBuilder", "cic" ); 
			AdapterNode.SetAttribute( "msgGroup", "aa" ); 
			AdapterNode.SetAttribute( "orgName", "ABC" ); 
			AdapterNode.SetAttribute( "gtin", "00000731470103" ); 
			AdapterNode.SetAttribute( "toGLN", "4595266440012" ); 
			AdapterNode.SetAttribute( "fromGLN", "4595266440029" ); 
            
			AdapterNode.SetAttribute( "gtin", "00000731470103" ); 
			AdapterNode.SetAttribute( "toGLN", "4595266440012" ); 
			AdapterNode.SetAttribute( "fromGLN", "4595266440029" ); 
            
			AdapterNode.SetAttribute( "pwd", "xriversandx.01" ); 
			AdapterNode.SetAttribute( "userId", "riversanddemand" ); 
			AdapterNode.SetAttribute( "onError", "NotifySupplierAndAdmin" ); 
			AdapterNode.SetAttribute( "targetMarketCountryCode", "840" ); 
            
            
			StepNode.AppendChild( AdapterNode ); 
            
			// after each step finishs, it updates the context
			XmlElement contextInformationNode = xmlDoc.CreateElement( "ContextInformation" ); 
			jobNode.AppendChild( contextInformationNode ); 
            
			return xmlDoc; 
		} 
        
        /*
		public void GetCoreProperties( ref string cnodeId, ref string gtin, ref string gln, ref string targetMarket ) 
		{ 
			InternalData dataInternal = new InternalData(); 
			System.Data.SqlClient.SqlDataReader rs = null; 
            
			dataInternal.AddInputParameter( "@intCNodeId", SqlDbType.Int, 4, cnodeId ); 
			dataInternal.ExecuteSpGetDataReader( dataInternal.GetSQLConnection(), "usp_N_GetPropByCnodeId" ); 
			if ( rs.Read() ) 
			{ 
				gtin = System.Convert.ToString( rs.GetValue( 0 ) ); 
				gln = System.Convert.ToString( rs.GetValue( 1 ) ); 
				targetMarket = System.Convert.ToString( rs.GetValue( 2 ) ); 
			}
		} 
		*/
	}
}