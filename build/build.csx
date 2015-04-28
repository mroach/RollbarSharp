#r "System.Xml.Linq"
using System.Reflection; 
using System.Xml.Linq;
using System.Diagnostics;

var assembly = Assembly.LoadFrom("..\\src\\RollbarSharp\\bin\\Debug\\RollbarSharp.dll");
var version = assembly.GetName().Version.ToString();
Console.WriteLine("Packing version {0}", version);

var pInfo = new ProcessStartInfo("nuget", string.Format("pack -Version {0}", version)) { WorkingDirectory = @"..\src\RollbarSharp\" };
Process.Start(pInfo).WaitForExit();
Console.WriteLine("nupkg created");

Console.WriteLine("What's your Api Key?");
var apiKey = Console.ReadLine();
var arguments = string.Format("push RollbarSharp.{0}.nupkg -ApiKey {1}", version, apiKey);
Console.WriteLine("Executing nuget {0}", arguments);
pInfo = new ProcessStartInfo("nuget", arguments) { WorkingDirectory = @"..\src\RollbarSharp\" };
Console.WriteLine("Pushing new version");
Process.Start(pInfo).WaitForExit();
Console.WriteLine("New version({0}) pushed", version);