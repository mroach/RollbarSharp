using System;
using System.Linq;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class ExceptionModelBuilder
    {
        /// <summary>
        /// Converts an exception to an <see cref="ExceptionModel"/>.
        /// </summary>
        public static ExceptionModel CreateFromException(Exception ex)
        {
            var m = new ExceptionModel(ex.GetType().Name, ex.Message);
            
            if (ex.Data.Count > 0)
                m.Data = ex.Data.Keys.Cast<object>().ToDictionary(k => k, k => ex.Data[k]);
            
            return m;
        }
    }
}
