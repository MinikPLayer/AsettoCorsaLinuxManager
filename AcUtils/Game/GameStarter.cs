using System.Diagnostics;
using AcUtils.Utils;

namespace AcUtils.Game;

public static class GameStarter
{
    public static Result<Process> StartGameProcess(string rootDirectory, Action? gameProcessExited = null)
    {
        var filePath = Path.Combine(rootDirectory, "acs.exe");
        if (!File.Exists(filePath))
            return Result<Process>.Err("Game executable not found");
        
        return Result<Process>.Err("Not implemented");
    }
}