using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class RequestModelBuilder
    {
        /// <summary>
        /// Converts the current standard <see cref="HttpRequest"/> to a <see cref="RequestModel"/>
        /// Copies over: URL, HTTP method, HTTP headers, query string params, POST params, user IP, route params
        /// </summary>
        /// <returns></returns>
        public static RequestModel CreateFromCurrentRequest()
        {
            var cx = HttpContext.Current;

            return cx == null ? new RequestModel() : CreateFromHttpRequest(cx.Request);
        }

        /// <summary>
        /// Converts a standard <see cref="HttpRequest"/> to a <see cref="RequestModel"/>
        /// Copies over: URL, HTTP method, HTTP headers, query string params, POST params, user IP, route params
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RequestModel CreateFromHttpRequest(HttpRequest request)
        {
            var m = new RequestModel();

            m.Url = request.Url.ToString();
            m.Method = request.HttpMethod;
            m.Headers = CollectionToDictionary(request.Headers);

            m.QueryStringParameters = CollectionToDictionary(request.QueryString);
            
            m.PostParameters = CollectionToDictionary(request.Form);
            m.UserIp = request.UserHostAddress;

            m.Parameters = request.RequestContext.RouteData.Values.ToDictionary(v => v.Key, v => v.Value.ToString());

            return m;
        }
        
        /// <summary>
        /// Convert a <see cref="NameValueCollection"/> to a dictionary which is far more usable.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        internal static IDictionary<string, string> CollectionToDictionary(NameValueCollection col)
        {
            if (col == null || col.Count == 0)
                return new Dictionary<string, string>();

            return col.AllKeys.ToDictionary(key => key, key => col[key]);
        }
    }
}
