using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
	using MDM.Interfaces;

	/// <summary>
	/// Specifies the MailConfig Instance Collection for the Object
	/// </summary>
	[DataContract]
	public class MailConfigCollection : ICollection<MailConfig>, IMailConfigCollection
	{
		#region Fields

		[DataMember]
		private Collection<MailConfig> _mailConfigs = new Collection<MailConfig>();

		#endregion

		#region Constructors

		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public MailConfigCollection()
		{ }

		/// <summary>
		/// Constructor which takes Xml as input
		/// </summary>
		public MailConfigCollection(String valueAsXml)
		{
			LoadMailConfigCollection(valueAsXml);
		}

		/// <summary>
		/// Initialize MailConfigCollection from IList
		/// </summary>
		/// <param name="mailConfigList">IList of MailConfigs</param>
		public MailConfigCollection(IList<MailConfig> mailConfigList)
		{
			_mailConfigs = new Collection<MailConfig>(mailConfigList);
		}

		#endregion

		#region Properties

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines whether two Object instances are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Object.</param>
		/// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is MailConfigCollection)
			{
				MailConfigCollection objectToBeCompared = obj as MailConfigCollection;
				Int32 mailConfigUnion = _mailConfigs.ToList().Union(objectToBeCompared.ToList()).Count();
				Int32 mailConfigIntersect = _mailConfigs.ToList().Intersect(objectToBeCompared.ToList()).Count();
				if (mailConfigUnion != mailConfigIntersect)
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
			foreach (MailConfig mailConfig in _mailConfigs)
			{
				hashCode += mailConfig.GetHashCode();
			}
			return hashCode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="valuesAsXml"></param>
		public void LoadMailConfigCollection(String valuesAsXml)
		{
			#region Sample Xml
			/*
			 * <MailConfigs></MailConfigs>
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
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "MailConfig")
						{
							String mailConfigXml = reader.ReadOuterXml();
							if (!String.IsNullOrEmpty(mailConfigXml))
							{
								MailConfig mailConfig = new MailConfig(mailConfigXml);
								Add(mailConfig);
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

		#region Private Methods

		#endregion

		#region ICollection<MailConfig> Members

		/// <summary>
		/// Add MailConfig object in collection
		/// </summary>
		/// <param name="item">MailConfig to add in collection</param>
		public void Add(MailConfig item)
		{
			_mailConfigs.Add(item);
		}

		/// <summary>
		/// Removes all MailConfigs from collection
		/// </summary>
		public void Clear()
		{
			_mailConfigs.Clear();
		}

		/// <summary>
		/// Determines whether the MailConfigCollection contains a specific MailConfig.
		/// </summary>
		/// <param name="item">The MailConfig object to locate in the MailConfigCollection.</param>
		/// <returns>
		/// <para>true : If MailConfig found in mappingCollection</para>
		/// <para>false : If MailConfig found not in mappingCollection</para>
		/// </returns>
		public bool Contains(MailConfig item)
		{
			return _mailConfigs.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the MailConfigCollection to an
		///  System.Array, starting at a particular System.Array index.
		/// </summary>
		/// <param name="array"> 
		///  The one-dimensional System.Array that is the destination of the elements
		///  copied from MailConfigCollection. The System.Array must have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public void CopyTo(MailConfig[] array, int arrayIndex)
		{
			_mailConfigs.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Get the count of no. of MailConfigs in MailConfigCollection
		/// </summary>
		public int Count
		{
			get
			{
				return _mailConfigs.Count;
			}
		}

		/// <summary>
		/// Check if MailConfigCollection is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the MailConfigCollection.
		/// </summary>
		/// <param name="item">The MailConfig object to remove from the MailConfigCollection.</param>
		/// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original MailConfigCollection</returns>
		public bool Remove(MailConfig item)
		{
			return _mailConfigs.Remove(item);
		}

		#endregion

		#region IEnumerable<MailConfig> Members

		/// <summary>
		/// Returns an enumerator that iterates through a MailConfigCollection.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
		public IEnumerator<MailConfig> GetEnumerator()
		{
			return _mailConfigs.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a MailConfigCollection.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _mailConfigs.GetEnumerator();
		}

		#endregion

		#region IMailConfigCollection Members

		#region ToXml methods

		/// <summary>
		/// Get Xml representation of MailConfigColleciton object
		/// </summary>
		/// <returns>Xml string representing the MailConfigCollection</returns>
		public String ToXml()
		{
			StringBuilder builder = new StringBuilder();

			foreach (MailConfig mailConfig in _mailConfigs)
			{
				builder.Append(mailConfig.ToXml());
			}

			return String.Format("<MailConfigs>{0}</MailConfigs>", builder);
		}

		#endregion ToXml methods

		#endregion IMailConfigCollection Memebers
	}
}
