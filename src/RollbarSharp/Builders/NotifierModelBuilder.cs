using System.Reflection;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class NotifierModelBuilder
    {
        /// <summary>
        /// Creates a model representing this notifier binding itself.
        /// Will be reported as the assembly's name and currently compiled version.
        /// </summary>
        /// <returns></returns>
        public static NotifierModel CreateFromAssemblyInfo()
        {
            var ai = Assembly.GetAssembly(typeof(NotifierModel)).GetName();

            return new NotifierModel(ai.Name, ai.Version.ToString());
        }
    }
}
