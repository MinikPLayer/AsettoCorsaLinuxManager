using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ACLinuxManager.Utils;

public static class ProcessUtils
{
    public static async Task<string> RunShellCommand(string cmd, bool redirectOutput = true)
    {
        Process p = new Process();
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Unix:
                p.StartInfo.FileName = "sh";
                p.StartInfo.Arguments = $"-c \"{cmd}\"";
                break;
            
            case PlatformID.Win32Windows:
            case PlatformID.Win32S:
            case PlatformID.Win32NT:
                p.StartInfo.FileName = "cmd";
                p.StartInfo.Arguments = $"/C \"{cmd}\"";
                break;
            
            default:
                throw new NotSupportedException("Platform not supported");
        }

        if (redirectOutput)
        {
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
        }

        p.Start();
        await p.WaitForExitAsync();

        if (redirectOutput)
            return await p.StandardOutput.ReadToEndAsync();

        return "";
    }
    
    // Works only on linux
    public static async Task<string> GetParentName()
    {
        if (Environment.OSVersion.Platform != PlatformID.Unix)
            throw new NotImplementedException();
        
        var parentProcess = await RunShellCommand("ps -e | grep $(ps -o ppid= -p $(echo $PPID))");
        return parentProcess;
    }

    public static async void _RunIfNotAvalonia(Action runFunc)
    {
        #if DEBUG
            var parentName = await GetParentName();
            if (parentName.Contains("java"))
                return;
        #endif

        runFunc();
    }
}