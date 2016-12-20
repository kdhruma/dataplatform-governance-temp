using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public interface IToken
	{
		/// <summary>
		/// Token Name
		/// </summary>
		String Name { get; set; }

		/// <summary>
		/// Token Type
		/// </summary>
		ExpressionTokenType TokenType { get; set; }
	}
}
