using System;
using NSpec;
using RollbarSharp.Builders;

namespace RollbarSharp.Tests.Serialization
{
    public class describe_ExceptionModel : nspec
    {
        public void when_creating_from_manually_created_exception()
        {
            var ex = new InvalidOperationException("Invalid operation");
            var model = ExceptionModelBuilder.CreateFromException(ex);

            it["should have expected Class"] = () => model.Class.should_be("InvalidOperationException");
            it["should have Message copied"] = () => model.Message.should_be("Invalid operation");
        }

        public void when_creating_from_thrown_exception()
        {
            try
            {
                int x = 0, y = 1;
                var z = y/x;
            }
            catch (Exception ex)
            {
                var model = ExceptionModelBuilder.CreateFromException(ex);

                it["should have excepted Class"] = () => model.Class.should_be("DivideByZeroException");
                it["should have Message"] = () => model.Message.should_not_be_empty();
            }
        }
    }
}
