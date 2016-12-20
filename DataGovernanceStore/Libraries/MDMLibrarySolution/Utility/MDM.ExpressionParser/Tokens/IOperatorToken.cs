using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public interface IOperatorToken : IToken
	{
		/// <summary>
		/// Operator Symbol
		/// </summary>
		String OperatorSymbol { get; set; }

		/// <summary>
		/// Precedence
		/// </summary>
		Int32 Precedence { get; set; }

		/// <summary>
		/// Operator Binding
		/// </summary>
		OperatorBinding OperatorBinding { get; set; }

		/// <summary>
		/// Linq Operator Type
		/// </summary>
		ExpressionType LinqOperatorType { get; set; }

		/// <summary>
		/// Operator Expression Type
		/// </summary>
		OperatorExpressionType OperatorExpressionType { get; set; }

		/// <summary>
		/// First Argument Type
		/// </summary>
		Type FirstArgumentType { get; set; }
	}
}
