using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MDM.ExpressionParser.Data
{
	using MDM.BusinessObjects;
	using MDM.BusinessObjects.Diagnostics;
	using MDM.Core;
	using MDM.Interfaces;
	using MDM.LookupManager.Business;
	using MDM.Utility;

	public class DefaultExpressionTokenDataProcessor : IExpressionTokenDataProcessor
	{
		#region Fields

		private readonly TraceSettings _traceSettings;

		private DiagnosticActivity _diagnosticActivity;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="processor"></param>
		/// <param name="entityData"></param>
		public DefaultExpressionTokenDataProcessor(IExpressionProcessor processor, Entity entityData)
		{
			EntityData = entityData;
			Processor = processor;

			_traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
			_diagnosticActivity = new DiagnosticActivity();
		}

		#endregion Constructors

		#region Properties

		private Entity EntityData { get; set; }

		private IExpressionProcessor Processor { get; set; }

		private Lazy<LookupBL> LookupManager { get { return new Lazy<LookupBL>(() => new LookupBL()); } }

		private Lazy<CallerContext> CallContext { get { return new Lazy<CallerContext>(() => (CallerContext)MDMObjectFactory.GetICallerContext()); } }

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Returns all the Tokens
		/// </summary>
		/// <returns></returns>
		public IDictionary<ParameterExpression, Object> GetTokenData()
		{
			if (_traceSettings.IsBasicTracingEnabled)
			{
				_diagnosticActivity.ActivityName = "MDM.ExpressionParser.Data.DefaultExpressionTokenDataProcessor.GetTokenData";
				_diagnosticActivity.Start();
			}

			var processedParameters = new Dictionary<ParameterExpression, Object>();

			try
			{
				var entityId = EntityData.Id;
				List<ParameterExpression> parameterExpressions = Processor.Variables.ToList();

				foreach (ParameterExpression prm in parameterExpressions)
				{
					switch (prm.Name.ToLowerInvariant())
					{
						case "##entityid##":
							processedParameters.Add(prm, entityId);
							break;
						case "##entityname##":
                            processedParameters.Add(prm, EntityData.Name.ToLowerInvariant());
							break;
						case "##categoryname##":
                            processedParameters.Add(prm, EntityData.CategoryName.ToLowerInvariant());
							break;
						case "##categorypath##":
                            processedParameters.Add(prm, EntityData.CategoryPath.Replace("#@#", "//").ToLowerInvariant());
							break;
						case "##entitytypeid##":
							processedParameters.Add(prm, EntityData.EntityTypeId);
							break;
						case "##entitytypename##":
                            processedParameters.Add(prm, EntityData.EntityTypeName.ToLowerInvariant());
							break;
						case "##entitytypelongname##":
                            processedParameters.Add(prm, EntityData.EntityTypeLongName.ToLowerInvariant());
							break;
						case "##containerid##":
							processedParameters.Add(prm, EntityData.ContainerId);
							break;
						case "##containername##":
                            processedParameters.Add(prm, EntityData.ContainerName.ToLowerInvariant());
							break;
						case "##containerlongname##":
                            processedParameters.Add(prm, EntityData.ContainerLongName.ToLowerInvariant());
							break;
						case "##organizationid##":
							processedParameters.Add(prm, EntityData.OrganizationId);
							break;
						case "##organizationname##":
                            processedParameters.Add(prm, EntityData.OrganizationName.ToLowerInvariant());
							break;
						case "##organizationlongname##":
                            processedParameters.Add(prm, EntityData.OrganizationLongName.ToLowerInvariant());
							break;
						default: //Process Attributes By Attribute Name|Locale OR AttributeId|Locale 
							Object value = "null";

							var attrname = prm.Name.Trim("##".ToCharArray());
							var attokens = attrname.Split('|');
							IAttribute attr;
							Int32 attributeId;
							var isAttributeId = Int32.TryParse(attokens[0], out attributeId);

							if (attokens.Length == 1) //No Locale use system locale
							{
								if (isAttributeId)
								{
									attr = EntityData.GetAttribute(attributeId);
								}
								else
								{
									attr = EntityData.GetAttribute(attokens[0]);
								}
							}
							else
							{
								LocaleEnum locale;
								var success = Enum.TryParse(attokens[1], out locale);

								if (success)
								{
									if (isAttributeId)
									{
										attr = EntityData.GetAttribute(attributeId, locale);
									}
									else
									{
										attr = EntityData.GetAttribute(attokens[0], locale);
									}
								}
								else
								{
									if (_traceSettings.IsBasicTracingEnabled)
									{
										_diagnosticActivity.LogError(String.Format("DefaultExpressionTokenDataProcessor:GetTokenData. Locale for the attribute token {0} for MDM Object {1}.", prm, entityId));
									}

									if (isAttributeId)
									{
										attr = EntityData.GetAttribute(attributeId);
									}
									else
									{
										attr = EntityData.GetAttribute(attokens[0]);
									}
								}
							}

							if (attr != null)
							{
								if (!attr.IsComplex)
								{
									if (attr.HasValue())
									{
										value = GetAttributeValue(attr);
									}
									else
									{
										if (_traceSettings.IsBasicTracingEnabled)
										{
											_diagnosticActivity.LogInformation(String.Format("DefaultExpressionTokenDataProcessor:GetTokenData. MDMObject {0} Attribute {1} does not have value defined.", entityId, attr.Name));
										}
									}
								}
								else
								{
									if (_traceSettings.IsBasicTracingEnabled)
									{
										_diagnosticActivity.LogError(String.Format("DefaultExpressionTokenDataProcessor:GetTokenData. MDMObject {0} Attribute {1} is a complex attribute. Complex attributes in Conditional expressions are not currently supported.", entityId, attr.Name));
									}
								}
							}
							else
							{
								if (_traceSettings.IsBasicTracingEnabled)
								{
									_diagnosticActivity.LogError(String.Format("DefaultExpressionTokenDataProcessor:GetTokenData. MDMObject {0} does not have Attribute {1}.", entityId, attokens[0]));
								}
							}

							processedParameters.Add(prm, value);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				_diagnosticActivity.LogError(String.Format("Error occurred : {0}", ex.Message));
				throw;
			}
			finally
			{
				if (_traceSettings.IsBasicTracingEnabled)
				{
					_diagnosticActivity.Stop();
				}
			}

			return processedParameters;
		}

		#endregion Public Methods

		#region Private Methods

		private Object GetAttributeValue(IAttribute attr)
		{
			Lookup lookup = null;
			if (attr.IsLookup)
			{
				lookup = LookupManager.Value.Get(attr.Id, attr.Locale, -1, CallContext.Value);
			}

			if (!attr.IsCollection)
			{
				if (attr.IsLookup)
				{
					return GetConvertedAttributeValue(attr.AttributeDataType, GetLookupDisplayValue(lookup, attr.GetCurrentValue().ToString()));
				}
				else
				{
					return GetConvertedAttributeValue(attr.AttributeDataType, attr.GetCurrentValue().ToString());
				}
			}
			else
			{
				if (attr.IsLookup)
				{
					return attr.GetCurrentValues().Select(v => GetConvertedAttributeValue(attr.AttributeDataType, GetLookupDisplayValue(lookup, v.AttrVal.ToString()))).ToList();
				}
				else
				{
					return attr.GetCurrentValues().Select(v => GetConvertedAttributeValue(attr.AttributeDataType, v.ToString())).ToList();
				}
			}
		}

		private String GetLookupDisplayValue(Lookup lookup, String value)
		{
			var displayValue = String.Empty;
			Int32 lookupId;
			var parsed = Int32.TryParse(value, out lookupId);

			if (!parsed)
			{
				return displayValue.ToLowerInvariant();
			}

			displayValue = lookup.GetDisplayFormatById(lookupId);
			return displayValue.ToLowerInvariant();
		}

		private Object GetConvertedAttributeValue(AttributeDataType dataType, String value)
		{
			Object convertedValue = new Object();

			switch (dataType)
			{
				case AttributeDataType.Decimal:
					Decimal dout;
					var dSuccess = Decimal.TryParse(value, out dout);
					if (dSuccess)
					{
						convertedValue = dout;
					}
					break;
				case AttributeDataType.String:
					convertedValue = value.ToLowerInvariant();
					break;
				case AttributeDataType.Date:
				case AttributeDataType.DateTime:
					DateTime dtout;
					var dtSuccess = DateTime.TryParse(value, out dtout);
					if (dtSuccess)
					{
						convertedValue = dtout;
					}
					break;
				case AttributeDataType.Boolean:
					Boolean bout;
					var bSuccess = Boolean.TryParse(value, out bout);
					if (bSuccess)
					{
						convertedValue = bout;
					}
					break;
				case AttributeDataType.Integer:
					Int32 iout;
					var iSuccess = Int32.TryParse(value, out iout);
					if (iSuccess)
					{
						convertedValue = iout;
					}
					break;
			}

			return convertedValue;
		}

		#endregion Private Methods

		#endregion Methods
	}
}
