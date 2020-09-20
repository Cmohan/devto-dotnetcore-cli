using System.IO;
using System.Reflection;

public static class Globals
{
    public static readonly string CONTENT_PATH = Directory.GetParent(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Parent.Parent.FullName + "\\content";
}