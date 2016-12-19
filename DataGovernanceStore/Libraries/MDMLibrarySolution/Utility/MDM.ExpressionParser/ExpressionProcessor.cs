using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MDM.ExpressionParser
{
	using MDM.Core;
	using MDM.ExpressionParser.Initializers;
	using MDM.ExpressionParser.Tokens;

	public class ExpressionProcessor : IExpressionProcessor
	{
		#region Constructors

		public ExpressionProcessor()
		{
			Tokens = new Stack<IToken>();
			ExpressionInitializer = new DefaultExpressionInitializer();
		}

		#endregion Constructors

		#region Properties

		private IList<Char> Breakers
		{
			get
			{
				return ExpressionInitializer.Breakers;
			}
		}

		private IList<IToken> GroupingTokens
		{
			get
			{
				return ExpressionInitializer.GroupingTokens;
			}
		}

		private Stack<IToken> Tokens { get; set; }

		private IDictionary<String, IOperatorToken> Operators
		{
			get
			{
				return ExpressionInitializer.Operators;
			}
		}

		private IExpressionInitializer ExpressionInitializer { get; set; }

		/// <summary>
		/// List of Parameters
		/// </summary>
		public IDictionary<String, IParameterToken> Parameters
		{
			get
			{
				return ExpressionInitializer.Parameters;
			}
		}

		/// <summary>
		/// Parsed Expression
		/// </summary>
		public Expression ParsedExpression { get; private set; }

		/// <summary>
		/// List of Parameter and Expressions
		/// </summary>
		public IList<ParameterExpression> Variables
		{
			get
			{
				return (from rp in Parameters.Values where rp.FoundInExpression && rp.ParameterType == ParameterType.Variable select rp.Parameter).ToList();
			}
		}

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
		/// Evaluates the expression compiling and applying the values to the parameters
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <param name="tokenProcessor"></param>
		/// <returns></returns>
		public TR Evaluate<TR>(IExpressionTokenDataProcessor tokenProcessor)
		{
			var parameterValues = tokenProcessor.GetTokenData();
			var vAssignmentList = new List<Expression>();
			foreach (var p in parameterValues)
			{
				vAssignmentList.Add(Expression.Assign(p.Key, Expression.Constant(p.Value)));
			}

			//Add the expressionTree
			if (ParsedExpression == null)
			{
				throw new ArgumentNullException("tokenProcessor");
			}

			vAssignmentList.Add(ParsedExpression);

			var parameters = (from rp in Parameters.Values where rp.FoundInExpression select rp.Parameter).ToList();

			var blockExpression = Expression.Block(parameters, vAssignmentList);

			return Expression.Lambda<Func<TR>>(blockExpression).Compile()();
		}

		/// <summary>
		/// Parse the expression
		/// </summary>
		/// <param name="expression"></param>
		public void Parse(StringBuilder expression)
		{
			// Stack for numbers: 'values'
			Stack<Expression> values = new Stack<Expression>();

			var sbuf = new StringBuilder();

			for (Int32 i = 0; i < expression.Length; i++)
			{
				// Current token is a whitespace, skip it
				if (Breakers.All(b => b != expression[i]))
				{
					sbuf.Append(expression[i]);
					if (i < expression.Length - 1)
					{
						continue;
					}
				}

				if (GroupingTokens.Any(g => (g.Name.Length <= sbuf.Length) && g.Name.Equals(sbuf.ToString(0, g.Name.Length)) && !g.Name.Equals(sbuf.ToString(sbuf.Length - g.Name.Length, g.Name.Length))))
				{
					sbuf.Append(expression[i]);
					if (i < expression.Length - 1)
					{
						continue;
					}
				}

				// Current token is a number, push it to stack for numbers
				if ((expression[i] == ' ') && (sbuf.Length > 0))
				{
					var exp = sbuf.ToString();
					if (Operators.ContainsKey(exp))
					{
						Tokens.Push(Operators[exp]);
					}
					else
					{
						ProcessTokens(values, exp);
					}
					sbuf.Clear();
				}
				// Current token is an opening brace, push it to 'ops'
				else if (expression[i] == '(')
				{
					Tokens.Push(new Token("(", ExpressionTokenType.Breaker));
					sbuf.Clear();
				}
				// Closing brace encountered, solve entire brace
				else if (expression[i] == ')' || (i == expression.Length - 1))
				{
					var tokenFound = false;
					if (sbuf.Length > 0)
					{
						ProcessTokens(values, sbuf.ToString());
					}
					while ((Tokens.Any()) && (Tokens.Peek().Name != "("))
					{
						var token = Tokens.Pop() as IOperatorToken;
						ProcessOperators(values, token);
						tokenFound = true;
					}

					if (!tokenFound)
					{
						throw new Exception(String.Format("Token {0} mismatch", expression[i]));
					}

					if (Tokens.Any())
					{
						Tokens.Pop();
					}
					sbuf.Clear();
				}
			}

			if (sbuf.Length > 0)
			{
				values.Push(Expression.Constant(sbuf.ToString()));
			}

			// Entire expression has been parsed at this point, apply remaining
			// ops to remaining values
			while (Tokens.Any())
			{
				var token = Tokens.Pop() as IOperatorToken;
				ProcessOperators(values, token);
			}
			// Top of 'values' contains result, return it
			ParsedExpression = values.Pop();
		}

		#endregion Public Methods

		#region Private Methods

		protected void ProcessOperators(Stack<Expression> values, IOperatorToken token)
		{
			if (token != null)
			{
				Expression operationResult = null;
				Expression firstArgument;
				Expression secondArgument;

				switch (token.OperatorExpressionType)
				{
					case OperatorExpressionType.Unary:
						var uargs = new[] { values.Pop() };
						operationResult = BuildExpression(token, uargs);
						break;
					case OperatorExpressionType.Binary:
						secondArgument = values.Pop();
						firstArgument = values.Pop();

						if (firstArgument.NodeType == ExpressionType.Parameter && secondArgument.NodeType == ExpressionType.Constant)
						{
							var argName = ((ParameterExpression)firstArgument).Name;
							if (Parameters.ContainsKey(argName))
							{
								Parameters[argName].Parameter = Expression.Variable(secondArgument.Type, argName);
								firstArgument = Parameters[argName].Parameter;
							}
						}
						else if (firstArgument.NodeType == ExpressionType.Constant && secondArgument.NodeType == ExpressionType.Parameter)
						{
							var argName = ((ParameterExpression)secondArgument).Name;
							if (Parameters.ContainsKey(argName))
							{
								Parameters[argName].Parameter = Expression.Variable(secondArgument.Type, argName);
								secondArgument = Parameters[argName].Parameter;
							}
						}
						var bargs = new[] { firstArgument, secondArgument };
						operationResult = BuildExpression(token, bargs);
						break;
					case OperatorExpressionType.InstanceMethodCall:
						//The arguments would be reversed as they are on stack
						secondArgument = values.Pop();
						firstArgument = values.Pop();

						//check the argument since at the time of processing we do not have an idea of the type of the first argument.
						if (firstArgument.Type != token.FirstArgumentType)
						{
							var arg = firstArgument as ParameterExpression;
							if (arg != null)
							{
								if (Parameters.ContainsKey(arg.Name))
								{
									Parameters[arg.Name].Parameter = Expression.Variable(token.FirstArgumentType, arg.Name);
									firstArgument = Parameters[arg.Name].Parameter;
								}
							}
						}

						var mcargs = new[] { firstArgument, secondArgument };
						operationResult = BuildExpression(token, mcargs);
						break;
				}

				values.Push(operationResult);
			}
		}

		protected void ProcessTokens(Stack<Expression> values, String value)
		{
			//Check if the value is a parameter other wise process as value
			var processAsValue = true;
			if ((GroupingTokens != null) && GroupingTokens.Any())
			{
				GroupingTokens.Where(gt => gt.TokenType == ExpressionTokenType.Parameter).ToList().ForEach((gt) =>
				{
					if (value.StartsWith(gt.Name) && value.EndsWith(gt.Name))
					{
						if (!Parameters.ContainsKey(value))
						{
							ExpressionInitializer.AddParameter(value, typeof(String), ParameterType.Variable);
						}

						Parameters[value].FoundInExpression = true;
						Parameters[value].ParameterType = (value.StartsWith(gt.Name) && value.EndsWith(gt.Name)) ? ParameterType.Variable : ParameterType.InputArgument;
						values.Push(Parameters[value].Parameter);

						processAsValue = false;
					}
				});
			}

			if (processAsValue)
			{
				var obvalue = new Object();
				var type = FindValueType(TrimValue(value).ToLower(), ref obvalue);
				values.Push(Expression.Constant(obvalue, type));
			}
		}

		private String TrimValue(String value)
		{
			var trimmedValue = value;
			if (GroupingTokens.Any())
			{
				GroupingTokens.Where(gt => gt.TokenType == ExpressionTokenType.Value).ToList().ForEach(gt => trimmedValue = value.Trim(gt.Name.ToCharArray()));
			}

			return trimmedValue;
		}

		protected virtual Expression BuildExpression(IOperatorToken mytoken, Expression[] arguments)
		{
			Expression result = null;

			if (mytoken != null)
			{
				if (arguments != null && arguments.Any())
				{
					switch (mytoken.OperatorExpressionType)
					{
						case OperatorExpressionType.Unary:
							if (arguments.Length != 1)
							{
								throw new ArgumentException("BuildExpression: Unary expression array length should be equal to 1");
							}
							var ufuncDefinition = (mytoken as OperatorToken<Func<Expression, UnaryExpression>>).FunctionDefiniton;
							result = ufuncDefinition.Invoke(arguments[0]);
							break;
						case OperatorExpressionType.Binary:
							if (arguments.Length != 2)
							{
								throw new ArgumentException("BuildExpression: Binary expression array length should be equal to 2");
							}
							var bfuncDefinition = (mytoken as OperatorToken<Func<Expression, Expression, BinaryExpression>>).FunctionDefiniton;
							result = bfuncDefinition.Invoke(arguments[0], arguments[1]);
							break;
						case OperatorExpressionType.InstanceMethodCall:
							if (arguments.Length != 2)
							{
								throw new ArgumentException("BuildExpression: InstanceMethodCall expression array length should be equal to 2");
							}
							var mfuncDefinition = (mytoken as OperatorToken<Func<Expression, MethodInfo, IEnumerable<Expression>, MethodCallExpression>>).FunctionDefiniton;
							var le = new List<Expression>() { { arguments[1] } };
							var callmethod = mytoken.FirstArgumentType.GetMethod(mytoken.OperatorSymbol);
							if (callmethod == null)
							{
								throw new ArgumentException(String.Format("BuildExpression: InstanceMethodCall method info expression for operatorsymbol {0} is null", mytoken.OperatorSymbol));
							}
							result = mfuncDefinition.Invoke(arguments[0], callmethod, le);
							break;
					}
				}
				else
				{
					throw new ArgumentException("BuildExpression: Empty Expression Array has been passed");
				}
			}

			return result;
		}

		private Type FindValueType(String value, ref Object returnVal)
		{
			Int32 inumber;
			if (Int32.TryParse(value, out inumber))
			{
				returnVal = inumber;
				return typeof(Int32);
			}

			Int64 lnumber;
			if (Int64.TryParse(value, out lnumber))
			{
				returnVal = lnumber;
				return typeof(Int64);
			}

			Boolean bl;
			if (Boolean.TryParse(value, out bl))
			{
				returnVal = bl;
				return typeof(Boolean);
			}

			Byte bnumber;
			if (Byte.TryParse(value, out bnumber))
			{
				returnVal = bnumber;
				return typeof(Byte);
			}

			Decimal dnumber;
			if (Decimal.TryParse(value, out dnumber))
			{
				returnVal = dnumber;
				return typeof(Decimal);
			}

			DateTime date;
			if (DateTime.TryParse(value, out date))
			{
				returnVal = date;
				return typeof(DateTime);
			}

			returnVal = value;
			return typeof(String);
		}

		#endregion Private Methods

		#endregion Methods
	}
}
