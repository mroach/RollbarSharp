using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class RequestModelBuilder
    {
        /// <summary>
        /// Converts a standard <see cref="HttpRequest"/> to a <see cref="RequestModel"/>
        /// Copies over: URL, HTTP method, HTTP headers, query string params, POST params, user IP, route params
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <param name="scrubParams"></param>
        /// <returns></returns>
        public static RequestModel CreateFromHttpRequest(HttpRequest request, HttpSessionState session, string[] scrubParams = null)
        {
            var m = new RequestModel();

            m.Url = request.Url.ToString();
            m.Method = request.HttpMethod;
            m.Headers = request.Headers.ToDictionary();
            m.Session = session.ToDictionary();

            m.QueryStringParameters = request.QueryString.ToDictionary();
            m.PostParameters = request.Unvalidated.Form.ToDictionary();

            // add posted files to the post collection
            try
            {
                if (request.Files.Count > 0)
                    foreach (var file in request.Files.Describe())
                        m.PostParameters.Add(file.Key, "FILE: " + file.Value);
            }
            catch (HttpException)
            {
                // Files from request could not be read here because they are streamed
                // and have been read earlier by e.g. WCF Rest Service or Open RIA Services
            }

            // if the X-Forwarded-For header exists, use that as the user's IP.
            // that will be the true remote IP of a user behind a proxy server or load balancer
            m.UserIp = IpFromXForwardedFor(request) ?? request.UserHostAddress;

            m.Parameters = request.RequestContext.RouteData.Values.ToDictionary(v => v.Key, v => v.Describe());

            if (scrubParams != null)
            {
                m.Headers = Scrub(m.Headers, scrubParams);
                m.Session = Scrub(m.Session, scrubParams);
                m.QueryStringParameters = Scrub(m.QueryStringParameters, scrubParams);
                m.PostParameters = Scrub(m.PostParameters, scrubParams);
            }

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
                forwardedFor = forwardedFor.Split(',').Last().Trim();
            }
            return forwardedFor;
        }

        /// <summary>
        /// Finds dictionary keys in the <see cref="scrubParams"/> list and replaces their values
        /// with asterisks. Key comparison is case insensitive.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="scrubParams"></param>
        /// <returns></returns>
        private static IDictionary<string, string> Scrub(IDictionary<string, string> dict, string[] scrubParams)
        {
            if (dict == null || !dict.Any())
                return dict;

            if (scrubParams == null || !scrubParams.Any())
                return dict;

            var itemsToUpdate = dict.Keys
                                    .Where(k => scrubParams.Contains(k, StringComparer.InvariantCultureIgnoreCase))
                                    .ToArray();

            if (itemsToUpdate.Any())
            {
                foreach (var key in itemsToUpdate)
                {
                    var len = dict[key] == null ? 8 : dict[key].Length;
                    dict[key] = new string('*', len);
                }
            }

            return dict;
        }
    }
}
