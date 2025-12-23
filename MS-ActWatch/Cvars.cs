using MS_ActWatch.Helpers;
using Sharp.Shared.Enums;
using Sharp.Shared.Objects;

namespace MS_ActWatch
{

    public partial class ActWatch
    {
        IConVar? g_Cvar_ButtonBanTime,
            g_Cvar_ButtonBanLong,
            g_Cvar_ButtonBanReason,
            g_Cvar_ButtonUnBanReason,
            g_Cvar_ButtonKeepExpiredBan,
            g_Cvar_ButtonGlobalEnable,
            g_Cvar_ButtonShowButton,
            g_Cvar_ButtonShowDoor,
            g_Cvar_ButtonShowPhysbox,
            g_Cvar_ButtonWatchButton,
            g_Cvar_ButtonWatchDoor,
            g_Cvar_ButtonWatchPhysbox,
            g_Cvar_TriggerBanTime,
            g_Cvar_TriggerBanLong,
            g_Cvar_TriggerBanReason,
            g_Cvar_TriggerUnBanReason,
            g_Cvar_TriggerKeepExpiredBan,
            g_Cvar_TriggerGlobalEnable,
            g_Cvar_TriggerShowOnce,
            g_Cvar_TriggerShowMultiple,
            g_Cvar_TriggerWatchOnce,
            g_Cvar_TriggerWatchMultiple,
            g_Cvar_OfflineClearTime,
            g_Cvar_PlayerFormat,
            g_Cvar_SchemeName,
            g_Cvar_LowerMapname,
            g_Cvar_ServerLanguage;

