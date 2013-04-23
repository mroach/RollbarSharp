namespace RollbarSharp
{
    /// <summary>
    /// Event args fired when the response is received from the Rollbar endpoint
    /// </summary>
    public class RequestCompletedEventArgs
    {
        public Result Result { get; set; }

        public RequestCompletedEventArgs(Result result)
        {
            Result = result;
        }
    }
}