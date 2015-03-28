using System;
using NUnit.Framework;
using RollbarSharp.Builders;

namespace RollbarSharp.Tests.Serialization
{
    [TestFixture]
    public class ExceptionModelTest
    {
        [Test]
        public void when_creating_from_manually_created_exception_should_have_expected_Class()
        {
            var ex = new InvalidOperationException("Invalid operation");
            var model = ExceptionModelBuilder.CreateFromException(ex);

            Assert.AreEqual(typeof(InvalidOperationException).Name, model.Class);
        }

        [Test]
        public void when_creating_from_manually_created_exception_should_have_Message_copied()
        {
            var ex = new InvalidOperationException("Invalid operation");
            var model = ExceptionModelBuilder.CreateFromException(ex);

            Assert.AreEqual("Invalid operation", model.Message);
        }

        [Test]
        public void when_creating_from_thrown_exception_should_have_expected_Class()
        {
            try
            {
                CreateException();
            }
            catch (Exception ex)
            {
                var model = ExceptionModelBuilder.CreateFromException(ex);

                Assert.AreEqual(typeof(DivideByZeroException).Name, model.Class);
            }
        }

        [Test]
        public void when_creating_from_thrown_exception_should_have_Message()
        {
            try
            {
                CreateException();
            }
            catch (Exception ex)
            {
                var model = ExceptionModelBuilder.CreateFromException(ex);

                Assert.IsNotNullOrEmpty(model.Message);
            }
        }

        private int CreateException()
        {
            var x = 0;
            var y = 1 / x;
            return y;
        }
    }
}
