using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    
    /// <summary>
    /// Business object for holding properties for command like connection string, command time out etc.
    /// </summary>
    [DataContract]
    public class DBCommandProperties : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Field denoting connection string
        /// </summary>
        private String _connectionString = String.Empty;

        /// <summary>
        /// Field denoting command timeout for a Sql statement
        /// </summary>
        private Int32 _dataCommandTimeout = 100;

        /// <summary>
        /// Field denoting command text for a Sql statement
        /// </summary>
        private String _commandText = String.Empty;

        /// <summary>
        /// Field denoting type of command. This is same as System.Data.CommandType.
        /// </summary>
        private CommandTypeEnum _commandType = CommandTypeEnum.Text;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting connection string
        /// </summary>
        [DataMember]
        public String ConnectionString
        {
            get
            {
                return this._connectionString ;
            }
            set
            {
                this._connectionString = value;
            }
        }

        /// <summary>
        /// Property denoting command timeout for a Sql statement
        /// </summary>
        [DataMember]
        public Int32 DataCommandTimeout 
        {
            get
            {
                return this._dataCommandTimeout ;
            }
            set
            {
                this._dataCommandTimeout = value;
            }
        }

        /// <summary>
        /// Property denoting command text for a Sql statement
        /// </summary>
        [DataMember]
        public String CommandText 
        {
            get
            {
                return this._commandText;
            }
            set
            {
                this._commandText = value;
            }
        }

        /// <summary>
        /// Property denoting type of command. This is same as System.Data.CommandType.
        /// </summary>
        [DataMember]
        public CommandTypeEnum CommandType 
        {
            get
            {
                return this._commandType;
            }
            set
            {
                this._commandType = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBCommandProperties()
        {
        }

        /// <summary>
        /// Initialize all properties through this constructor
        /// </summary>
        /// <param name="connectionString">Connection string for command</param>
        /// <param name="commandText">Text for command</param>
        /// <param name="dataCommandTimeout">Sql time out for this command</param>
        /// <param name="commandType">Type of command</param>
        public DBCommandProperties( String connectionString, String commandText, Int32 dataCommandTimeout, CommandTypeEnum commandType )
        {
            this._connectionString = connectionString;
            this._commandText = commandText;
            this._dataCommandTimeout = dataCommandTimeout;
            this._commandType = commandType;
        }

        /// <summary>
        /// Initialize command from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for Command object
        /// <para>Sample Xml</para>
        /// <![CDATA[
        /// <Command ConnectionString="" DataCommandTimeOut="-1" CommandText="1" CommandType="Text"/>
        /// ]]>
        /// </param>
        public DBCommandProperties( String valuesAsXml )
        {
            this.LoadCommand(valuesAsXml);
        }
        
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Initialize command from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for Command object
        /// <para>Sample Xml</para>
        /// <![CDATA[
        /// <Command ConnectionString="" DataCommandTimeOut="-1" CommandText="1" CommandType="Text"/>
        /// ]]>
        /// </param>
        public void LoadCommand( string valuesAsXml )
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while ( !reader.EOF )
                {
                    if ( reader.NodeType == XmlNodeType.Element && reader.Name == "Command" )
                    {
                        //Read command information
                        #region Read command information

                        if ( reader.HasAttributes )
                        {
                            if ( reader.MoveToAttribute("ConnectionString") )
                            {
                                this.ConnectionString = reader.ReadContentAsString();
                            }

                            if ( reader.MoveToAttribute("DataCommandTimeOut") )
                            {
                                Int32 commandTimeout = 100;
                                Int32.TryParse(reader.ReadContentAsString(), out commandTimeout);
                                this.DataCommandTimeout = commandTimeout;
                            }

                            if ( reader.MoveToAttribute("CommandText") )
                            {
                                this.CommandText = reader.ReadContentAsString();
                            }

                            if ( reader.MoveToAttribute("CommandType") )
                            {
                                CommandTypeEnum type = Core.CommandTypeEnum.Text;
                                Enum.TryParse(reader.ReadContentAsString(), out type);
                                this.CommandType = type;
                            }
                        }
                        #endregion Read command information
                    }
                    else
                    {
                        reader.Read();
                    }
                }
            }
            finally
            {
                if ( reader != null )
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Get XML representation of Command object
        /// </summary>
        /// <returns>XML representation of Command</returns>
        public String ToXml()
        {
            String commandXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Command node start
            xmlWriter.WriteStartElement("Command");

            xmlWriter.WriteAttributeString("ConnectionString", this.ConnectionString);
            xmlWriter.WriteAttributeString("DataCommandTimeout", this.DataCommandTimeout.ToString());
            xmlWriter.WriteAttributeString("CommandText", this.CommandText);
            xmlWriter.WriteAttributeString("CommandType", this.CommandType.ToString());

            
            //Value node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            commandXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return commandXml;
        }

        #endregion Methods
    }
}
