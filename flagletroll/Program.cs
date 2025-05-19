using System;
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
}
