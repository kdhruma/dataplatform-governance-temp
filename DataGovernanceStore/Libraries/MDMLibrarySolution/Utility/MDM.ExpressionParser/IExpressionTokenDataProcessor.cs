using System.Collections.Generic;
using System.Linq.Expressions;

namespace MDM.ExpressionParser
{
	public interface IExpressionTokenDataProcessor
	{
		/// <summary>
		/// Interface method to return Token Data
		/// </summary>
		/// <returns></returns>
		IDictionary<ParameterExpression, object> GetTokenData();
	}
}
