using System;
using NSpec;
using RollbarSharp.Builders;

namespace RollbarSharp.Tests.Serialization
{
    public class describe_TraceModel : nspec
    {
        public void when_creating_from_thrown_exception()
        {
            try
            {
                GenerateException(null);
            }
            catch (Exception ex)
            {
                var model = TraceModelBuilder.CreateFromException(ex);
                it["should have frames"] = () => model.Frames.should_not_be_default();
                it["should have more than 0 frames"] = () => model.Frames.should(f => f.Length > 0);
                it["should have Exception model"] = () => model.Exception.should_not_be_null();
            }
        }

        private int GenerateException(string noparam)
        {
            var x = 0;
            var q = 1 / x;
            return q;
        }
    }
}
