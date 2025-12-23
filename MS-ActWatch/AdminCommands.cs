using MS_ActWatch.ActBan;
using MS_ActWatch.Helpers;
using Sharp.Shared.Enums;
using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using static MS_ActWatch.ActBan.ActBanDB;

namespace MS_ActWatch
{
    public partial class ActWatch
    {
        void RegAdminCommands()
        {
            AdminCmdsManager.InstallCommandCallback("areloadwl", "aw_reload", OnAWReload_WhiteList);
            AdminCmdsManager.InstallCommandCallback("ashowwl", "aw_reload", OnAWShow_WhiteList);

            AdminCmdsManager.InstallCommandCallback("areloadscheme", "aw_reload", OnAWReload_Scheme);
            AdminCmdsManager.InstallCommandCallback("ashowscheme", "aw_reload", OnAWShow_Scheme);

            AdminCmdsManager.InstallCommandCallback("bban", "bw_ban", OnBWBan);
            AdminCmdsManager.InstallCommandCallback("bunban", "bw_unban", OnBWUnBan);
            AdminCmdsManager.InstallCommandCallback("bbanlist", "bw_ban", OnBWBanList);

            AdminCmdsManager.InstallCommandCallback("trban", "tw_ban", OnTWBan);
            AdminCmdsManager.InstallCommandCallback("trunban", "tw_unban", OnTWUnBan);
            AdminCmdsManager.InstallCommandCallback("trbanlist", "tw_ban", OnTWBanList);

            AdminCmdsManager.InstallCommandCallback("alist", "aw_ban", OnAWList);
        }

