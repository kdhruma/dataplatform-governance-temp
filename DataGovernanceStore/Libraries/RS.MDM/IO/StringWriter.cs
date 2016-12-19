using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RS.MDM.IO
{
    /// <summary>
    /// Provides String Writer functionality with user defined encoding
    /// </summary>
    public class StringWriter : System.IO.StringWriter
    {
        #region Fields

        /// <summary>
        /// Field for the encoding
        /// </summary>
        private Encoding _encoding;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with string builder and encoding as input parameters
        /// </summary>
        /// <param name="encoding">Indicates the encoding that needs to be used for the writing strings</param>
        public StringWriter(Encoding encoding)
            : base()
        {
            this._encoding = encoding;
        }

        /// <summary>
        /// Constructor with string builder and encoding as input parameters
        /// </summary>
        /// <param name="stringBuilder">Indicates instance of string builder</param>
        /// <param name="encoding">Indicates the encoding that needs to be used for the writing strings</param>
        public StringWriter(StringBuilder stringBuilder, Encoding encoding)
            : base(stringBuilder)
        {
            this._encoding = encoding;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Indicates the encoding that needs to be used for the writing strings 
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
        }

        #endregion
    }
}
