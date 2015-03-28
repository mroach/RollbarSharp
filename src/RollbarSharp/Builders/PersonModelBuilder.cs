using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public class PersonModelBuilder
    {
        /// <summary>
        /// Find just the username from server vars: AUTH_USER, LOGON_USER, REMOTE_USER
        /// Sets both the ID and Username to this username since ID is required.
        /// Email address won't be set.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static PersonModel CreateFromHttpRequest(HttpRequest request)
        {
            var username = request.ServerVariables["AUTH_USER"] ??
                           request.ServerVariables["LOGON_USER"] ??
                           request.ServerVariables["REMOTE_USER"];

            return new PersonModel {Id = username, Username = username};
        }
    }
}
