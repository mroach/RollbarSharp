using System;
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

            // add posted files to the post collection
            if (request.Files.Count > 0)
                foreach (var file in DescribePostedFiles(request.Files))
                    m.PostParameters.Add(file.Key, "FILE: " + file.Value);

            // if the X-Forwarded-For header exists, use that as the user's IP.
            // that will be thetrue remote IP of a user behind a proxy server or load balancer
            m.UserIp = IpFromXForwardedFor(request) ?? request.UserHostAddress;

            m.Parameters = request.RequestContext.RouteData.Values.ToDictionary(v => v.Key, v => v.Value.ToString());

            return m;
        }

        // X-Forwarded-For header, if populated, contains a comma separated list of ip address
        // of each successive proxy server. Take the last or most reliable IP address if there
        // are multiple addresses.
        private static string IpFromXForwardedFor(HttpRequest request)
        {
            var forwardedFor = request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(forwardedFor) && forwardedFor.Contains(","))
            {
                forwardedFor = forwardedFor.Split(new[] { ',' }).Last().Trim();
            }
            return forwardedFor;
        }

        /// <summary>
        /// Convert a <see cref="NameValueCollection"/> to a dictionary which is far more usable.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private static IDictionary<string, string> CollectionToDictionary(NameValueCollection col)
        {
            if (col == null || col.Count == 0)
                return new Dictionary<string, string>();

            return col.AllKeys.Where(key => key != null).ToDictionary(key => key, key => col[key]);
        }

        /// <summary>
        /// Create a dictionary describing the files posted.
        /// The key is the form field name, value the file name, mime type, and size in bytes.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static IDictionary<string, string> DescribePostedFiles(HttpFileCollection files)
        {
            return files.AllKeys.ToDictionary(k => k, k => DescribePostedFile(files[k]));
        }

        private static string DescribePostedFile(HttpPostedFile file)
        {
            if (file.ContentLength == 0 && string.IsNullOrEmpty(file.FileName))
                return "[empty]";

            return string.Format("{0} ({1}, {2} bytes)", file.FileName, file.ContentType, file.ContentLength);
        }
    }
}
