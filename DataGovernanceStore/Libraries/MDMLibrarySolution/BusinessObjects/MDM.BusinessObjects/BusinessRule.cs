using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies a BusinessRule
    /// </summary>
    [DataContract]
    public class BusinessRule : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting BusinessRuleType Id of business rule
        /// </summary>
        private Int32 _businessRuleTypeId = -1;

        /// <summary>
        /// Field denoting short name of BusinessRuleType
        /// </summary>

        private String _businessRuleTypeName = String.Empty;

        /// <summary>
        /// Field denoting RuleXML
        /// </summary>
        private String _ruleXml = String.Empty;

        /// <summary>
        /// Field denoting Description of business rule
        /// </summary>
        private String _description = String.Empty;

        /// <summary>
        /// Field denoting Precondition for business rule
        /// </summary>
        private String _preCondition = String.Empty;

        /// <summary>
        /// Field denoting Post condition for business rule
        /// </summary>
        private String _postCondition = String.Empty;

        /// <summary>
        /// Field denoting XSDSchema for business rule
        /// </summary>
        private String _xsdSchema = String.Empty;

        /// <summary>
        /// Field denoting SampleXML for business rule
        /// </summary>
        private String _sampleXml = String.Empty;

        /// <summary>
        /// Field denoting RuleValue of business rule
        /// </summary>
        private Object _ruleValue = String.Empty;

        /// <summary>
        /// Field denoting if business rule is active
        /// </summary>
        private Boolean _activeFlag = false;

        /// <summary>
        /// Field denoting user who created business rule
        /// </summary>
        private String _createUser = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public BusinessRule()
            :base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule</param>
       public BusinessRule(Int32 id)
           :base(id)
       {
       }

        /// <summary>
        /// Constructor with Id, Name and Description of a BusinessRule as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule</param>
        /// <param name="name">Indicates the ShortName of a BusinessRule </param>
        /// <param name="longName">Indicates the LongName of a BusinessRule </param>
        public BusinessRule(Int32 id, String name, String longName)
            :base(id,name,longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an BusinessRule as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule</param>
        /// <param name="name">Indicates the ShortName of a BusinessRule </param>
        /// <param name="longName">Indicates the LongName of a BusinessRule </param>
        /// <param name="locale">Indicates the Locale of a BusinessRule </param>
        public BusinessRule(Int32 id, String name, String longName, LocaleEnum locale)
            :base(id, name, longName, locale)
        {
        }

        /// <summary>
        /// Constructor to populate business rule object using array
        /// </summary>
        /// <param name="objectArray">Array containing value of business rule</param>
        public BusinessRule(Object[] objectArray)
        {
            int intId = -1;
            int RuleTypeID = -1;

            if (objectArray.Length>0 && objectArray[0] != null)
                Int32.TryParse(objectArray[0].ToString(), out intId);
            this.Id = intId;

            if (objectArray.Length>1 && objectArray[1] != null)
                this.Name = objectArray[1].ToString();

            if (objectArray.Length>2 && objectArray[2] != null)
                this.LongName = objectArray[2].ToString();

            if (objectArray.Length>3 && objectArray[3] != null)
            {
                Int32.TryParse(objectArray[3].ToString(),out RuleTypeID);
                this._businessRuleTypeId = RuleTypeID;
            }

            if( objectArray.Length > 4 && objectArray[4] != null )
                this._ruleXml = objectArray[4].ToString();

            if( objectArray.Length > 5 && objectArray[5] != null )
                this._businessRuleTypeName = objectArray[5].ToString();

            if (objectArray.Length>6 && objectArray[6] != null)
                this._createUser = objectArray[6].ToString();

        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting BusinessRuleType Id of business rule
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleTypeId
        {
            get {return this._businessRuleTypeId;}
            set { this._businessRuleTypeId = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRuleType name of business rule
        /// </summary>
        [DataMember]
        public String BusinessRuleTypeName
        {
            get { return this._businessRuleTypeName; }
            set { this._businessRuleTypeName = value; }
        }

        /// <summary>
        /// Property denoting the RuleXML of business rule
        /// </summary>
        [DataMember]
        public String RuleXML
        {
            get{return this._ruleXml; }
            set { this._ruleXml = value; }
        }

        /// <summary>
        /// Property denoting the Description of business rule
        /// </summary>
        [DataMember]
        public String Description
        {
            get{return this._description; }
            set {this._description=value;}
        }

        /// <summary>
        /// Property denoting the PreCondition of business rule
        /// </summary>
        [DataMember]
        public String PreCondition
        {
            get{return this._preCondition; }
            set { this._preCondition = value; }
        }

        /// <summary>
        /// Property denoting the PostCondition for business rule
        /// </summary>
        [DataMember]
        public String PostCondition
        {
            get{return this._postCondition; }
            set { this._postCondition = value; }
        }

        /// <summary>
        /// Property denoting the XsdSchema of business rule
        /// </summary>
        [DataMember]
        public String XsdSchema
        {
            get {return this._xsdSchema;}
            set {this._xsdSchema = value;}
        }

        /// <summary>
        /// Property denoting the SampleXml of business rule
        /// </summary>
        [DataMember]
        public String SampleXml
        {
            get {return this._sampleXml;}
            set { this._sampleXml = value; }
        }

        /// <summary>
        /// Property denoting the RuleValue of business rule
        /// </summary>
        [DataMember]
        public Object RuleValue
        {
            get{ return this._ruleValue; }
            set { this._ruleValue = value; }
        }

        /// <summary>
        /// Property denoting if business rule is active
        /// </summary>
        [DataMember]
        public Boolean ActiveFlag
        {
            get { return this._activeFlag;}
            set { this._activeFlag = value; }
        }

        /// <summary>
        /// Property denoting the user who created business rule
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get { return this._createUser; }
            set { this._createUser = value; }
        }

        #endregion
        /// <summary>
        /// Get XML representation of BusinessRule object
        /// </summary>
        /// <returns>XML representation of BusinessRule</returns>
        public String ToXML()
        {
            String xml = string.Empty;
            xml = "<Rule ID=\"{0}\" BusinessRuleTypeID=\"{1}\" ShortName=\"{2}\" LongName=\"{3}\" Description=\"\" Precondition=\"\" Postcondition=\"\" ActiveFlag=\"1\"/>";
            //xml = "<ContainerEntityType Id=\"{0}\" ContainerId=\"{1}\" OrgId = \"{2}\" EntityTypeId = \"{3}\" Action=\"{4}\" />    ";

            string retXML = string.Format(xml, this.Id, this.BusinessRuleTypeId, this.Name, this.LongName);

            return retXML;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is BusinessRule)
                {
                    BusinessRule objectToBeCompared = obj as BusinessRule;

                    if (this.BusinessRuleTypeId != objectToBeCompared.BusinessRuleTypeId)
                        return false;

                    if (this.BusinessRuleTypeName != objectToBeCompared.BusinessRuleTypeName)
                        return false;

                    if (this.RuleXML != objectToBeCompared.RuleXML)
                        return false;

                    if (this.Description != objectToBeCompared.Description)
                        return false;

                    if (this.PreCondition != objectToBeCompared.PreCondition)
                        return false;

                    if (this.PostCondition != objectToBeCompared.PostCondition)
                        return false;

                    if (this.XsdSchema != objectToBeCompared.XsdSchema)
                        return false;

                    if (this.SampleXml != objectToBeCompared.SampleXml)
                        return false;

                    if (this.RuleValue != objectToBeCompared.RuleValue)
                        return false;

                    if (this.ActiveFlag != objectToBeCompared.ActiveFlag)
                        return false;

                    if (this.CreateUser != objectToBeCompared.CreateUser)
                        return false;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.BusinessRuleTypeId.GetHashCode() ^ this.BusinessRuleTypeName.GetHashCode() ^ this.RuleXML.GetHashCode() ^ this.Description.GetHashCode() ^ this.PreCondition.GetHashCode() ^ this.PostCondition.GetHashCode() ^ this.XsdSchema.GetHashCode() ^ this.SampleXml.GetHashCode() ^ this.RuleValue.GetHashCode() ^ this.ActiveFlag.GetHashCode() ^ this.CreateUser.GetHashCode();
            return hashCode;
        }

    }
}
