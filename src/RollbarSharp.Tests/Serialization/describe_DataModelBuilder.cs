using System;
using System.Collections.Generic;
using NSpec;
using RollbarSharp.Builders;

namespace RollbarSharp.Tests.Serialization
{
    public class describe_DataModelBuilder : nspec
    {
        public void when_creating_notice_from_an_exception_that_has_data()
        {
            var ex = new Exception("fail");
            ex.Data["session_id"] = 123;
            ex.Data["user"] = "mroach";

            var model = (new DataModelBuilder()).CreateExceptionNotice(ex);

            it["should have Custom['exception_data']"] = () => model.Custom.ContainsKey("exception_data");
            it["should have a Dictionary<object, object> in there"] = () => model.Custom["exception_data"].should_cast_to<Dictionary<object, object>>();
            it["should have the same number of items"] = () => ((Dictionary<object, object>)model.Custom["exception_data"]).Count.should_be(ex.Data.Count);
        }
    }
}
