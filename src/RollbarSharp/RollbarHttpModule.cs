using System;
using System.Web;

namespace RollbarSharp
{
  public class RollbarHttpModule : IHttpModule
  {
    public void Init(HttpApplication context)
    {
      context.Error += SendError;
    }

    public void Dispose()
    {
    }

    private void SendError(object sender, EventArgs e)
    {
      var application = (HttpApplication)sender;
      new RollbarClient().SendException(application.Server.GetLastError().GetBaseException());
    }
    
  }
}