        private void RegisterCvars()
        {
            g_Cvar_ButtonBanTime = _convars.CreateConVar("ms_awc_bbantime", 0, 0, 43200, "Default button press ban time. 0 - Permanent", ConVarFlags.None);
            g_Cvar_ButtonBanLong = _convars.CreateConVar("ms_awc_bbanlong", 720, 1, 1440000, "Max button press ban time with once `bw_ban` privilege", ConVarFlags.None);
            g_Cvar_ButtonBanReason = _convars.CreateConVar("ms_awc_bbanreason", "Trolling", "Default button press ban reason", ConVarFlags.None);
            g_Cvar_ButtonUnBanReason = _convars.CreateConVar("ms_awc_bunbanreason", "Giving another chance", "Default button press unban reason", ConVarFlags.None);
            g_Cvar_ButtonKeepExpiredBan = _convars.CreateConVar("ms_awc_bkeep_expired_ban", true, "Enable/Disable keep expired button press bans", ConVarFlags.None);

            g_Cvar_ButtonGlobalEnable = _convars.CreateConVar("ms_awc_benable", true, "Enable/Disable button press functionality", ConVarFlags.None);
            g_Cvar_ButtonShowButton = _convars.CreateConVar("ms_awc_bshow_button", true, "Enable/Disable display of func_[rot_]button presses", ConVarFlags.None);
            g_Cvar_ButtonShowDoor = _convars.CreateConVar("ms_awc_bshow_door", true, "Enable/Disable display of func_door[_rotating] presses", ConVarFlags.None);
            g_Cvar_ButtonShowPhysbox = _convars.CreateConVar("ms_awc_bshow_physbox", true, "Enable/Disable display of func_physbox presses", ConVarFlags.None);
            g_Cvar_ButtonWatchButton = _convars.CreateConVar("ms_awc_bwatch_button", true, "Enable/Disable watch of func_[rot_]button presses. Do bans affect", ConVarFlags.None);
            g_Cvar_ButtonWatchDoor = _convars.CreateConVar("ms_awc_bwatch_door", true, "Enable/Disable watch of func_door[_rotating] presses. Do bans affect", ConVarFlags.None);
            g_Cvar_ButtonWatchPhysbox = _convars.CreateConVar("ms_awc_bwatch_physbox", true, "Enable/Disable watch of func_physbox presses. Do bans affect", ConVarFlags.None);

            g_Cvar_TriggerBanTime = _convars.CreateConVar("ms_awc_tbantime", 0, 0, 43200, "Default trigger touch ban time. 0 - Permanent", ConVarFlags.None);
            g_Cvar_TriggerBanLong = _convars.CreateConVar("ms_awc_tbanlong", 720, 1, 1440000, "Max trigger touch ban time with once `tw_ban` privilege", ConVarFlags.None);
            g_Cvar_TriggerBanReason = _convars.CreateConVar("ms_awc_tbanreason", "Trolling", "Default trigger touch ban reason", ConVarFlags.None);
            g_Cvar_TriggerUnBanReason = _convars.CreateConVar("ms_awc_tunbanreason", "Giving another chance", "Default trigger touch unban reason", ConVarFlags.None);
            g_Cvar_TriggerKeepExpiredBan = _convars.CreateConVar("ms_awc_tkeep_expired_ban", true, "Enable/Disable keep expired trigger touch bans", ConVarFlags.None);

            g_Cvar_TriggerGlobalEnable = _convars.CreateConVar("ms_awc_tenable", true, "Enable/Disable trigger touch functionality", ConVarFlags.None);
            g_Cvar_TriggerShowOnce = _convars.CreateConVar("ms_awc_tshow_once", true, "Enable/Disable display of trigger_once touching", ConVarFlags.None);
            g_Cvar_TriggerShowMultiple = _convars.CreateConVar("ms_awc_tshow_multiple", false, "Enable/Disable display of trigger_multiple touching", ConVarFlags.None);
            g_Cvar_TriggerWatchOnce = _convars.CreateConVar("ms_awc_twatch_once", true, "Enable/Disable watch of trigger_once touching. Do bans affect", ConVarFlags.None);
            g_Cvar_TriggerWatchMultiple = _convars.CreateConVar("ms_awc_twatch_multiple", false, "Enable/Disable watch of trigger_multiple touching. Do bans affect", ConVarFlags.None);

            g_Cvar_OfflineClearTime = _convars.CreateConVar("ms_awc_offline_clear_time", 30, 1, 240, "Time during which data is stored (1-240)", ConVarFlags.None);
            g_Cvar_PlayerFormat = _convars.CreateConVar("ms_awc_player_format", 3, 0, 3, "Changes the way player information is displayed by default (0 - Only Nickname, 1 - Nickname and UserID, 2 - Nickname and SteamID, 3 - Nickname, UserID and SteamID)", ConVarFlags.Notify);

            g_Cvar_SchemeName = _convars.CreateConVar("ms_awc_scheme_name", "default.json", "Filename for the ActWatch scheme", ConVarFlags.None);
            g_Cvar_LowerMapname = _convars.CreateConVar("ms_awc_lower_mapname", false, "Automatically lowercase map name", ConVarFlags.None);

            g_Cvar_ServerLanguage = _convars.CreateConVar("ms_awc_server_lang", "en-us", "Specify the language into which the server messages should be translated", ConVarFlags.None);

            if (g_Cvar_ButtonBanTime != null) _convars.InstallChangeHook(g_Cvar_ButtonBanTime, OnCvarChanged_ButtonBanTime);
            if (g_Cvar_ButtonBanLong != null) _convars.InstallChangeHook(g_Cvar_ButtonBanLong, OnCvarChanged_ButtonBanLong);
            if (g_Cvar_ButtonBanReason != null) _convars.InstallChangeHook(g_Cvar_ButtonBanReason, OnCvarChanged_ButtonBanReason);
            if (g_Cvar_ButtonUnBanReason != null) _convars.InstallChangeHook(g_Cvar_ButtonUnBanReason, OnCvarChanged_ButtonUnBanReason);
            if (g_Cvar_ButtonKeepExpiredBan != null) _convars.InstallChangeHook(g_Cvar_ButtonKeepExpiredBan, OnCvarChanged_ButtonKeepExpiredBan);

            if (g_Cvar_ButtonGlobalEnable != null) _convars.InstallChangeHook(g_Cvar_ButtonGlobalEnable, OnCvarChanged_ButtonGlobalEnable);
            if (g_Cvar_ButtonShowButton != null) _convars.InstallChangeHook(g_Cvar_ButtonShowButton, OnCvarChanged_ButtonShowButton);
            if (g_Cvar_ButtonShowDoor != null) _convars.InstallChangeHook(g_Cvar_ButtonShowDoor, OnCvarChanged_ButtonShowDoor);
            if (g_Cvar_ButtonShowPhysbox != null) _convars.InstallChangeHook(g_Cvar_ButtonShowPhysbox, OnCvarChanged_ButtonShowPhysbox);
            if (g_Cvar_ButtonWatchButton != null) _convars.InstallChangeHook(g_Cvar_ButtonWatchButton, OnCvarChanged_ButtonWatchButton);
            if (g_Cvar_ButtonWatchDoor != null) _convars.InstallChangeHook(g_Cvar_ButtonWatchDoor, OnCvarChanged_ButtonWatchDoor);
            if (g_Cvar_ButtonWatchPhysbox != null) _convars.InstallChangeHook(g_Cvar_ButtonWatchPhysbox, OnCvarChanged_ButtonWatchPhysbox);

            if (g_Cvar_TriggerBanTime != null) _convars.InstallChangeHook(g_Cvar_TriggerBanTime, OnCvarChanged_TriggerBanTime);
            if (g_Cvar_TriggerBanLong != null) _convars.InstallChangeHook(g_Cvar_TriggerBanLong, OnCvarChanged_TriggerBanLong);
            if (g_Cvar_TriggerBanReason != null) _convars.InstallChangeHook(g_Cvar_TriggerBanReason, OnCvarChanged_TriggerBanReason);
            if (g_Cvar_TriggerUnBanReason != null) _convars.InstallChangeHook(g_Cvar_TriggerUnBanReason, OnCvarChanged_TriggerUnBanReason);
            if (g_Cvar_TriggerKeepExpiredBan != null) _convars.InstallChangeHook(g_Cvar_TriggerKeepExpiredBan, OnCvarChanged_TriggerKeepExpiredBan);

            if (g_Cvar_TriggerGlobalEnable != null) _convars.InstallChangeHook(g_Cvar_TriggerGlobalEnable, OnCvarChanged_TriggerGlobalEnable);
            if (g_Cvar_TriggerShowOnce != null) _convars.InstallChangeHook(g_Cvar_TriggerShowOnce, OnCvarChanged_TriggerShowOnce);
            if (g_Cvar_TriggerShowMultiple != null) _convars.InstallChangeHook(g_Cvar_TriggerShowMultiple, OnCvarChanged_TriggerShowMultiple);
            if (g_Cvar_TriggerWatchOnce != null) _convars.InstallChangeHook(g_Cvar_TriggerWatchOnce, OnCvarChanged_TriggerWatchOnce);
            if (g_Cvar_TriggerWatchMultiple != null) _convars.InstallChangeHook(g_Cvar_TriggerWatchMultiple, OnCvarChanged_TriggerWatchMultiple);

            if (g_Cvar_OfflineClearTime != null) _convars.InstallChangeHook(g_Cvar_OfflineClearTime, OnCvarChanged_OfflineClearTime);
            if (g_Cvar_PlayerFormat != null) _convars.InstallChangeHook(g_Cvar_PlayerFormat, OnCvarChanged_PlayerFormat);

            if (g_Cvar_SchemeName != null) _convars.InstallChangeHook(g_Cvar_SchemeName, OnCvarChanged_SchemeName);
            if (g_Cvar_LowerMapname != null) _convars.InstallChangeHook(g_Cvar_LowerMapname, OnCvarChanged_LowerMapname);

            if (g_Cvar_ServerLanguage != null) _convars.InstallChangeHook(g_Cvar_ServerLanguage, OnCvarChanged_ServerLanguage);
        }

