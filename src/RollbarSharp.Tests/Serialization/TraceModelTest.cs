using System;
using System.Linq;
using NUnit.Framework;
using RollbarSharp.Builders;
using System.Reflection;

namespace RollbarSharp.Tests.Serialization
{
    [TestFixture]
    public class TraceModelTest
    {
        [Test]
        public void when_creating_from_thrown_exception_should_have_frames()
        {
            try
            {
                GenerateException(null);
            }
            catch (Exception ex)
            {
                var model = TraceChainModelBuilder.CreateFromException(ex).FirstOrDefault();
                Assert.IsNotNull(model.Frames);
            }
        }

        [Test]
        public void when_creating_from_thrown_exception_should_have_more_than_0_frames()
        {
            try
            {
                GenerateException(null);
            }
            catch (Exception ex)
            {
                var model = TraceChainModelBuilder.CreateFromException(ex).FirstOrDefault();
                Assert.Greater(model.Frames.Length, 0);
            }
        }

        [Test]
        public void when_creating_from_thrown_exception_should_have_Exception_model()
        {
            try
            {
                GenerateException(null);
            }
            catch (Exception ex)
            {
                var model = TraceChainModelBuilder.CreateFromException(ex).FirstOrDefault();
                Assert.IsNotNull(model.Exception);
            }
        }

        [Test()]
        public void when_creating_from_thrown_within_reflection_call_exception_should_have_Exception_model()
        {
            var method = this.GetType().GetMethod("MethodWithException");
            try
            {
                method.Invoke(this, new object[]{ null });
                Assert.Fail("La llamada anterior debe lanzar una excepción");
            }
            catch (Exception ex)
            {
                var model = TraceChainModelBuilder.CreateFromException(ex).FirstOrDefault();
                Assert.IsNotNull(model.Exception);
            }
        }

        private int GenerateException(string noparam)
        {
            var x = 0;
            var q = 1 / x;
            return q;
        }

        public void MethodWithException(string noparam)
        {
            throw new ArgumentException("Test exception");
        }
    }
}
