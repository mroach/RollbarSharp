using System.Reflection;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class NotifierModelBuilder
    {
        public static NotifierModel CreateFromAssemblyInfo()
        {
            var ai = Assembly.GetAssembly(typeof(NotifierModel)).GetName();

            return new NotifierModel(ai.Name, ai.Version.ToString());
        }
    }
}
