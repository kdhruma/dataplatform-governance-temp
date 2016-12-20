using System;
using System.Linq.Expressions;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public class ParameterToken : Token, IParameterToken
	{
		#region Fields

		/// <summary>
		/// Parameter
		/// </summary>
		public ParameterExpression Parameter { get; set; }

		/// <summary>
		/// Found In Expression
		/// </summary>
		public Boolean FoundInExpression { get; set; }

		/// <summary>
		/// Parameter Type
		/// </summary>
		public ParameterType ParameterType { get; set; }

		/// <summary>
		/// Data Type
		/// </summary>
		public Type DataType { get; set; }

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parameter"></param>
		/// <param name="paramType"></param>
		public ParameterToken(String name, ParameterExpression parameter, ParameterType paramType)
			: base(name, ExpressionTokenType.Parameter)
		{
			Parameter = parameter;
			ParameterType = paramType;
		}

		#endregion Constructors
	}
}
