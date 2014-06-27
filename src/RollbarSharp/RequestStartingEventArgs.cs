namespace RollbarSharp
{
    public class RequestStartingEventArgs
    {
        public string Payload { get; set; }
        public object UserParam { get; set; }

        public RequestStartingEventArgs(string payload, object userParam)
        {
            Payload = payload;
            UserParam = userParam;
        }
    }
}