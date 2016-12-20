using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDM.ExpressionParser
{
	using MDM.BufferManager;
	using MDM.CacheManager.Business;
	using MDM.Utility;
	
	public class ExpressionParserManager
    {
		#region Constructors

		/// <summary>
        /// 
        /// </summary>
		public ExpressionParserManager()
        {
            ParserBufferManager = new ExpressionParserBufferManager();
        }

		#endregion Constructors

		#region Properties

		private ExpressionParserBufferManager ParserBufferManager { get; set; }

		#endregion Properties

		#region Methods

		#region Public Methods

		/// <summary>
        /// Parse and get the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="getLatest"></param>
        /// <returns></returns>
		public ExpressionProcessor Get(String expression, Boolean getLatest = false)
        {
            ExpressionProcessor processor = null;

            if (!getLatest)
            {
                processor = ParserBufferManager.FindExpression(expression);
            }

            if ((processor == null))
            {
                processor = new ExpressionProcessor(); 
                processor.Parse(new StringBuilder(expression.ToString()));

                ParserBufferManager.AddExpressionToCache(expression, processor, 3, false);
            }

            return processor;
		}

		#endregion Public Methods

		#region Private Methods

		#endregion Private Methods

		#endregion Methods
	}
}
