using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MDM.MessageBrokerManager
{
    /// <summary>
    /// Generic Rest client class which can make requests to a Rest API.
    /// </summary>
    public class RestClient
    {
        #region Variables

        private MediaTypeWithQualityHeaderValue _contentTypeHeader = null;
        private HttpClient _httpClient = null;

        #endregion Variables

        /// <summary>
        /// Initializes a new instance of the RestClient class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public RestClient(String endpoint)
        {
            if (String.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException("endpoint");
            }

            if (!endpoint.EndsWith("/"))
            {
                endpoint += "/";
            }

            Uri endPointUri = new Uri(endpoint);

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            _httpClient.Timeout = new TimeSpan(0, 10, 0);
            _httpClient.BaseAddress = endPointUri;
        }

        /// <summary>
        /// Sets the content type header.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public void SetContentTypeHeader(String contentType)
        {
            _contentTypeHeader = new MediaTypeWithQualityHeaderValue(contentType);
        }

        /// <summary>
        /// Posts to the specified method name with the post data.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>Returns the response as String</returns>
        public String Post(String methodName, String postData)
        {
            StringContent content = new StringContent(postData);

            if (_contentTypeHeader != null)
            {
                content.Headers.ContentType = _contentTypeHeader;
            }

            var response = _httpClient.PostAsync(methodName, content).Result;

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Gets the specified method name.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>Returns the response as String</returns>
        public String Get(String methodName)
        {
            var response = _httpClient.GetAsync(methodName).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
