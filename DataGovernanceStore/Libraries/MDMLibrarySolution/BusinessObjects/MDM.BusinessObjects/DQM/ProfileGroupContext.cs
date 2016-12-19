using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.DQM
{
    using System.IO;
    using System.Xml;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ProfileGroupContext : IProfileGroupContext
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Int32 _id = 0;

        /// <summary>
        /// 
        /// </summary>
        private ApplicationContext _applicationContext = new ApplicationContext();

        /// <summary>
        /// 
        /// </summary>
        private Int32 _profileId = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ProfileGroupContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="conatinerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="profileId"></param>
        public ProfileGroupContext(Int32 organizationId, Int32 conatinerId, Int64 categoryId, Int32 entityTypeId, Int32 profileId)
        {
            this.ApplicationContext.OrganizationId = organizationId;
            this.ApplicationContext.ContainerId = conatinerId;
            this.ApplicationContext.CategoryId = categoryId;
            this.ApplicationContext.EntityTypeId = entityTypeId;
            this.ProfileId = profileId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public ProfileGroupContext(String valuesAsXml)
        {
            LoadProfileGroupContext(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public ProfileGroupContext(ProfileGroupContext source)
        {
            ApplicationContext.OrganizationId = source.ApplicationContext.OrganizationId;
            ApplicationContext.ContainerId = source.ApplicationContext.ContainerId;
            ApplicationContext.CategoryId = source.ApplicationContext.CategoryId;
            ApplicationContext.EntityTypeId = source.ApplicationContext.EntityTypeId;
            ProfileId = source.ProfileId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ApplicationContext ApplicationContext
        {
            get { return this._applicationContext; }
            set { this._applicationContext = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 ProfileId
        {
            get { return this._profileId; }
            set { this._profileId = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ProfileGroupContext)
            {
                ProfileGroupContext objectToBeCompared = obj as ProfileGroupContext;

                if (this.ProfileId != objectToBeCompared.ProfileId)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = this.ProfileId.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Clone Matching profile group
        /// </summary>
        /// <returns>Cloned Matching profile object group</returns>
        public object Clone()
        {
            ProfileGroupContext clonedProfileGroupContext = new ProfileGroupContext(this);
            return clonedProfileGroupContext;
        }

        /// <summary>
        /// Get Xml representation of ProfileGroupContext object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            String profileGroupContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("Map");

            if (this.ApplicationContext != null)
            {
                xmlWriter.WriteAttributeString("AppContextId", this.ApplicationContext.Id.ToString());
            }
            else
            {
                xmlWriter.WriteAttributeString("AppContextId", "0");
            }

            xmlWriter.WriteAttributeString("ProfileId", this.ProfileId.ToString());

            //Parameter node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            profileGroupContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return profileGroupContextXml;
        }

        /// <summary>
        /// Compare ProfileGroupContext with current ProfileGroupContext.
        /// This method will compare ProfileGroupContext.
        /// </summary>
        /// <param name="subSetProfileGroupContext">ProfileGroupContext to be compared with current ProfileGroupContext</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(ProfileGroupContext subSetProfileGroupContext, Boolean compareIds = false)
        {
            if (subSetProfileGroupContext != null)
            {
                if (compareIds)
                {
                    if (this.ProfileId != subSetProfileGroupContext.ProfileId)
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadProfileGroupContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Map")
                        {
                            #region Read Context Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AppContextId"))
                                {
                                    this.ApplicationContext.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ApplicationContext.Id);
                                }
                                if (reader.MoveToAttribute("ProfileId"))
                                {
                                    this.ProfileId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ProfileId);
                                }
                            }

                            #endregion Read Context Properties
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion
    }
}
