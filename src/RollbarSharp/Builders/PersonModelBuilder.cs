using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public class PersonModelBuilder
    {
        /// <summary>
        /// Creates a <see cref="PersonModel"/> from the current HTTP request.
        /// This will not be able to find the user's email address
        /// </summary>
        /// <returns></returns>
        public static PersonModel CreateFromCurrentRequest()
        {
            if (HttpContext.Current == null)
                return new PersonModel();

            return CreateFromHttpRequest(HttpContext.Current.Request);
        }

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