        private ECommandAction OnAWReload_WhiteList(IGameClient client, StringCommand command)
        {
            if (!client.IsValid) return ECommandAction.Stopped;

            if (AW.g_WhiteList != null)
            {
                AW.g_WhiteList.Buttons.Clear();
                AW.g_WhiteList.Triggers.Clear();
                AW.g_WhiteList = null;
            }
            AW.LoadWhiteList();
            UI.ReplyToCommand(client, "ActWatch.Reply.Reload_WhiteList", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "");

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWShow_WhiteList(IGameClient client, StringCommand command)
        {
            if (!client.IsValid) return ECommandAction.Stopped;
            if (AW.g_WhiteList != null)
            {
                if (AW.g_WhiteList.Buttons.Count > 0) foreach(var buttonid in AW.g_WhiteList.Buttons) UI.ReplyToCommand(client, "ActWatch.Reply.ShowWhiteList", command.ChatTrigger, 0, AW.g_Scheme != null ? AW.g_Scheme.Color_use_button : "", buttonid);
                else UI.ReplyToCommand(client, "ActWatch.Reply.ShowWhiteList", command.ChatTrigger, 0, AW.g_Scheme != null ? AW.g_Scheme.Color_use_button : "", "null");

                if (AW.g_WhiteList.Triggers.Count > 0) foreach (var triggerid in AW.g_WhiteList.Triggers) UI.ReplyToCommand(client, "ActWatch.Reply.ShowWhiteList", command.ChatTrigger, 1, AW.g_Scheme != null ? AW.g_Scheme.Color_use_trigger : "", triggerid);
                else UI.ReplyToCommand(client, "ActWatch.Reply.ShowWhiteList", command.ChatTrigger, 1, AW.g_Scheme != null ? AW.g_Scheme.Color_use_trigger : "", "null");
            } else UI.ReplyToCommand(client, "ActWatch.Reply.WhiteList_NotFound", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "");

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWReload_Scheme(IGameClient client, StringCommand command)
        {
            if (!client.IsValid) return ECommandAction.Stopped;

            AW.LoadScheme();
            UI.ReplyToCommand(client, "ActWatch.Reply.Reload_Scheme", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "");

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWShow_Scheme(IGameClient client, StringCommand command)
        {
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_name", AW.g_Scheme.Color_name, UI.ReplaceSpecial(AW.g_Scheme.Color_name));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_steamid", AW.g_Scheme.Color_steamid, UI.ReplaceSpecial(AW.g_Scheme.Color_steamid));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_warning", AW.g_Scheme.Color_warning, UI.ReplaceSpecial(AW.g_Scheme.Color_warning));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_enabled", AW.g_Scheme.Color_enabled, UI.ReplaceSpecial(AW.g_Scheme.Color_enabled));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_disabled", AW.g_Scheme.Color_disabled, UI.ReplaceSpecial(AW.g_Scheme.Color_disabled));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_use_trigger", AW.g_Scheme.Color_use_trigger, UI.ReplaceSpecial(AW.g_Scheme.Color_use_trigger));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_use_button", AW.g_Scheme.Color_use_button, UI.ReplaceSpecial(AW.g_Scheme.Color_use_button));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_entity_name", AW.g_Scheme.Color_entity_name, UI.ReplaceSpecial(AW.g_Scheme.Color_entity_name));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Color_entity_id", AW.g_Scheme.Color_entity_id, UI.ReplaceSpecial(AW.g_Scheme.Color_entity_id));
            UI.ReplyToCommand(client, "ActWatch.Reply.ShowScheme", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, "Server_name", AW.g_Scheme.Color_warning, UI.ReplaceSpecial(AW.g_Scheme.Server_name));

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWBan(IGameClient client, StringCommand command)
        {
            if (!Cvar.ButtonGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            int iArgNeed = 1;
            string sArgHelper = "<#userid|name|#steamid> [<time>] [<reason>]";
            if (command.ArgCount < iArgNeed)
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.MinArg", command.ChatTrigger, 2, iArgNeed, command.CommandName, sArgHelper);
                return ECommandAction.Stopped;
            }

            var players = TargetManager.Find(client, command.GetArg(1));

            OfflineBan? target = null;

            if (players.Count > 0)
            {
                //One Target
                IGameClient clientOnline = players.Single();
                if (AW.GetPlayerImmunity(client) < AW.GetPlayerImmunity(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.You_cannot_target", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }

                if (!AW.CheckDictionary(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                    return ECommandAction.Stopped;
                }

                if (AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.bBanned)
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Buttons.Has_a_ban", command.ChatTrigger, 0, UI.PlayerInfo(client, UI.PlayerInfoFormat(clientOnline)), AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }
                foreach (OfflineBan OfflineTest in AW.g_OfflinePlayer.ToList())
                {
                    if (OfflineTest.UserID == clientOnline.UserId)
                    {
                        target = OfflineTest;
                        break;
                    }
                }
            }
            else
            {
                target = OfflineFunc.FindTarget(client, command.GetArg(1), command.ChatTrigger);
            }

            if (target == null)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            int time = Cvar.ButtonBanTime;
            if (command.ArgCount >= 2)
            {
                if (!int.TryParse(command.GetArg(2), out int timeparse))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Must_be_an_integer", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);
                    return ECommandAction.Stopped;
                }
                time = timeparse;
            }

            if (time == 0 && !AW.CheckPermission(client, "bw_ban_perm"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Permanent", command.ChatTrigger, 0, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            if (time > Cvar.ButtonBanLong && !AW.CheckPermission(client, "bw_ban_long"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Long", command.ChatTrigger, 0,AW.g_Scheme.Color_warning, Cvar.ButtonBanLong);
                return ECommandAction.Stopped;
            }

            string reason = command.ArgCount >= 3 ? command.GetArg(3) : "";
            if (string.IsNullOrEmpty(reason)) reason = Cvar.ButtonBanReason;

            ActBanPlayer banPlayer = (target.Online && target.Player != null) ? AW.g_AWPlayer[target.Player].ButtonBannedPlayer : new ActBanPlayer(true);

            string? sSteamID = AW.ConvertSteamID64ToSteamID(client.SteamId.ToString());

            if (banPlayer.SetBan(UI.ReplaceSpecial(client.Name), !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER", UI.ReplaceSpecial(target.Name), target.SteamID, time, reason))
                UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Success", command.ChatTrigger, 0, AW.g_Scheme.Color_warning);
            else
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Failed", command.ChatTrigger, 0, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            UI.AWChatAdminBan(UI.PlayerInfoFormat(client), target.Online && target.Player != null ? UI.PlayerInfoFormat(target.Player) : UI.PlayerInfoFormat(target.Name, target.SteamID), reason, true, true);

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWUnBan(IGameClient client, StringCommand command)
        {
            if (!Cvar.ButtonGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            int iArgNeed = 1;
            string sArgHelper = "<#userid|name|#steamid> [<reason>]";
            if (command.ArgCount < iArgNeed)
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.MinArg", command.ChatTrigger, 2, iArgNeed, command.CommandName, sArgHelper);
                return ECommandAction.Stopped;
            }

            var players = TargetManager.Find(client, command.GetArg(1));

            ActBanPlayer target = new(true);
            string sTarget = command.GetArg(1);

            bool bOnline = players.Count > 0;

            if (bOnline)
            {
                IGameClient clientOnline = players.Single();

                if (AW.GetPlayerImmunity(client) < AW.GetPlayerImmunity(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.You_cannot_target", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }

                if (!AW.CheckDictionary(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                    return ECommandAction.Stopped;
                }

                target.bBanned = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.bBanned;
                target.sAdminName = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.sAdminSteamID;
                target.sAdminSteamID = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.sAdminSteamID;
                target.iDuration = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.iDuration;
                target.iTimeStamp_Issued = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.iTimeStamp_Issued;
                target.sReason = AW.g_AWPlayer[clientOnline].ButtonBannedPlayer.sReason;
                target.sClientName = UI.ReplaceSpecial(clientOnline.Name);
                string? sSteamID = AW.ConvertSteamID64ToSteamID(clientOnline.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.InvalidSteamID", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled, AW.g_Scheme.Color_name, UI.ReplaceSpecial(clientOnline.Name));
                    return ECommandAction.Stopped;
                }
                target.sClientSteamID = sSteamID;
            }

            string reason = command.ArgCount >= 2 ? command.GetArg(2) : "";
            if (string.IsNullOrEmpty(reason)) reason = Cvar.ButtonUnBanReason;

            if (bOnline) UnBanButtonsComm(client, players.Single(), target, reason, command.ChatTrigger);
            else if (sTarget.StartsWith("#steam_", StringComparison.OrdinalIgnoreCase))
            {
                ActBanPlayer.GetBan(sTarget[1..], client, reason, command.ChatTrigger, GetBanComm_Handler, true);
            }
            else UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);

            return ECommandAction.Stopped;
        }

        readonly GetBanCommFunc GetBanComm_Handler = (sClientSteamID, client, reason, bChat, DBQuery_Result, bType) =>
        {
            if (DBQuery_Result.Count > 0)
            {
                ActBanPlayer target = new(bType)
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
                if (bType) UnBanButtonsComm(client, null, target, reason, bChat);
                else UnBanTriggersComm(client, null, target, reason, bChat);
                return;
            }
            if (bType) UnBanButtonsComm(client, null, null, reason, bChat);
            else UnBanTriggersComm(client, null, null, reason, bChat);
        };

        static void UnBanButtonsComm(IGameClient client, IGameClient? player, ActBanPlayer? target, string reason, bool bChat)
        {
            if (AW.g_Scheme == null) return;
            if (target == null)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", bChat, 2, AW.g_Scheme.Color_warning);
                return;
            }

            if (!target.bBanned)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Buttons.Can_use", bChat, 0, UI.PlayerInfo(client, UI.PlayerInfoFormat(target.sClientName, target.sClientSteamID)), AW.g_Scheme.Color_enabled);
                return;
            }

            if (target.iDuration == 0 && !AW.CheckPermission(client, "bw_unban_perm"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.UnPermanent", bChat, 0, AW.g_Scheme.Color_warning);
                return;
            }

            string? sSteamID = AW.ConvertSteamID64ToSteamID(client.SteamId.ToString());
            if (!string.Equals(target.sAdminSteamID, !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER") && AW.CheckPermission(client, "bw_unban_other"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Other", bChat, 0, AW.g_Scheme.Color_warning);
                return;
            }

            if (target.UnBan(UI.ReplaceSpecial(client.Name), !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER", target.sClientSteamID, reason))
            {
                if (player != null) AW.g_AWPlayer[player].ButtonBannedPlayer.bBanned = false;
                UI.ReplyToCommand(client, "ActWatch.Reply.UnBan.Success", bChat, 0, AW.g_Scheme.Color_warning);
            }
            else
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.UnBan.Failed", bChat, 0, AW.g_Scheme.Color_warning);
                return;
            }

            UI.AWChatAdminBan(UI.PlayerInfoFormat(client), UI.PlayerInfoFormat(target.sClientName, target.sClientSteamID), reason, false, true);
        }

        private ECommandAction OnBWBanList(IGameClient client, StringCommand command)
        {
            if (!Cvar.ButtonGlobalEnable) return ECommandAction.Stopped;
            return OnBanList(client, command, true);
        }

        private ECommandAction OnTWBan(IGameClient client, StringCommand command)
        {
            if (!Cvar.TriggerGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            int iArgNeed = 1;
            string sArgHelper = "<#userid|name|#steamid> [<time>] [<reason>]";
            if (command.ArgCount < iArgNeed)
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.MinArg", command.ChatTrigger, 2, iArgNeed, command.CommandName, sArgHelper);
                return ECommandAction.Stopped;
            }

            var players = TargetManager.Find(client, command.GetArg(1));

            OfflineBan? target = null;

            if (players.Count > 0)
            {
                //One Target
                IGameClient clientOnline = players.Single();
                if (AW.GetPlayerImmunity(client) < AW.GetPlayerImmunity(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.You_cannot_target", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }

                if (!AW.CheckDictionary(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                    return ECommandAction.Stopped;
                }

                if (AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.bBanned)
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Triggers.Has_a_ban", command.ChatTrigger, 1, UI.PlayerInfo(client, UI.PlayerInfoFormat(clientOnline)), AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }
                foreach (OfflineBan OfflineTest in AW.g_OfflinePlayer.ToList())
                {
                    if (OfflineTest.UserID == clientOnline.UserId)
                    {
                        target = OfflineTest;
                        break;
                    }
                }
            }
            else
            {
                target = OfflineFunc.FindTarget(client, command.GetArg(1), command.ChatTrigger);
            }

            if (target == null)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            int time = Cvar.TriggerBanTime;
            if (command.ArgCount >= 2)
            {
                if (!int.TryParse(command.GetArg(2), out int timeparse))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Must_be_an_integer", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);
                    return ECommandAction.Stopped;
                }
                time = timeparse;
            }

            if (time == 0 && !AW.CheckPermission(client, "tw_ban_perm"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Permanent", command.ChatTrigger, 1, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            if (time > Cvar.TriggerBanLong && !AW.CheckPermission(client, "tw_ban_long"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Long", command.ChatTrigger, 1, AW.g_Scheme.Color_warning, Cvar.TriggerBanLong);
                return ECommandAction.Stopped;
            }

            string reason = command.ArgCount >= 3 ? command.GetArg(3) : "";
            if (string.IsNullOrEmpty(reason)) reason = Cvar.TriggerBanReason;

            ActBanPlayer banPlayer = (target.Online && target.Player != null) ? AW.g_AWPlayer[target.Player].TriggerBannedPlayer : new ActBanPlayer(false);

            string? sSteamID = AW.ConvertSteamID64ToSteamID(client.SteamId.ToString());

            if (banPlayer.SetBan(UI.ReplaceSpecial(client.Name), !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER", UI.ReplaceSpecial(target.Name), target.SteamID, time, reason))
                UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Success", command.ChatTrigger, 1, AW.g_Scheme.Color_warning);
            else
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Failed", command.ChatTrigger, 1, AW.g_Scheme.Color_warning);
                return ECommandAction.Stopped;
            }

            UI.AWChatAdminBan(UI.PlayerInfoFormat(client), target.Online && target.Player != null ? UI.PlayerInfoFormat(target.Player) : UI.PlayerInfoFormat(target.Name, target.SteamID), reason, true, false);

            return ECommandAction.Stopped;
        }

        private ECommandAction OnTWUnBan(IGameClient client, StringCommand command)
        {
            if (!Cvar.TriggerGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            int iArgNeed = 1;
            string sArgHelper = "<#userid|name|#steamid> [<reason>]";
            if (command.ArgCount < iArgNeed)
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.MinArg", command.ChatTrigger, 2, iArgNeed, command.CommandName, sArgHelper);
                return ECommandAction.Stopped;
            }

            var players = TargetManager.Find(client, command.GetArg(1));

            ActBanPlayer target = new(false);
            string sTarget = command.GetArg(1);

            bool bOnline = players.Count > 0;

            if (bOnline)
            {
                IGameClient clientOnline = players.Single();

                if (AW.GetPlayerImmunity(client) < AW.GetPlayerImmunity(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.You_cannot_target", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled);
                    return ECommandAction.Stopped;
                }

                if (!AW.CheckDictionary(clientOnline))
                {
                    UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                    return ECommandAction.Stopped;
                }

                target.bBanned = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.bBanned;
                target.sAdminName = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.sAdminSteamID;
                target.sAdminSteamID = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.sAdminSteamID;
                target.iDuration = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.iDuration;
                target.iTimeStamp_Issued = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.iTimeStamp_Issued;
                target.sReason = AW.g_AWPlayer[clientOnline].TriggerBannedPlayer.sReason;
                target.sClientName = UI.ReplaceSpecial(clientOnline.Name);
                string? sSteamID = AW.ConvertSteamID64ToSteamID(clientOnline.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.InvalidSteamID", command.ChatTrigger, 2, AW.g_Scheme.Color_disabled, AW.g_Scheme.Color_name, UI.ReplaceSpecial(clientOnline.Name));
                    return ECommandAction.Stopped;
                }
                target.sClientSteamID = sSteamID;
            }

            string reason = command.ArgCount >= 2 ? command.GetArg(2) : "";
            if (string.IsNullOrEmpty(reason)) reason = Cvar.TriggerUnBanReason;

            if (bOnline) UnBanTriggersComm(client, players.Single(), target, reason, command.ChatTrigger);
            else if (sTarget.StartsWith("#steam_", StringComparison.OrdinalIgnoreCase))
            {
                ActBanPlayer.GetBan(sTarget[1..], client, reason, command.ChatTrigger, GetBanComm_Handler, false);
            }
            else UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);

            return ECommandAction.Stopped;
        }

        static void UnBanTriggersComm(IGameClient client, IGameClient? player, ActBanPlayer? target, string reason, bool bChat)
        {
            if (AW.g_Scheme == null) return;
            if (target == null)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", bChat, 2, AW.g_Scheme.Color_warning);
                return;
            }

            if (!target.bBanned)
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Triggers.Can_touch", bChat, 1, UI.PlayerInfo(client, UI.PlayerInfoFormat(target.sClientName, target.sClientSteamID)), AW.g_Scheme.Color_enabled);
                return;
            }

            if (target.iDuration == 0 && !AW.CheckPermission(client, "tw_unban_perm"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.UnPermanent", bChat, 1, AW.g_Scheme.Color_warning);
                return;
            }

            string? sSteamID = AW.ConvertSteamID64ToSteamID(client.SteamId.ToString());
            if (!string.Equals(target.sAdminSteamID, !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER") && AW.CheckPermission(client, "tw_unban_other"))
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.Access.Other", bChat, 1, AW.g_Scheme.Color_warning);
                return;
            }

            if (target.UnBan(UI.ReplaceSpecial(client.Name), !string.IsNullOrEmpty(sSteamID) ? sSteamID : "SERVER", target.sClientSteamID, reason))
            {
                if (player != null) AW.g_AWPlayer[player].TriggerBannedPlayer.bBanned = false;
                UI.ReplyToCommand(client, "ActWatch.Reply.UnBan.Success", bChat, 1, AW.g_Scheme.Color_warning);
            }
            else
            {
                UI.ReplyToCommand(client, "ActWatch.Reply.UnBan.Failed", bChat, 1, AW.g_Scheme.Color_warning);
                return;
            }

            UI.AWChatAdminBan(UI.PlayerInfoFormat(client), UI.PlayerInfoFormat(target.sClientName, target.sClientSteamID), reason, false, false);
        }

        private ECommandAction OnTWBanList(IGameClient client, StringCommand command)
        {
            if (!Cvar.TriggerGlobalEnable) return ECommandAction.Stopped;
            return OnBanList(client, command, false);
        }

        private static ECommandAction OnBanList(IGameClient client, StringCommand command, bool bType)
        {
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            UI.ReplyToCommand(client, bType ? "ActWatch.Reply.Buttons.List" : "ActWatch.Reply.Triggers.List", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning);

            int iCount = 0;
            foreach (var pair in AW.g_AWPlayer.ToList())
            {
                ActBanPlayer bannedPlayer = bType ? pair.Value.ButtonBannedPlayer : pair.Value.TriggerBannedPlayer;
                if (bannedPlayer.bBanned)
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Player", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, UI.PlayerInfo(client, UI.PlayerInfoFormat(pair.Key)));
                    UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Admin", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, UI.PlayerInfo(client, UI.PlayerInfoFormat(bannedPlayer.sAdminName, bannedPlayer.sAdminSteamID)));
                    switch (bannedPlayer.iDuration)
                    {
                        case -1: UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Duration.Temporary", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_enabled); break;
                        case 0: UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Duration.Permanently", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_disabled); break;
                        default: UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Duration.Minutes", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_disabled, bannedPlayer.iDuration); break;
                    }
                    UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Expires", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_disabled, DateTimeOffset.FromUnixTimeSeconds(bannedPlayer.iTimeStamp_Issued));
                    UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Reason", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_disabled, bannedPlayer.sReason);
                    UI.ReplyToCommand(client, "ActWatch.Reply.Ban.Separator", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning);
                    iCount++;
                }
            }
            if (iCount == 0) UI.ReplyToCommand(client, "ActWatch.Reply.Ban.NoPlayers", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning);

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWList(IGameClient client, StringCommand command)
        {
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            UI.ReplyToCommand(client, "ActWatch.Reply.Offline.Info", command.ChatTrigger, 2, AW.g_Scheme.Color_warning);

            int iCount = 0;
            double CurrentTime = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            foreach (OfflineBan OfflineTest in AW.g_OfflinePlayer.ToList())
            {
                iCount++;
                if (OfflineTest.Online)
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Offline.OnServer", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, iCount, OfflineTest.Name, OfflineTest.UserID, OfflineTest.SteamID);
                }
                else
                {
                    UI.ReplyToCommand(client, "ActWatch.Reply.Offline.Leave", command.ChatTrigger, 2, AW.g_Scheme.Color_warning, iCount, OfflineTest.Name, OfflineTest.UserID, OfflineTest.SteamID, (int)((CurrentTime - OfflineTest.TimeStamp_Start) / 60));
                }
            }

            return ECommandAction.Stopped;
        }
    }
}
