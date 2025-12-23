using MS_ActWatch.ActBan;
using MS_ActWatch.Helpers;
using Sharp.Shared.Enums;
using Sharp.Shared.Objects;
using System.Text.Json;

namespace MS_ActWatch
{
    static class AW
    {
        public static Scheme? g_Scheme = new();
        public static Dictionary<IGameClient, AWPlayer> g_AWPlayer = [];
        public static List<OfflineBan> g_OfflinePlayer = [];
        public static WhiteListConfig? g_WhiteList;
        public static AWAPI g_cAWAPI = new();

        public static Guid? g_TimerRetryDB = null;
        public static Guid? g_TimerUnban = null;

        public static void LoadScheme()
        {
            string sFileName = $"{ActWatch._dllPath!}/scheme/{Cvar.SchemeName}";
            string sData;
            try
            {
                if (File.Exists(sFileName) || File.Exists(sFileName = $"{ActWatch._dllPath!}/scheme/default.json"))
                {
                    sData = File.ReadAllText(sFileName);
                    UI.AWSysInfo("ActWatch.Info.Scheme.Loading", 7, sFileName);
                }
                else
                {
                    UI.AWSysInfo("ActWatch.Info.Scheme.NotFound", 14);
                    return;
                }
                g_Scheme = JsonSerializer.Deserialize<Scheme>(sData);
            }
            catch (Exception e)
            {
                UI.AWSysInfo("ActWatch.Info.Error", 15, $"Bad Scheme file for {Cvar.SchemeName}!");
                UI.AWSysInfo("ActWatch.Info.Error", 15, $"{e.Message}");
            }
        }

        public static void LoadWhiteList()
        {
            if (ActWatch._modSharp!.GetMapName() is { } mapname)
            {
                string sFileName = $"{ActWatch._dllPath!}/maps/{(Cvar.LowerMapname ? mapname.ToLower() : mapname)}.json";
                string sData;
                try
                {
                    if (File.Exists(sFileName))
                    {
                        sData = File.ReadAllText(sFileName);
                        UI.AWSysInfo("ActWatch.Info.WhiteList.Loading", 7, sFileName);
                        g_WhiteList = JsonSerializer.Deserialize<WhiteListConfig>(sData);
                    }
                }
                catch (Exception e)
                {
                    UI.AWSysInfo("ActWatch.Info.Error", 15, $"Bad WhiteList file for {(Cvar.LowerMapname ? mapname.ToLower() : mapname)}!");
                    UI.AWSysInfo("ActWatch.Info.Error", 15, $"{e.Message}");
                }
            }
        }

        public static void InitTimers()
        {
            InitTimerRetry();
            InitTimerUnban();
        }

        public static void RemoveTimers()
        {
            if (g_TimerRetryDB != null)
            {
                ActWatch._modSharp!.StopTimer((Guid)g_TimerRetryDB);
                g_TimerRetryDB = null;
            }
            if (g_TimerUnban != null)
            {
                ActWatch._modSharp!.StopTimer((Guid)g_TimerUnban);
                g_TimerUnban = null;
            }
        }

        private static void InitTimerRetry()
        {
            if (g_TimerRetryDB != null)
            {
                ActWatch._modSharp!.StopTimer((Guid)g_TimerRetryDB);
                g_TimerRetryDB = null;
            }
            g_TimerRetryDB = ActWatch._modSharp!.PushTimer(ActWatch.TimerRetry, 5.0f, GameTimerFlags.Repeatable);
        }

        private static void InitTimerUnban()
        {
            if (g_TimerUnban != null)
            {
                ActWatch._modSharp!.StopTimer((Guid)g_TimerUnban);
                g_TimerUnban = null;
            }
            g_TimerUnban = ActWatch._modSharp!.PushTimer(ActWatch.TimerUnban, 60.0f, GameTimerFlags.Repeatable);
        }

        public static bool CheckDictionary(IGameClient client)
        {
            if (!g_AWPlayer.ContainsKey(client))
                return g_AWPlayer.TryAdd(client, new AWPlayer());
            return true;
        }

        public static bool CheckPermission(IGameClient client, string permission)
        {
            if (client is { IsValid: true, IsFakeClient: false, IsHltv: false })
            {
                var admin = client.IsAuthenticated ? ActWatch._clients!.FindAdmin(client.SteamId) : null;
                if (admin is not null && admin.HasPermission(permission)) return true;
            }
            return false;
        }

        public static byte GetPlayerImmunity(IGameClient client)
        {
            if (client is { IsValid: true, IsFakeClient: false, IsHltv: false })
            {
                var admin = client.IsAuthenticated ? ActWatch._clients!.FindAdmin(client.SteamId) : null;
                if (admin is not null) return admin.Immunity;
            }
            return 0;
        }
        public static string? ConvertSteamID64ToSteamID(string steamId64)
        {
            if (ulong.TryParse(steamId64, out var communityId) && communityId > 76561197960265728)
            {
                var authServer = (communityId - 76561197960265728) % 2;
                var authId = (communityId - 76561197960265728 - authServer) / 2;
                return $"STEAM_0:{authServer}:{authId}";
            }
            return null;
        }
    }
}
