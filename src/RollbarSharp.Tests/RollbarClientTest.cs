using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RollbarSharp.Builders;

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

        [Test]
        public void when_calling_send_exception_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            Exception exception = new NotImplementedException();

            var task = client.SendException(exception);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_critical_exception_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            Exception exception = new NotImplementedException();

            var task = client.SendCriticalException(exception);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        
        [Test]
        public void when_calling_send_error_exception_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            Exception exception = new NotImplementedException();

            var task = client.SendErrorException(exception);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_warning_exception_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            Exception exception = new NotImplementedException();

            var task = client.SendWarningException(exception);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_critical_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendCriticalMessage(message);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_error_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendErrorMessage(message);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_warning_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendWarningMessage(message);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_info_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendInfoMessage(message);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_debug_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendDebugMessage(message);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_message_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";

            var task = client.SendMessage(message, "info");

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }

        [Test]
        public void when_calling_send_task_returns_and_can_be_used()
        {
            var client = new RollbarClient();
            var message = "";
            
            var notice = new DataModelBuilder().CreateMessageNotice(message, "debug");

            var task = client.Send(notice, null);

            Assert.IsNotNull(task);
            Assert.DoesNotThrow(() => task.Wait(0));
        }
    }
}
