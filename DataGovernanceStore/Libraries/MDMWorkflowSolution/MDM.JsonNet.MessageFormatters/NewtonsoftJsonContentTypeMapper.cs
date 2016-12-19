using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace MDM.JsonNet.MessageFormatters
{
    /// <summary>
    /// 
    /// </summary>
    public class NewtonsoftJsonContentTypeMapper : WebContentTypeMapper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}
