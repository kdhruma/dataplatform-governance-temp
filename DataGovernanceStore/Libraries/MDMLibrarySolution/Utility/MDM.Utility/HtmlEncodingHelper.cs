using System;

namespace MDM.Utility
{
    /// <summary>
    /// Represents helper methods for html encoding.
    /// </summary>
    public class HtmlEncodingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static String HtmlEncode(String inString)
        {
            String s = null;
            if (!String.IsNullOrEmpty(inString) && inString != "&amp;" && inString != "&quot;" && inString != "&apos;" && inString != "&lt;" && inString != "&gt;")
            {
                s = inString;
                s = s.Replace("&", "&amp;");
                s = s.Replace("<", "&lt;");
                s = s.Replace(">", "&gt;");
                s = s.Replace(@"""", "&quot;");
                s = s.Replace("'", "&apos;");
            }

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static String HtmlDecode(String inString)
        {
            String s = null;
            if (!String.IsNullOrEmpty(inString) && inString != "&amp;" && inString != "&quot;" && inString != "&apos;" && inString != "&lt;" && inString != "&gt;")
            {
                s = inString;
                s = s.Replace("&amp;", "&");
                s = s.Replace("&lt;", "<");
                s = s.Replace("&gt;", ">");
                s = s.Replace("&quot;", @"""");
                s = s.Replace("&apos;", "'");
            }

            return s;
        }
    }
}