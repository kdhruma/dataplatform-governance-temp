using System.Runtime.Serialization;
using System.Linq;
using System;

namespace MDM.BusinessObjects.Jobs
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.Xml;

    /// <summary>
    /// Represent Collection of JobSchedule Object
    /// </summary>
    [DataContract]
    public class JobScheduleCollection : InterfaceContractCollection<IJobSchedule, JobSchedule>, IJobScheduleCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the JobSchedule Collection
        /// </summary>
        public JobScheduleCollection() { }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Relationship type in xml format</param>
        public JobScheduleCollection(String valuesAsXml)
        {
            LoadJobSchedule(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if JobScheduleCollection contains JobSchedule with given Id
        /// </summary>
        /// <param name="Id">Id using which JobSchedule is to be searched from collection</param>
        /// <returns>
        /// <para>true : If JobSchedule found in JobScheduleCollection</para>
        /// <para>false : If JobSchedule found not in JobScheduleCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            if (GetJobSchedule(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove jobSchedule object from JobScheduleCollection
        /// </summary>
        /// <param name="jobScheduleId">jobScheduleId of jobSchedule which is to be removed from collection</param>
        /// <returns>true if jobSchedule is successfully removed; otherwise, false. This method also returns false if jobSchedule was not found in the original collection</returns>
        public bool Remove(Int32 jobScheduleId)
        {
            IJobSchedule jobSchedule = GetJobSchedule(jobScheduleId);

            if (jobSchedule == null)
                throw new ArgumentException("No JobSchedule found for given Id :" + jobScheduleId);
            else
                return this.Remove(jobSchedule);
        }

        /// <summary>
        /// Get relationship type based on given relationship type id
        /// </summary>
        /// <param name="Id">Id of relationship type to search on</param>
        /// <returns>Relationship type with given id.</returns>
        public IJobSchedule GetJobSchedule(Int32 Id)
        {
            var filteredJobSchedule = from jobSchedule in this._items
                                           where jobSchedule.Id == Id
                                           select jobSchedule;

            if (filteredJobSchedule.Any())
                return filteredJobSchedule.First();
            else
                return null;
        }

        /// <summary>
        /// Get Xml representation of JobScheduleCollection
        /// </summary>
        /// <returns>Xml representation of JobScheduleCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<JobSchedules>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (JobSchedule jobSchedule in this._items)
                {
                    returnXml = String.Concat(returnXml, jobSchedule.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</JobSchedules>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of JobScheduleCollection
        /// </summary>
        /// <returns>Xml representation of JobScheduleCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<JobSchedules>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (JobSchedule jobSchedule in this._items)
                {
                    returnXml = String.Concat(returnXml, jobSchedule.ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</JobSchedules>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is JobScheduleCollection)
            {
                JobScheduleCollection objectToBeCompared = obj as JobScheduleCollection;

                Int32 jobSchedulesUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 jobSchedulesIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (jobSchedulesUnion != jobSchedulesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (JobSchedule JobSchedule in this._items)
            {
                hashCode += JobSchedule.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private void LoadJobSchedule(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <JobSchedules>
                <JobSchedule
                  Id="2" 
                  Name="Kit Code Products" 
                  LongName="Kit Code Products" 
                  ValidationRequired="1" 
                  ShowValidFlag="0 
                  ReadOnly="0" 
                  DrillDown="0" 
                  IsDefault="1" 
              </JobSchedule>
              </JobSchedules>
			 */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "JobSchedule")
                        {
                            String scheduleXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(scheduleXml))
                            {
                                JobSchedule jobSchedule = new JobSchedule(scheduleXml);
                                if (jobSchedule != null)
                                {
                                    this.Add(jobSchedule);
                                }
                            }

                        }
                        else
                        {
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
