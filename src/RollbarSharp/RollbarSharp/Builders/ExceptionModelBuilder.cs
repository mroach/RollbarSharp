using System;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class ExceptionModelBuilder
    {
        public static ExceptionModel CreateFromException(Exception ex)
        {
            var m = new ExceptionModel(ex.GetType().Name, ex.Message);

            if (ex.Data.Contains("fingerprint"))
                m.Fingerprint = ex.Data["fingerprint"].ToString();

            return m;
        }
    }
}
