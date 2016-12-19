using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Reflection;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Workflow
    /// </summary>
    /// 
    [DataContract]
    public class Workflow : MDMObject, IWorkflow
    {
        #region Fields

        /// <summary>
        /// Indicates the workflow version
        /// </summary>
        private Collection<WorkflowVersion> _workflowVersions = new Collection<WorkflowVersion>();

        /// <summary>
        /// Indicates which version is the latest for the workflow.
        /// </summary>
        private Int32 _latestVersion = 0;

        /// <summary>
        /// Indicates the type of the workflow
        /// </summary>
        private String _workflowType = "WWF";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Workflow()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an AppConfig Instance</param>
        public Workflow(Int32 id)
            : base(id)
        {

        }

        /// <summary>
        /// Constructor with Object array. 
        /// </summary>
        /// <param name="objectArray">Object array containing values for Workflow. </param>
        public Workflow(object[] objectArray)
        {
            if (objectArray != null && objectArray.Count() > 0)
            {
                int intId = -1;
                if (objectArray[0] != null)
                    Int32.TryParse(objectArray[0].ToString(), out intId);

                this.Id = intId;

                if (objectArray[1] != null)
                    this.Name = objectArray[1].ToString();

                if (objectArray[2] != null)
                    this.LongName = objectArray[2].ToString();
            }
        }

        /// <summary>
        /// Constructor with workflow details in the form of xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the workflow details in xml format</param>
        public Workflow(String valuesAsXml)
        {
            /*
             * Sample:
             * <Workflow Id="13" ShortName="Workflow1" LongName="Workflow1" VersionId="0" />
             */
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (reader.Read())
                    {
                        if (reader.Name == "Workflow")
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = reader.ReadContentAsInt();
                                    }
                                    if (reader.MoveToAttribute("ShortName"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }
                                    if (reader.MoveToAttribute("LongName"))
                                    {
                                        this.LongName = reader.ReadContentAsString();
                                    }
                                    if (reader.MoveToAttribute("VersionId"))
                                    {
                                        this.LatestVersion = reader.ReadContentAsInt();
                                    }
                                    if (reader.MoveToAttribute("WorkflowType"))
                                    {
                                        this.WorkflowType = reader.ReadContentAsString().ToUpper();
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        #endregion

        #region Property
        /// <summary>
        /// Property denoting the Workflow versions
        /// </summary>
        [DataMember]
        public Collection<WorkflowVersion> WorkflowVersions
        {
            get
            {
                return this._workflowVersions;
            }
            set
            {
                this._workflowVersions = value;
            }
        }

        /// <summary>
        /// Property denoting which version is the latest for the workflow.
        /// </summary>
        [DataMember]
        public Int32 LatestVersion
        {
            get
            {
                return this._latestVersion;
            }
            set
            {
                this._latestVersion = value;
            }
        }

        /// <summary>
        /// Property denoting the type of the workflow
        /// </summary>
        [DataMember]
        public String WorkflowType
        {
            get
            {
                return this._workflowType;
            }
            set
            {
                this._workflowType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of object
        /// </summary>
        /// <returns>Xml representation of the object</returns>
        public String ToXML()
        {
            String propertyValuesFormat = "Id=\"{0}\" Name=\"{1}\" LongName=\"{2}\" LatestVersionId=\"{3}\" Action=\"{4}\"";

            String propertyValues = String.Format(propertyValuesFormat, this.Id, this.Name, this.LongName, this.LatestVersion, this.Action);

            String workflowVersionsString = String.Empty;

            foreach (WorkflowVersion workflowVersion in this.WorkflowVersions)
            {
                workflowVersionsString = String.Concat(workflowVersionsString, workflowVersion.ToXML());
            }

            String retXML = String.Format("<Workflow {0}><WorkflowVersions>{1}</WorkflowVersions></Workflow>", propertyValues, workflowVersionsString);

            return retXML;
        }

        #endregion

    }
}
