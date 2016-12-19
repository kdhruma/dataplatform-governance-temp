using System;

namespace MDM.ExpressionParser.Tokens
{
	using MDM.Core;

	public class Token : IToken
	{
		#region Properties

		/// <summary>
		/// Token Name
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// Token Type
		/// </summary>
		public ExpressionTokenType TokenType { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		public Token(String name, ExpressionTokenType type)
		{
			Name = name;
			TokenType = type;
		}

		#endregion
	}
}
