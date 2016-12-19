using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MDM.ExpressionParser
{
	using Tokens;

	public interface IExpressionProcessor
	{
		IDictionary<String, IParameterToken> Parameters { get; }

		IList<ParameterExpression> Variables { get; }
	}
}
