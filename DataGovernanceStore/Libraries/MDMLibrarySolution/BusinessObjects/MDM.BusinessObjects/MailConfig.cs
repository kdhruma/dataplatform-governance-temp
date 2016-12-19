using System;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
	using Core;
	using MDM.Interfaces;

	/// <summary>
	/// Specifies the MailConfig
	/// </summary>
	[DataContract]
	public class MailConfig : MDMObject, IMailConfig
	{
		#region Fields

		/// <summary>
		/// Application name to pass as argument. This is the Enum indicating which application is performing current action 
		/// </summary>
		private MDMCenterApplication _application = MDMCenterApplication.MDMCenter;

		/// <summary>
		/// Module name to pass as argument. This is the Enum indicating which module is performing current action 
		/// </summary>
		private MDMCenterModules _module = MDMCenterModules.Unknown;

		#endregion

		#region Properties

		/// <summary>
		/// Application name to pass as argument. This is the Enum indicating which application is performing current action 
		/// </summary>
		[DataMember]
		public MDMCenterApplication Application
		{
			get
			{
				return _application;
			}
			set
			{
				_application = value;
			}
		}

		/// <summary>
		/// Module name to pass as argument. This is the Enum indicating which module is performing current action 
		/// </summary>
		[DataMember]
		public MDMCenterModules Module
		{
			get
			{
				return _module;
			}
			set
			{
				_module = value;
			}
		}

		/// <summary>
		/// Property denoting the From of MailConfig
		/// </summary>
		[DataMember]
		public string From { get; set; }

		/// <summary>
		/// Property denoting the Host of SMTP Server
		/// </summary>
		[DataMember]
		public string Host { get; set; }

		/// <summary>
		/// Property denoting the Port SMTP Server
		/// </summary>
		[DataMember]
		public int Port { get; set; }

		/// <summary>
		/// Property denoting the SMTP User's password
		/// </summary>
		[DataMember]
		public string Password { get; set; }

		/// <summary>
		/// Property denoting the SMTP SSL Setting
		/// </summary>
		[DataMember]
		public bool EnableSSL { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Parameterless Constructor
		/// </summary>
		public MailConfig()
		{
		}

		/// <summary>
		/// Constructor with Id, Name and Description of a MailConfig as input parameters
		/// </summary>
		/// <param name="id">Indicates the Identity of a MailConfig</param>
		/// <param name="name">Indicates the Name of a MailConfig</param>
		/// <param name="longName">Indicates the Description of a MailConfig</param>
		/// <param name="application"></param>
		/// <param name="module"></param>
		/// <param name="from"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="enableSSL"></param>
		public MailConfig(Int32 id, String name, String longName, MDMCenterApplication application, MDMCenterModules module, String from, String host, Int32 port, String userName, String password, Boolean enableSSL)
			: base(id, name, longName)
		{
			_application = application;
			_module = module;
			From = from;
			Host = host;
			Port = port;
			UserName = userName;
			Password = password;
			EnableSSL = enableSSL;
		}

		/// <summary>
		/// Constructor with object array as input parameter
		/// </summary>
		/// <param name="objectArray">Object array containing value for MailConfig object</param>
		public MailConfig(object[] objectArray)
		{
			Int32 intId = -1;
			if (objectArray[0] != null)
				Int32.TryParse(objectArray[0].ToString(), out intId);

			Id = intId;

			if (objectArray[1] != null)
				Name = objectArray[1].ToString();

			if (objectArray[2] != null)
				LongName = objectArray[2].ToString();

			if (objectArray[3] != null)
				Enum.TryParse(objectArray[3].ToString(), out _application);

			if (objectArray[4] != null)
				Enum.TryParse(objectArray[4].ToString(), out _module);

			if (objectArray[5] != null)
				From = objectArray[5].ToString();

			if (objectArray[6] != null)
				Host = objectArray[6].ToString();

			if (objectArray[7] != null)
				Port = ValueTypeHelper.Int32TryParse(objectArray[7].ToString(), 0);

			if (objectArray[8] != null)
				UserName = objectArray[8].ToString();

			if (objectArray[9] != null)
				Password = objectArray[9].ToString();

			if (objectArray[10] != null)
				EnableSSL = ValueTypeHelper.BooleanTryParse(objectArray[9].ToString(), false);
		}

		/// <summary>
		/// Constructor with XML having values of object. Populate current object using XML
		/// </summary>
		/// <param name="valuesAsXml">XML having xml value</param>
		/// <example>
		///     Sample XML:
		///     <para>
		///            <MailConfig
		///         		PK_MailConfig="101" 
		///					ShortName="VPMailConfig" 
		///					LongName="VPMailConfig"
		///					Application="VPMailConfig"
		///         		Module="VPMailConfig"
		///         		From="james.riversand@gmail.com"
		///         		Host=""
		///         		Port="1"
		///         		UserName=""
		///         		Password="50"
		///         		EnableSSL="" />
		/// </para>
		/// </example>
		public MailConfig(String valuesAsXml)
		{
			LoadMailConfig(valuesAsXml);
		}

		#endregion

		#region Methods

		#region Load Methods

		/// <summary>
		/// Load DBTable object from XML.
		/// </summary>
		/// <param name="valuesAsXml">XML having xml value</param>
		/// <example>
		///     Sample XML:
		///     <para>
		///     <![CDATA[
		///         <MailConfig
		///             PK_MailConfig="101" 
        ///             ShortName="VPMailConfig"  
        ///             LongName="VPMailConfig"
        ///             Application="VPMailConfig"
        ///             Module="VPMailConfig"
        ///             From="james.riversand@gmail.com"
        ///             Host=""
        ///             Port="1"
        ///             UserName=""
        ///             Password="50"
        ///             EnableSSL=""
		///         </MailConfig>
		///     ]]>    
		///     </para>
		/// </example>
		public void LoadMailConfig(String valuesAsXml)
		{
			if (!String.IsNullOrWhiteSpace(valuesAsXml))
			{
				XmlTextReader reader = null;
				try
				{
					reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

					while (!reader.EOF)
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "MailConfig")
						{
							#region Read DBTable Properties

							if (reader.HasAttributes)
							{
								if (reader.MoveToAttribute("PK_MailConfig"))
								{
									Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
								}

								if (reader.MoveToAttribute("ShortName"))
								{
									Name = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("LongName"))
								{
									LongName = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("Application"))
								{
									MDMCenterApplication mdmCenterApplication;
									Enum.TryParse(reader.ReadContentAsString(), out mdmCenterApplication);
									Application = mdmCenterApplication;
								}

								if (reader.MoveToAttribute("Module"))
								{
									MDMCenterModules mdmCenterModules;
									Enum.TryParse(reader.ReadContentAsString(), out mdmCenterModules);
									Module = mdmCenterModules;
								}

								if (reader.MoveToAttribute("From"))
								{
									From = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("Host"))
								{
									Host = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("Port"))
								{
									Port = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
								}

								if (reader.MoveToAttribute("UserName"))
								{
									UserName = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("Password"))
								{
									Password = reader.ReadContentAsString();
								}

								if (reader.MoveToAttribute("EnableSSL"))
								{
									EnableSSL = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
								}
							}

							#endregion
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

		#region ToXml methods

		/// <summary>
		/// Get Xml representation of MailConfig object
		/// </summary>
		/// <returns>Xml representation of object</returns>
		public override String ToXml()
		{
			string xml = String.Format("<MailConfig PK_MailConfig=\"{0}\" ShortName=\"{1}\" LongName=\"{2}\" Application=\"{3}\" Module=\"{4}\" From=\"{5}\" Host=\"{6}\" Port=\"{7}\" UserName=\"{8}\" Password=\"{9}\" EnableSSL=\"{10}\" />", Id, Name, LongName, Application, Module, From, Host, Port, UserName, Password, EnableSSL);
			return xml;
		}

		#endregion

		#endregion
	}
}
