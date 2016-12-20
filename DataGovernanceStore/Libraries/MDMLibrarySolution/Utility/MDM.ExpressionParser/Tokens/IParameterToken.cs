using System;
using System.Linq.Expressions;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public interface IParameterToken : IToken
	{
		/// <summary>
		/// Parameter
		/// </summary>
		ParameterExpression Parameter { get; set; }

		/// <summary>
		/// Flag to indicate Found in Expression
		/// </summary>
		Boolean FoundInExpression { get; set; }

		/// <summary>
		/// Parameter Type
		/// </summary>
		ParameterType ParameterType { get; set; }

		/// <summary>
		/// Data Type
		/// </summary>
		Type DataType { get; set; }
	}
}