        private void UnRegisterCvars()
        {
            if (g_Cvar_ButtonBanTime != null) _convars.RemoveChangeHook(g_Cvar_ButtonBanTime, OnCvarChanged_ButtonBanTime);
            if (g_Cvar_ButtonBanLong != null) _convars.RemoveChangeHook(g_Cvar_ButtonBanLong, OnCvarChanged_ButtonBanLong);
            if (g_Cvar_ButtonBanReason != null) _convars.RemoveChangeHook(g_Cvar_ButtonBanReason, OnCvarChanged_ButtonBanReason);
            if (g_Cvar_ButtonUnBanReason != null) _convars.RemoveChangeHook(g_Cvar_ButtonUnBanReason, OnCvarChanged_ButtonUnBanReason);
            if (g_Cvar_ButtonKeepExpiredBan != null) _convars.RemoveChangeHook(g_Cvar_ButtonKeepExpiredBan, OnCvarChanged_ButtonKeepExpiredBan);

            if (g_Cvar_ButtonGlobalEnable != null) _convars.RemoveChangeHook(g_Cvar_ButtonGlobalEnable, OnCvarChanged_ButtonGlobalEnable);
            if (g_Cvar_ButtonShowButton != null) _convars.RemoveChangeHook(g_Cvar_ButtonShowButton, OnCvarChanged_ButtonShowButton);
            if (g_Cvar_ButtonShowDoor != null) _convars.RemoveChangeHook(g_Cvar_ButtonShowDoor, OnCvarChanged_ButtonShowDoor);
            if (g_Cvar_ButtonShowPhysbox != null) _convars.RemoveChangeHook(g_Cvar_ButtonShowPhysbox, OnCvarChanged_ButtonShowPhysbox);
            if (g_Cvar_ButtonWatchButton != null) _convars.RemoveChangeHook(g_Cvar_ButtonWatchButton, OnCvarChanged_ButtonWatchButton);
            if (g_Cvar_ButtonWatchDoor != null) _convars.RemoveChangeHook(g_Cvar_ButtonWatchDoor, OnCvarChanged_ButtonWatchDoor);
            if (g_Cvar_ButtonWatchPhysbox != null) _convars.RemoveChangeHook(g_Cvar_ButtonWatchPhysbox, OnCvarChanged_ButtonWatchPhysbox);

            if (g_Cvar_TriggerBanTime != null) _convars.RemoveChangeHook(g_Cvar_TriggerBanTime, OnCvarChanged_TriggerBanTime);
            if (g_Cvar_TriggerBanLong != null) _convars.RemoveChangeHook(g_Cvar_TriggerBanLong, OnCvarChanged_TriggerBanLong);
            if (g_Cvar_TriggerBanReason != null) _convars.RemoveChangeHook(g_Cvar_TriggerBanReason, OnCvarChanged_TriggerBanReason);
            if (g_Cvar_TriggerUnBanReason != null) _convars.RemoveChangeHook(g_Cvar_TriggerUnBanReason, OnCvarChanged_TriggerUnBanReason);
            if (g_Cvar_TriggerKeepExpiredBan != null) _convars.RemoveChangeHook(g_Cvar_TriggerKeepExpiredBan, OnCvarChanged_TriggerKeepExpiredBan);

            if (g_Cvar_TriggerGlobalEnable != null) _convars.RemoveChangeHook(g_Cvar_TriggerGlobalEnable, OnCvarChanged_TriggerGlobalEnable);
            if (g_Cvar_TriggerShowOnce != null) _convars.RemoveChangeHook(g_Cvar_TriggerShowOnce, OnCvarChanged_TriggerShowOnce);
            if (g_Cvar_TriggerShowMultiple != null) _convars.RemoveChangeHook(g_Cvar_TriggerShowMultiple, OnCvarChanged_TriggerShowMultiple);
            if (g_Cvar_TriggerWatchOnce != null) _convars.RemoveChangeHook(g_Cvar_TriggerWatchOnce, OnCvarChanged_TriggerWatchOnce);
            if (g_Cvar_TriggerWatchMultiple != null) _convars.RemoveChangeHook(g_Cvar_TriggerWatchMultiple, OnCvarChanged_TriggerWatchMultiple);

            if (g_Cvar_OfflineClearTime != null) _convars.RemoveChangeHook(g_Cvar_OfflineClearTime, OnCvarChanged_OfflineClearTime);
            if (g_Cvar_PlayerFormat != null) _convars.RemoveChangeHook(g_Cvar_PlayerFormat, OnCvarChanged_PlayerFormat);

            if (g_Cvar_SchemeName != null) _convars.RemoveChangeHook(g_Cvar_SchemeName, OnCvarChanged_SchemeName);
            if (g_Cvar_LowerMapname != null) _convars.RemoveChangeHook(g_Cvar_LowerMapname, OnCvarChanged_LowerMapname);

            if (g_Cvar_ServerLanguage != null) _convars.RemoveChangeHook(g_Cvar_ServerLanguage, OnCvarChanged_ServerLanguage);
        }

