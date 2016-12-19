using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MDM.ExpressionParser.Initializers
{
	using MDM.Core;
	using MDM.ExpressionParser.Tokens;

	public class DefaultExpressionInitializer : IExpressionInitializer
	{
		#region Properties

		/// <summary>
		/// List of Operators
		/// </summary>
		public IDictionary<String, IOperatorToken> Operators { get; set; }

		/// <summary>
		/// List of Tokens
		/// </summary>
		public Stack<IToken> Tokens { get; set; }

		/// <summary>
		/// List of Breakers
		/// </summary>
		public IList<Char> Breakers { get; set; }

		/// <summary>
		/// List of Grouping Tokens
		/// </summary>
		public IList<IToken> GroupingTokens { get; set; }

		/// <summary>
		/// List of Parameters
		/// </summary>
		public IDictionary<String, IParameterToken> Parameters { get; set; }

		#endregion Properties

		#region Constructors

		public DefaultExpressionInitializer()
		{
			InitializeOperators();
			InitializeBreakers();
			InitializeParameters();
			InitializeGroupingTokens();
		}

		#endregion Constructors

		#region Methods

		#region Public Methods

		/// <summary>
		/// Add a Parameter to the List of Parameters
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="parameterType"></param>
		public void AddParameter(String name, Type type, ParameterType parameterType)
		{
			Parameters.Add(name, new ParameterToken(name, Expression.Variable(type, name), parameterType));
		}

		#endregion Public Methods

		#region Private Methods

		private void InitializeOperators()
		{
			Operators = new Dictionary<String, IOperatorToken>(StringComparer.InvariantCultureIgnoreCase)
			{
				{ "IN", new OperatorToken<Func<Expression, MethodInfo, IEnumerable<Expression>, MethodCallExpression>>("Contains", "Contains", 12, OperatorBinding.Left, ExpressionType.Call, Expression.Call, typeof(List<String>), OperatorExpressionType.InstanceMethodCall) }
				, { "NOT", new OperatorToken<Func<Expression, UnaryExpression>>("!", "!", 11, OperatorBinding.Left, ExpressionType.Not, Expression.Not, OperatorExpressionType.Unary) }
				, { "MUL", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("*", "*", 10, OperatorBinding.Left, ExpressionType.Multiply, Expression.Multiply) }
				, { "DIV", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("/", "/", 10, OperatorBinding.Left, ExpressionType.Divide, Expression.Divide) }
				, { "SUM", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("+", "+", 9, OperatorBinding.Left, ExpressionType.Add, Expression.Add) }
				, { "MINUS", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("-", "-", 9, OperatorBinding.Left, ExpressionType.Subtract, Expression.Subtract) }
				, { "LT", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("<", "<", 7, OperatorBinding.Left, ExpressionType.LessThan, Expression.LessThan) }
				, { "GT", new OperatorToken<Func<Expression, Expression, BinaryExpression>>(">", ">", 7, OperatorBinding.Left, ExpressionType.GreaterThan, Expression.GreaterThan) }
				, { "LTEQ", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("<=", "<=", 7, OperatorBinding.Left, ExpressionType.LessThanOrEqual, Expression.LessThanOrEqual) }
				, { "GTEQ", new OperatorToken<Func<Expression, Expression, BinaryExpression>>(">=", ">=", 7, OperatorBinding.Left, ExpressionType.GreaterThanOrEqual, Expression.GreaterThanOrEqual) }
				, { "EQ", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("==", "==", 6, OperatorBinding.Left, ExpressionType.Equal, Expression.Equal) }
				, { "NOTEQ", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("!=", "!=", 6, OperatorBinding.Left, ExpressionType.NotEqual, Expression.NotEqual) }
				, { "AND", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("&&", "&&", 2, OperatorBinding.Left, ExpressionType.AndAlso, Expression.AndAlso) }
				, { "OR", new OperatorToken<Func<Expression, Expression, BinaryExpression>>("||", "||", 1, OperatorBinding.Left, ExpressionType.OrElse, Expression.OrElse) }
			};
		}

		private void InitializeBreakers()
		{
			Breakers = new List<Char> { ' ', '(', ')' };
		}

		private void InitializeGroupingTokens()
		{
            GroupingTokens = new List<IToken> { new Token("##", ExpressionTokenType.Parameter),new Token("\"", ExpressionTokenType.Value), new Token("#", ExpressionTokenType.Value) };
		}

		private void InitializeParameters()
		{
			Parameters = new Dictionary<String, IParameterToken>(StringComparer.InvariantCultureIgnoreCase);
			AddParameter("##EntityId##", typeof(Int64), ParameterType.Variable);
			AddParameter("##EntityName##", typeof(String), ParameterType.Variable);
			AddParameter("##CategoryName##", typeof(String), ParameterType.Variable);
			AddParameter("##CategoryPath##", typeof(String), ParameterType.Variable);
			AddParameter("##EntityTypeId##", typeof(Int32), ParameterType.Variable);
			AddParameter("##EntityTypeName##", typeof(String), ParameterType.Variable);
			AddParameter("##EntityTypeLongName##", typeof(String), ParameterType.Variable);
			AddParameter("##ContainerId##", typeof(Int32), ParameterType.Variable);
			AddParameter("##ContainerName##", typeof(String), ParameterType.Variable);
			AddParameter("##ContainerLongName##", typeof(String), ParameterType.Variable);
			AddParameter("##OrganizationId##", typeof(Int32), ParameterType.Variable);
			AddParameter("##OrganizationName##", typeof(String), ParameterType.Variable);
			AddParameter("##OrganizationLongName##", typeof(String), ParameterType.Variable);
			AddParameter("##AttributeName|Locale##", typeof(String), ParameterType.Variable);
		}

		#endregion Private Methods

		#endregion Methods
	}
}
