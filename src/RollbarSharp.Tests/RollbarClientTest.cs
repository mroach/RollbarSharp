using System;
using NUnit.Framework;

namespace RollbarSharp.Tests
{
    [TestFixture]
    public class RollbarClientTest
    {
        [Test]
        public void when_serializing_message_notice_result_should_not_be_empty()
        {
            var client = new RollbarClient();
            var notice = client.NoticeBuilder.CreateMessageNotice("Hello");

            notice.Server.Host = "miker";
            notice.Request.Url = "http://localhost/hej";
            notice.Request.Method = "GET";

            var serialized = client.Serialize(notice);

            Assert.IsNotNullOrEmpty(serialized);
        }

        [Test]
        public void when_serializing_exception_notice_result_should_not_be_empty()
        {
            var client = new RollbarClient();
            Exception exception = new NotImplementedException();

            var notice = client.NoticeBuilder.CreateExceptionNotice(exception);
            notice.Server.Host = "miker";
            notice.Request.Url = "http://localhost/hej";
            notice.Request.Method = "GET";
            notice.Request.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_3) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.65 Safari/537.31");
            notice.Request.UserIp = "67.90.39.34";
            notice.Person.Id = "123";
            notice.Person.Username = Environment.UserName;

            var serialized = client.Serialize(notice);

            Assert.IsNotNullOrEmpty(serialized);
        }
    }
}
