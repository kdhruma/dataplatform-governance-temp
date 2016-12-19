using System;
using System.Data.SqlTypes;

namespace Riversand.StoredProcedures
{
	/// <summary>
	/// Summary description for Convert.
	/// </summary>
	public class DataConvert
	{
		private DataConvert()
		{
		}

		public static SqlInt32 Int(string data)
		{
			return (data == null) ? SqlInt32.Null : new SqlInt32(Int32.Parse(data));
		}

		public static SqlInt32 Int(int data)
		{
			return new SqlInt32(data);
		}

		public static SqlString String(string data)
		{
			return (data == null) ? SqlString.Null : new SqlString(data);
		}

		public static SqlBoolean Bool(string data)
		{
			return (data == null) ? SqlBoolean.Null : new SqlBoolean(data.ToLower() == "true");
		}
	}
}
