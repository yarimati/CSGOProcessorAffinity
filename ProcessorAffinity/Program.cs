using System.Diagnostics;

const string CSGO = "csgo";
const int MAX_ATTEMPTS_COUNT = 10;
int attemptsCount = 0;

SetProcessorAffinity();

return;

void SetProcessorAffinity()
{
    while (attemptsCount != MAX_ATTEMPTS_COUNT)
    {
        var process = Log(() =>
        {
            Process[] processes = Process.GetProcessesByName(CSGO);

            attemptsCount++;
            return processes.FirstOrDefault();
        });

        if (process is null)
        {
            Thread.Sleep(4500);
            continue;
        }

        process.ProcessorAffinity = (IntPtr)(1 << Environment.ProcessorCount) - 2;
        Console.WriteLine($"Process: {process.ProcessName}.exe got unchecked CPU 0");
        Thread.Sleep(5000);
        break;
    }
}

Process? Log(Func<Process?> func)
{
    var process = func.Invoke();

    if (process is null)
    {
        Console.WriteLine($"Process: {CSGO}.exe was not found, attempt {attemptsCount}/{MAX_ATTEMPTS_COUNT}");
        return null;
    }
    else
        Console.WriteLine($"Process: {process.ProcessName}.exe found, performing changes");

    return process;
}