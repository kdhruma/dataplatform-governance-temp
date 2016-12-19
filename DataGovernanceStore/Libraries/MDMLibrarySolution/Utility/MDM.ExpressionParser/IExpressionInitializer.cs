using System;
using System.Collections.Generic;

namespace MDM.ExpressionParser
{
	using MDM.Core;
	using MDM.ExpressionParser.Tokens;

	interface IExpressionInitializer
	{
		/// <summary>
		/// Dictionary of Operators
		/// </summary>
		IDictionary<String, IOperatorToken> Operators { get; set; }

		/// <summary>
		/// Dictionary of Parameters
		/// </summary>
		IDictionary<String, IParameterToken> Parameters { get; set; }

		/// <summary>
		/// List of Breakers
		/// </summary>
		IList<Char> Breakers { get; set; }

		/// <summary>
		/// List of Grouping Tokens
		/// </summary>
		IList<IToken> GroupingTokens { get; set; }

		/// <summary>
		/// Interface method to add a new Parameter
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="parameterType"></param>
		void AddParameter(String name, Type type, ParameterType parameterType);
	}
}
