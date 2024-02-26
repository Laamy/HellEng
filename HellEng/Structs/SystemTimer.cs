using System.Runtime.InteropServices;

internal class SystemTimer
{
    [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)] static extern uint TimeBeginPeriod(uint uMilliseconds);
    [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)] static extern uint TimeEndPeriod(uint uMilliseconds);
    [DllImport("winmm.dll", EntryPoint = "timeGetDevCaps", SetLastError = true)] static extern uint timeGetDevCaps(ref TIMECAPS timeCaps, int size);

    [StructLayout(LayoutKind.Sequential)]
    private struct TIMECAPS
    {
        public uint wPeriodMin;
        public uint wPeriodMax;
    }

    private static int GetSystemTimerResolution()
    {
        // storage for the timer resolution
        TIMECAPS caps = new TIMECAPS();

        if (timeGetDevCaps(ref caps, Marshal.SizeOf<TIMECAPS>()) != 0)
            return (int)caps.wPeriodMin; // success

        return -1; // error
    }

    private static void SetSystemTimerResolition(int resolution)
    {
        TimeBeginPeriod((uint)resolution);
    }

    public static void Sleep(int ms)
    {
        int curRes = GetSystemTimerResolution(); // 64hz on windows

        SetSystemTimerResolition(1); // set to 1000hz

        System.Threading.Thread.Sleep(ms); // sleep

        SetSystemTimerResolition(curRes); // reset to original resolution
    }
}