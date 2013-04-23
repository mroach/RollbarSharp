using System;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class ExceptionModelBuilder
    {
        /// <summary>
        /// Converts an exception to an <see cref="ExceptionModel"/>.
        /// If the <see cref="Exception.Data"/> dictionary contains a
        /// key called 'fingerprint' it will be used to calculate the
        /// 'fingerprint' field on the notice.
        /// </summary>
        public static ExceptionModel CreateFromException(Exception ex)
        {
            var m = new ExceptionModel(ex.GetType().Name, ex.Message);

            if (ex.Data.Contains("fingerprint"))
                m.Fingerprint = ex.Data["fingerprint"].ToString();

            return m;
        }
    }
}
