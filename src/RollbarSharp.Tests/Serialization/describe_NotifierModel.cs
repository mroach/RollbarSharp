using System.Reflection;
using NSpec;
using RollbarSharp.Builders;
using RollbarSharp.Serialization;

namespace RollbarSharp.Tests.Serialization
{
    public class describe_NotifierModel : nspec
    {
        public void when_creating_notifier_from_assembly_info()
        {
            var model = NotifierModelBuilder.CreateFromAssemblyInfo();
            var expectedVersion = Assembly.GetAssembly(typeof (NotifierModel)).GetName();

            it["should have RollbarSharp for Name"] = () => model.Name.should_be("RollbarSharp");
            it["should have Version populated"] = () => model.Version.should_not_be_empty();
            it["Version should match assembly version"] = () => model.Version.should_be(expectedVersion.Version.ToString());
        }
    }
}
