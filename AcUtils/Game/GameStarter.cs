using System.Diagnostics;
using AcUtils.Utils;

namespace AcUtils.Game;

public static class GameStarter
{
    public static Result<bool> StartGameProcess()
    {
        var steamProcess = new Process();
        steamProcess.StartInfo.FileName = "steam";
        steamProcess.StartInfo.Arguments = "steam://rungameid/244210";
        steamProcess.Start();
        return Result<bool>.Ok(true);
    }

    public static async Task<Result<Process>> WaitUntilGameLaunches(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var process = Process.GetProcessesByName("acs.exe");
                if (process.Length > 0)
                    return Result<Process>.Ok(process[0]);

                await Task.Delay(100, token);
            }
        }
        catch (TaskCanceledException e)
        {
        }

        return Result<Process>.Err("Task cancelled");
    }

    public static async Task<Result<bool>> WaitUntilGameExits(Process process, CancellationToken token)
    {
        try
        {
            await process.WaitForExitAsync(token);
        }
        catch (TaskCanceledException e)
        {
            return Result<bool>.Err("Task cancelled");
        }

        return Result<bool>.Ok(true);
    }

    public static Result<bool> KillProcess(Process gameProcess)
    {
        if(gameProcess.HasExited)
            return Result<bool>.Err("Game process is not running");

        gameProcess.Kill();
        return Result<bool>.Ok(true);
    }
}