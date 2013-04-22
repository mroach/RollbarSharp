using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class RequestModelBuilder
    {
        public static RequestModel CreateFromCurrentRequest()
        {
            var cx = HttpContext.Current;

            return cx == null ? new RequestModel() : CreateFromHttpRequest(cx.Request);
        }

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
        
        internal static IDictionary<string, string> CollectionToDictionary(NameValueCollection col)
        {
            if (col == null || col.Count == 0)
                return new Dictionary<string, string>();

            return col.AllKeys.ToDictionary(key => key, key => col[key]);
        }
    }
}
