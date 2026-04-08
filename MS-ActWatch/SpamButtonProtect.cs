namespace MS_ActWatch
{
    static class SpamButtonProtect
    {
        static Dictionary<Sharp.Shared.Units.EntityIndex, long> g_Buttons = [];
        static Dictionary<Sharp.Shared.Units.EntityIndex, long> g_Triggers = [];

        public static bool ButtonAvailableToShow(Sharp.Shared.Units.EntityIndex iID)
        {
            if (Cvar.ButtonSpam <= 0.0f) return true;
            long iTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (g_Buttons.GetValueOrDefault(iID) + Math.Ceiling(Cvar.ButtonSpam * 1000) < iTime)
            {
                g_Buttons[iID] = iTime;
                return true;
            }
            
            return false;
        }

        public static bool TriggersAvailableToShow(Sharp.Shared.Units.EntityIndex iID)
        {
            if (Cvar.TriggerSpam <= 0.0f) return true;
            long iTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (g_Triggers.GetValueOrDefault(iID) + Math.Ceiling(Cvar.TriggerSpam * 1000) < iTime)
            {
                g_Triggers[iID] = iTime;
                return true;
            }

            return false;
        }

        public static void MapStartClear()
        {
            g_Buttons.Clear();
            g_Triggers.Clear();
        }
    }
}