        private void OnCvarChanged_ButtonBanTime(IConVar conVar)
        {
            int value = conVar.GetInt32();
            if (value >= 0 && value <= 43200) Cvar.ButtonBanTime = value;
            else Cvar.ButtonBanTime = 0;
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonBanTime.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonBanLong(IConVar conVar)
        {
            int value = conVar.GetInt32();
            if (value >= 1 && value <= 1440000) Cvar.ButtonBanLong = value;
            else Cvar.ButtonBanLong = 720;
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonBanLong.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonBanReason(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.ButtonBanReason = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.ButtonBanReason, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }

        private void OnCvarChanged_ButtonUnBanReason(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.ButtonUnBanReason = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.ButtonUnBanReason, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }

        private void OnCvarChanged_ButtonKeepExpiredBan(IConVar conVar)
        {
            Cvar.ButtonKeepExpiredBan = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonKeepExpiredBan.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonGlobalEnable(IConVar conVar)
        {
            Cvar.ButtonGlobalEnable = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonGlobalEnable.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonShowButton(IConVar conVar)
        {
            Cvar.ButtonShowButton = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonShowButton.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonShowDoor(IConVar conVar)
        {
            Cvar.ButtonShowDoor = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonShowDoor.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonShowPhysbox(IConVar conVar)
        {
            Cvar.ButtonShowPhysbox = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonShowPhysbox.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonWatchButton(IConVar conVar)
        {
            Cvar.ButtonWatchButton = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonWatchButton.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonWatchDoor(IConVar conVar)
        {
            Cvar.ButtonWatchDoor = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonWatchDoor.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ButtonWatchPhysbox(IConVar conVar)
        {
            Cvar.ButtonWatchPhysbox = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.ButtonWatchPhysbox.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerBanTime(IConVar conVar)
        {
            int value = conVar.GetInt32();
            if (value >= 0 && value <= 43200) Cvar.TriggerBanTime = value;
            else Cvar.TriggerBanTime = 0;
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerBanTime.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerBanLong(IConVar conVar)
        {
            int value = conVar.GetInt32();
            if (value >= 1 && value <= 1440000) Cvar.TriggerBanLong = value;
            else Cvar.TriggerBanLong = 720;
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerBanLong.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerBanReason(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.TriggerBanReason = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.TriggerBanReason, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }

        private void OnCvarChanged_TriggerUnBanReason(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.TriggerUnBanReason = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.TriggerUnBanReason, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }

        private void OnCvarChanged_TriggerKeepExpiredBan(IConVar conVar)
        {
            Cvar.TriggerKeepExpiredBan = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerKeepExpiredBan.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerGlobalEnable(IConVar conVar)
        {
            Cvar.TriggerGlobalEnable = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerGlobalEnable.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerShowOnce(IConVar conVar)
        {
            Cvar.TriggerShowOnce = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerShowOnce.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerShowMultiple(IConVar conVar)
        {
            Cvar.TriggerShowMultiple = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerShowMultiple.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerWatchOnce(IConVar conVar)
        {
            Cvar.TriggerWatchOnce = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerWatchOnce.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_TriggerWatchMultiple(IConVar conVar)
        {
            Cvar.TriggerWatchMultiple = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.TriggerWatchMultiple.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_OfflineClearTime(IConVar conVar)
        {
            int value = conVar.GetInt32();
            if (value >= 1 && value <= 240) Cvar.OfflineClearTime = value;
            else Cvar.OfflineClearTime = 30;
            UI.CvarChangeNotify(conVar.Name, Cvar.OfflineClearTime.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_PlayerFormat(IConVar conVar)
        {
            Cvar.PlayerFormat = conVar.GetInt16();
            UI.CvarChangeNotify(conVar.Name, Cvar.PlayerFormat.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_SchemeName(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.SchemeName = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.SchemeName, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }

        private void OnCvarChanged_LowerMapname(IConVar conVar)
        {
            Cvar.LowerMapname = conVar.GetBool();
            UI.CvarChangeNotify(conVar.Name, Cvar.LowerMapname.ToString(), conVar.Flags.HasFlag(ConVarFlags.Notify));
        }

        private void OnCvarChanged_ServerLanguage(IConVar conVar)
        {
            var value = conVar.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                Cvar.ServerLanguage = value.Replace("\"", "");
                UI.CvarChangeNotify(conVar.Name, Cvar.ServerLanguage, conVar.Flags.HasFlag(ConVarFlags.Notify));
            }
        }
    }
    static class Cvar
    {
        public static int ButtonBanTime = 0;
        public static int ButtonBanLong = 720;
        public static string ButtonBanReason = "Trolling";
        public static string ButtonUnBanReason = "Giving another chance";
        public static bool ButtonKeepExpiredBan = true;

        public static bool ButtonGlobalEnable = true;
        public static bool ButtonShowButton = true;
        public static bool ButtonShowDoor = true;
        public static bool ButtonShowPhysbox = true;
        public static bool ButtonWatchButton = true;
        public static bool ButtonWatchDoor = true;
        public static bool ButtonWatchPhysbox = true;

        public static int TriggerBanTime = 0;
        public static int TriggerBanLong = 720;
        public static string TriggerBanReason = "Trolling";
        public static string TriggerUnBanReason = "Giving another chance";
        public static bool TriggerKeepExpiredBan = true;

        public static bool TriggerGlobalEnable = true;
        public static bool TriggerShowOnce = true;
        public static bool TriggerShowMultiple = false;
        public static bool TriggerWatchOnce = true;
        public static bool TriggerWatchMultiple = false;

        public static int OfflineClearTime = 30;
        public static short PlayerFormat = 3;

        public static string SchemeName = "default.json";
        public static bool LowerMapname = false;

        public static string ServerLanguage = "en-us";
    }
}
