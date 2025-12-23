using MS_ActWatch.Helpers;
using MS_ActWatch_Shared;
using Sharp.Shared.Objects;

namespace MS_ActWatch.ActBan
{
    internal class ActBanPlayer(bool b)
    {
        public bool bBanned = false;
        public bool bTriggerBanned = false;

        public string sAdminName = "";
        public string sAdminSteamID = "";
        public int iDuration;
        public int iTimeStamp_Issued;
        public string sReason = "";

        public string sClientName = "";
        public string sClientSteamID = "";

        private readonly bool bType = b;

        public bool SetBan(string sBanAdminName, string sBanAdminSteamID, string sBanClientName, string sBanClientSteamID, int iBanDuration, string sBanReason)
        {
            if (!string.IsNullOrEmpty(sBanClientSteamID))
            {
                bBanned = true;
                if (!bType) bTriggerBanned = true;
                sAdminName = sBanAdminName;
                sAdminSteamID = sBanAdminSteamID;
                sReason = sBanReason;
                if (iBanDuration < -1)
                {
                    iDuration = -1;
                    iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    return true;
                }
                else if (iBanDuration == 0)
                {
                    iDuration = 0;
                    iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                }
                else
                {
                    iDuration = iBanDuration;
                    iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + iDuration * 60;
                }
                SAWAPI_Ban apiBan = new()
                {
                    bBanned = bBanned,
                    sAdminName = sAdminName,
                    sAdminSteamID = sAdminSteamID,
                    iDuration = iDuration,
                    iTimeStamp_Issued = iTimeStamp_Issued,
                    sReason = sReason,
                    sClientName = sClientName,
                    sClientSteamID = sClientSteamID
                };
                if (bType) AW.g_cAWAPI.ButtonOnClientBanned(apiBan);
                else AW.g_cAWAPI.TriggerOnClientBanned(apiBan);
                ActBanDB.BanClient(sBanClientName, sBanClientSteamID, sAdminName, sAdminSteamID, iDuration, iTimeStamp_Issued, sReason, bType);
                return true;
            }
            return false;
        }

        public bool UnBan(string sUnBanAdminName, string sUnBanAdminSteamID, string sUnBanClientSteamID, string sUnbanReason)
        {
            if (!string.IsNullOrEmpty(sUnBanClientSteamID))
            {
                bBanned = false;
                if (string.IsNullOrEmpty(sUnbanReason)) sUnbanReason = "Amnesty";
                SAWAPI_Ban apiBan = new()
                {
                    bBanned = bBanned,
                    sAdminName = sUnBanAdminName,
                    sAdminSteamID = sUnBanAdminSteamID,
                    iDuration = 0,
                    iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                    sReason = sUnbanReason,
                    sClientName = "",
                    sClientSteamID = sUnBanClientSteamID
                };
                if (bType) AW.g_cAWAPI.ButtonOnClientUnbanned(apiBan);
                else AW.g_cAWAPI.TriggerOnClientUnbanned(apiBan);
                ActBanDB.UnBanClient(sUnBanClientSteamID, sUnBanAdminName, sUnBanAdminSteamID, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), sUnbanReason, bType);
                return true;
            }
            return false;
        }

        public static void GetBan(IGameClient player, bool bType, bool bShow = false)
        {
            ActBanDB.GetBan(player, GetBanPlayer_Handler, bType, bShow);
        }
        static readonly ActBanDB.GetBanPlayerFunc GetBanPlayer_Handler = (player, DBQuery_Result, bType, bShow) =>
        {
            if (player.IsValid && AW.CheckDictionary(player))
            {
                ActBanPlayer dActBan = bType ? AW.g_AWPlayer[player].ButtonBannedPlayer : AW.g_AWPlayer[player].TriggerBannedPlayer;
                if (DBQuery_Result.Count > 0)
                {
                    dActBan.bBanned = true;
                    if (!bType) dActBan.bTriggerBanned = true;
                    dActBan.sAdminName = DBQuery_Result[0][0];
                    dActBan.sAdminSteamID = DBQuery_Result[0][1];
                    dActBan.iDuration = Convert.ToInt32(DBQuery_Result[0][2]);
                    dActBan.iTimeStamp_Issued = Convert.ToInt32(DBQuery_Result[0][3]);
                    dActBan.sReason = DBQuery_Result[0][4];
                    if (bShow)
                    {
                        UI.AWSysInfo(bType ? "ActWatch.Info.Buttons.PlayerConnect" : "ActWatch.Info.Triggers.PlayerConnect" , 4, UI.ReplaceColorTags(UI.PlayerInfoFormat(player)[3], false), dActBan.iDuration, dActBan.iTimeStamp_Issued, UI.ReplaceColorTags(UI.PlayerInfoFormat(dActBan.sAdminName, dActBan.sAdminSteamID)[3], false), dActBan.sReason);
                    }
                }
                else dActBan.bBanned = false;
            }
        };
        public static void GetBan(string sClientSteamID, IGameClient? admin, string reason, bool bConsole, ActBanDB.GetBanCommFunc handler, bool bType)
        {
            ActBanDB.GetBan(sClientSteamID, admin, reason, bConsole, handler, bType);
        }
        public static void GetBan(string sClientSteamID, ActBanDB.GetBanAPIFunc handler, bool bType)
        {
            ActBanDB.GetBan(sClientSteamID, handler, bType);
        }
    }
}
