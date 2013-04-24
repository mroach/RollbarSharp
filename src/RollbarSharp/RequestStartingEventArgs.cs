namespace RollbarSharp
{
    public class RequestStartingEventArgs
    {
        public string Payload { get; set; }

        public RequestStartingEventArgs(string payload)
        {
            Payload = payload;
        }
    }
}