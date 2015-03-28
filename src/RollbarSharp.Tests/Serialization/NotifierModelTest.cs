using System.Reflection;
using NUnit.Framework;
using RollbarSharp.Builders;
using RollbarSharp.Serialization;

namespace RollbarSharp.Tests.Serialization
{
    [TestFixture]
    public class NotifierModelTest
    {
        NotifierModel model;

        [SetUp]
        public void SetUp()
        {
            model = NotifierModelBuilder.CreateFromAssemblyInfo();
        }

        [Test]
        public void when_creating_notifier_from_assembly_info_should_have_RollbarSharp_for_Name()
        {
            Assert.AreEqual("RollbarSharp", model.Name);
        }

        [Test]
        public void when_creating_notifier_from_assembly_info_should_have_Version_populated()
        {
            Assert.IsNotNullOrEmpty(model.Version);
        }

        [Test]
        public void when_creating_notifier_from_assembly_info_Version_should_match_assembly_version()
        {
            var expectedVersion = Assembly.GetAssembly(typeof(NotifierModel)).GetName();

            Assert.AreEqual(expectedVersion.Version.ToString(), model.Version);
        }
    }
}
