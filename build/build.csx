#r "System.Xml.Linq"
using System.Reflection; 
using System.Xml.Linq;
using System.Diagnostics;

var assembly = Assembly.LoadFrom("..\\src\\RollbarSharp\\bin\\Debug\\RollbarSharp.dll");
var version = assembly.GetName().Version.ToString();
Console.WriteLine("Packing version {0}", version);

var pInfo = new ProcessStartInfo("nuget", string.Format("pack -Version {0}", version)) { WorkingDirectory = @"..\src\RollbarSharp\" };
var p = new Process { StartInfo = pInfo };
p.Start();

//var path = @"..\src\RollbarSharp\RollbarSharp.nuspec";
//var doc = XDocument.Load(path);
//var versionEle = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "version");
//versionEle.Value = version;
//doc.Save(path);
Console.WriteLine("Nuget packed");