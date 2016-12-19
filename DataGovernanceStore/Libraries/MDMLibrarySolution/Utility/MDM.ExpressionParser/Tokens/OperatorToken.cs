using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public class OperatorToken<T> : Token, IOperatorToken
	{
		#region Fields

		/// <summary>
		/// Property storing the c# operator
		/// </summary>
		public String CSOperator { get; set; }

		/// <summary>
		/// Property storing the c# operator symbol
		/// </summary>
		public String OperatorSymbol { get; set; }

		/// <summary>
		/// Property storing the precedence
		/// </summary>
		public Int32 Precedence { get; set; }

		/// <summary>
		/// Property storing the Linq expression type
		/// </summary>
		public ExpressionType LinqExpressionType { get; set; }

		/// <summary>
		/// Property storing the function definition
		/// </summary>
		public T FunctionDefiniton { get; set; }

		/// <summary>
		/// Property storing the Linq operator type
		/// </summary>
		public ExpressionType LinqOperatorType { get; set; }

		/// <summary>
		/// Property storing the operator binding
		/// </summary>
		public OperatorBinding OperatorBinding { get; set; }

		/// <summary>
		/// Property storing the operator expression type
		/// </summary>
		public OperatorExpressionType OperatorExpressionType { get; set; }

		/// <summary>
		/// Property indicating the first argument type
		/// </summary>
		public Type FirstArgumentType { get; set; }

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">token name</param>
		/// <param name="operatorSymbol">operator symbol</param>
		/// <param name="precedence">precedence</param>
		/// <param name="operatorBinding">operator binding</param>
		/// <param name="linqExpressionType">linq expression</param>
		/// <param name="functionDefinition">function definition</param>
		/// <param name="operatorExpressionType">operator expression type</param>
		public OperatorToken(String name, String operatorSymbol, Int32 precedence, OperatorBinding operatorBinding, ExpressionType linqExpressionType, T functionDefinition, OperatorExpressionType operatorExpressionType = OperatorExpressionType.Binary) : base(name, ExpressionTokenType.Operator)
		{
			OperatorSymbol = operatorSymbol;
			LinqExpressionType = linqExpressionType;
			FunctionDefiniton = functionDefinition;
			Precedence = precedence;
			OperatorBinding = operatorBinding;
			OperatorExpressionType = operatorExpressionType;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">token name</param>
		/// <param name="operatorSymbol">operator symbol</param>
		/// <param name="precedence">precedence</param>
		/// <param name="operatorBinding">operator binding</param>
		/// <param name="linqExpressionType">linq expression</param>
		/// <param name="functionDefinition">function definition</param>
		/// <param name="firstArgumentType">first argument type</param>
		/// <param name="operatorExpressionType">operator expression type</param>
		public OperatorToken(String name, String operatorSymbol, Int32 precedence, OperatorBinding operatorBinding, ExpressionType linqExpressionType, T functionDefinition, Type firstArgumentType, OperatorExpressionType operatorExpressionType = OperatorExpressionType.Binary)
			: this(name, operatorSymbol, precedence, operatorBinding, linqExpressionType, functionDefinition, operatorExpressionType)
		{
			FirstArgumentType = firstArgumentType;
		}

		#endregion Constructors
	}
}
