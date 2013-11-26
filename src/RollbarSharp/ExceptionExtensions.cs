using System;
using RollbarSharp.Serialization;

namespace RollbarSharp
{
    public static class ExceptionExtensions
    {
        public static void SendToRollbar(this Exception ex, string title = null, string level = "error", Action<DataModel> modelAction = null)
        {
            if (ex == null)
                return;

            (new RollbarClient()).SendException(ex, title, level, modelAction);
        }
    }
}
