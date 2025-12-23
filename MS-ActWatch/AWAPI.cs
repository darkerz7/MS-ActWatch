using MS_ActWatch_Shared;
using Sharp.Shared.Objects;

namespace MS_ActWatch
{
    internal class AWAPI: IActWatchAPI
    {
        public void Native_ButtonWatch_IsClientBanned(string sSteamID)
        {
            if (!string.IsNullOrEmpty(sSteamID)) ActBan.ActBanPlayer.GetBan(sSteamID, GetBanAPI_Handler, true);
        }
        readonly ActBan.ActBanDB.GetBanAPIFunc GetBanAPI_Handler = (sClientSteamID, DBQuery_Result, bType) =>
        {
            if (DBQuery_Result.Count > 0)
            {
                SAWAPI_Ban target = new()
                {
                    bBanned = true,
                    sAdminName = DBQuery_Result[0][0],
                    sAdminSteamID = DBQuery_Result[0][1],
                    iDuration = Convert.ToInt32(DBQuery_Result[0][2]),
                    iTimeStamp_Issued = Convert.ToInt32(DBQuery_Result[0][3]),
                    sReason = DBQuery_Result[0][4],
                    sClientName = DBQuery_Result[0][5],
                    sClientSteamID = sClientSteamID
                };
                if (bType) AW.g_cAWAPI?.ButtonIsClientBannedResult(target);
                else AW.g_cAWAPI?.TriggerIsClientBannedResult(target);
                return;
            }
            if (bType) AW.g_cAWAPI?.ButtonIsClientBannedResult(new SAWAPI_Ban());
            else AW.g_cAWAPI?.TriggerIsClientBannedResult(new SAWAPI_Ban());
        };
        public void Native_ButtonWatch_BanClient(SAWAPI_Ban sawPlayer)
        {
            ActBan.ActBanDB.BanClient(sawPlayer.sClientName, sawPlayer.sClientSteamID, sawPlayer.sAdminName, sawPlayer.sAdminSteamID, sawPlayer.iDuration, sawPlayer.iTimeStamp_Issued, sawPlayer.sReason, true);
        }
        public void Native_ButtonWatch_UnbanClient(SAWAPI_Ban sawPlayer)
        {
            ActBan.ActBanDB.UnBanClient(sawPlayer.sClientSteamID, sawPlayer.sAdminName, sawPlayer.sAdminSteamID, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), sawPlayer.sReason, true);
        }
        public void Native_ButtonWatch_UpdateStatusBanClient(IGameClient Player)
        {
            ActBan.ActBanPlayer.GetBan(Player, true);
        }
        public void Native_TriggerWatch_IsClientBanned(string sSteamID)
        {
            if (!string.IsNullOrEmpty(sSteamID)) ActBan.ActBanPlayer.GetBan(sSteamID, GetBanAPI_Handler, false);
        }
        public void Native_TriggerWatch_BanClient(SAWAPI_Ban sawPlayer)
        {
            ActBan.ActBanDB.BanClient(sawPlayer.sClientName, sawPlayer.sClientSteamID, sawPlayer.sAdminName, sawPlayer.sAdminSteamID, sawPlayer.iDuration, sawPlayer.iTimeStamp_Issued, sawPlayer.sReason, false);
        }
        public void Native_TriggerWatch_UnbanClient(SAWAPI_Ban sawPlayer)
        {
            ActBan.ActBanDB.UnBanClient(sawPlayer.sClientSteamID, sawPlayer.sAdminName, sawPlayer.sAdminSteamID, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), sawPlayer.sReason, false);
        }
        public void Native_TriggerWatch_UpdateStatusBanClient(IGameClient Player)
        {
            ActBan.ActBanPlayer.GetBan(Player, false);
        }
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_OnClientBanned? Forward_ButtonWatch_OnClientBanned;
        public void ButtonOnClientBanned(SAWAPI_Ban sawPlayer) => Forward_ButtonWatch_OnClientBanned?.Invoke(sawPlayer);
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_OnClientUnbanned? Forward_ButtonWatch_OnClientUnbanned;
        public void ButtonOnClientUnbanned(SAWAPI_Ban sawPlayer) => Forward_ButtonWatch_OnClientUnbanned?.Invoke(sawPlayer);
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_OnDoorPressed? Forward_ButtonWatch_OnDoorPressed;
        public void ButtonOnDoorPressed(IGameClient Player, string sDoorName, string sDoorID) => Forward_ButtonWatch_OnDoorPressed?.Invoke(Player, sDoorName, sDoorID);
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_OnButtonPressed? Forward_ButtonWatch_OnButtonPressed;
        public void ButtonOnButtonPressed(IGameClient Player, string sButtonName, string sButtonID) => Forward_ButtonWatch_OnButtonPressed?.Invoke(Player, sButtonName, sButtonID);
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_OnPhysboxPressed? Forward_ButtonWatch_OnPhysboxPressed;
        public void ButtonOnPhysboxPressed(IGameClient Player, string sPhysboxName, string sPhysboxID) => Forward_ButtonWatch_OnPhysboxPressed?.Invoke(Player, sPhysboxName, sPhysboxID);
        //===================================================================================================
        public event IActWatchAPI.Forward_BW_IsClientBannedResult? Forward_ButtonWatch_IsClientBannedResult;
        public void ButtonIsClientBannedResult(SAWAPI_Ban sawPlayer) => Forward_ButtonWatch_IsClientBannedResult?.Invoke(sawPlayer);
        //===================================================================================================
        public event IActWatchAPI.Forward_TW_OnClientBanned? Forward_TriggerWatch_OnClientBanned;
        public void TriggerOnClientBanned(SAWAPI_Ban sawPlayer) => Forward_TriggerWatch_OnClientBanned?.Invoke(sawPlayer);
        //===================================================================================================
        public event IActWatchAPI.Forward_TW_OnClientUnbanned? Forward_TriggerWatch_OnClientUnbanned;
        public void TriggerOnClientUnbanned(SAWAPI_Ban sawPlayer) => Forward_TriggerWatch_OnClientUnbanned?.Invoke(sawPlayer);
        //===================================================================================================
        public event IActWatchAPI.Forward_TW_OnTriggerOnceTouch? Forward_TriggerWatch_OnTriggerOnceTouch;
        public void TriggerOnTriggerOnceTouch(IGameClient Player, string sTriggerName, string sTriggerID) => Forward_TriggerWatch_OnTriggerOnceTouch?.Invoke(Player, sTriggerName, sTriggerID);
        //===================================================================================================
        public event IActWatchAPI.Forward_TW_OnTriggerMultipleTouch? Forward_TriggerWatch_OnTriggerMultipleTouch;
        public void TriggerOnTriggerMultipleTouch(IGameClient Player, string sTriggerName, string sTriggerID) => Forward_TriggerWatch_OnTriggerMultipleTouch?.Invoke(Player, sTriggerName, sTriggerID);
        //===================================================================================================
        public event IActWatchAPI.Forward_TW_IsClientBannedResult? Forward_TriggerWatch_IsClientBannedResult;
        public void TriggerIsClientBannedResult(SAWAPI_Ban sawPlayer) => Forward_TriggerWatch_IsClientBannedResult?.Invoke(sawPlayer);
        //===================================================================================================
    }
}
