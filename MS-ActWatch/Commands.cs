using MS_ActWatch.ActBan;
using MS_ActWatch.Helpers;
using Sharp.Shared.Enums;
using Sharp.Shared.Objects;
using Sharp.Shared.Types;

namespace MS_ActWatch
{
    public partial class ActWatch
    {
        void RegCommands()
        {
            _clients!.InstallCommandCallback("apf", OnAWChangePlayerFormat);
            _clients!.InstallCommandCallback("buttons", OnAWChangeButtonsWatch);
            _clients!.InstallCommandCallback("triggers", OnAWChangeTriggersWatch);
            _clients!.InstallCommandCallback("bstatus", OnBWStatus);
            _clients!.InstallCommandCallback("trstatus", OnTWStatus);
        }

        void UnRegCommands()
        {
            _clients!.RemoveCommandCallback("apf", OnAWChangePlayerFormat);
            _clients!.RemoveCommandCallback("buttons", OnAWChangeButtonsWatch);
            _clients!.RemoveCommandCallback("triggers", OnAWChangeTriggersWatch);
            _clients!.RemoveCommandCallback("bstatus", OnBWStatus);
            _clients!.RemoveCommandCallback("trstatus", OnTWStatus);
        }

        private ECommandAction OnAWChangePlayerFormat(IGameClient client, StringCommand command)
        {
            if (!client.IsValid) return ECommandAction.Stopped;

            int iArgNeed = 1;
            string sArgHelper = "[number] (default: 3; min 0; max 3)";
            if (command.ArgCount < iArgNeed)
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.MinArg", command.ChatTrigger, 2, iArgNeed, command.CommandName, sArgHelper);
                return ECommandAction.Stopped;
            }

            if (!AW.CheckDictionary(client))
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                return ECommandAction.Stopped;
            }

            if (!Int32.TryParse(command.GetArg(1), out int number)) number = 3;
            if (number >= 0 && number <= 3)
            {
                AW.g_AWPlayer[client].PFormatPlayer = number;

                if (GetClientPrefs() is { } cp && cp.IsLoaded(client.SteamId))
                {
                    cp.SetCookie(client.SteamId, "AW_PInfo_Format", number.ToString());
                }

                switch (number)
                {
                    case 1: UI.ReplyToCommand(client, "ActWatch.Reply.PlayerInfo.UserID", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : ""); break;
                    case 2: UI.ReplyToCommand(client, "ActWatch.Reply.PlayerInfo.SteamID", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : ""); break;
                    case 3: UI.ReplyToCommand(client, "ActWatch.Reply.PlayerInfo.Full", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : ""); break;
                    default: UI.ReplyToCommand(client, "ActWatch.Reply.PlayerInfo.NicknameOnly", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : ""); break;
                }
            }
            else UI.ReplyToCommand(client, "ActWatch.Reply.NotValid", command.ChatTrigger, 2, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "");

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWChangeButtonsWatch(IGameClient client, StringCommand command)
        {
            if (!Cvar.ButtonGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid) return ECommandAction.Stopped;

            if (!AW.CheckDictionary(client))
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                return ECommandAction.Stopped;
            }

            bool bNewValue = AW.g_AWPlayer[client].Buttons;

            string? sValue = command.ArgCount > 0 ? command.GetArg(1) : null;
            if (!string.IsNullOrEmpty(sValue))
            {
                bNewValue = sValue.Contains("true", StringComparison.OrdinalIgnoreCase) || string.Equals(sValue, "1");
            }
            else bNewValue = !bNewValue;

            AW.g_AWPlayer[client].Buttons = bNewValue;

            if (bNewValue)
            {
                if (GetClientPrefs() is { } cp && cp.IsLoaded(client.SteamId))
                {
                    cp.SetCookie(client.SteamId, "AW_Buttons", "1");
                }
                UI.ReplyToCommand(client, "ActWatch.Reply.Button.Enabled", command.ChatTrigger, 0, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : "");
            }
            else
            {
                if (GetClientPrefs() is { } cp && cp.IsLoaded(client.SteamId))
                {
                    cp.SetCookie(client.SteamId, "AW_Buttons", "0");
                }
                UI.ReplyToCommand(client, "ActWatch.Reply.Button.Disabled", command.ChatTrigger, 0, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_disabled : "");
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnAWChangeTriggersWatch(IGameClient client, StringCommand command)
        {
            if (!Cvar.TriggerGlobalEnable) return ECommandAction.Stopped;
            if (!client.IsValid) return ECommandAction.Stopped;

            if (!AW.CheckDictionary(client))
            {
                UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                return ECommandAction.Stopped;
            }

            bool bNewValue = AW.g_AWPlayer[client].Triggers;

            string? sValue = command.ArgCount > 0 ? command.GetArg(1) : null;
            if (!string.IsNullOrEmpty(sValue))
            {
                bNewValue = sValue.Contains("true", StringComparison.OrdinalIgnoreCase) || string.Equals(sValue, "1");
            }
            else bNewValue = !bNewValue;

            AW.g_AWPlayer[client].Triggers = bNewValue;

            if (bNewValue)
            {
                if (GetClientPrefs() is { } cp && cp.IsLoaded(client.SteamId))
                {
                    cp.SetCookie(client.SteamId, "AW_Triggers", "1");
                }
                UI.ReplyToCommand(client, "ActWatch.Reply.Trigger.Enabled", command.ChatTrigger, 1, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_enabled : "");
            }
            else
            {
                if (GetClientPrefs() is { } cp && cp.IsLoaded(client.SteamId))
                {
                    cp.SetCookie(client.SteamId, "AW_Triggers", "0");
                }
                UI.ReplyToCommand(client, "ActWatch.Reply.Trigger.Disabled", command.ChatTrigger, 1, AW.g_Scheme != null ? AW.g_Scheme.Color_warning : "", AW.g_Scheme != null ? AW.g_Scheme.Color_disabled : "");
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWStatus(IGameClient client, StringCommand command)
        {
            if (!Cvar.ButtonGlobalEnable) return ECommandAction.Stopped;
            return OnAWStatus(client, command, true);
        }

        private ECommandAction OnTWStatus(IGameClient client, StringCommand command)
        {
            if (!Cvar.TriggerGlobalEnable) return ECommandAction.Stopped;
            return OnAWStatus(client, command, false);
        }

        private static ECommandAction OnAWStatus(IGameClient client, StringCommand command, bool bType)
        {
            if (!client.IsValid || AW.g_Scheme == null) return ECommandAction.Stopped;

            var players = command.ArgCount >= 1 ? TargetManager.Find(client, command.GetArg(1)) : TargetManager.Find(client, "@me");

            if (players.Count > 0)
            {
                IGameClient target = players.Single();

                if (!AW.CheckDictionary(target))
                {
                    UI.ReplyToCommand(client, "ActWatch.Info.Error.NotFoundInDictionary", command.ChatTrigger, 2);
                    return ECommandAction.Stopped;
                }
                ActBanPlayer bannedPlayer = bType ? AW.g_AWPlayer[target].ButtonBannedPlayer : AW.g_AWPlayer[target].TriggerBannedPlayer;
                if (bannedPlayer.bBanned)
                {
                    UI.ReplyToCommand(client, bType ? "ActWatch.Reply.Buttons.Has_a_ban" : "ActWatch.Reply.Triggers.Has_a_ban", command.ChatTrigger, 0, UI.PlayerInfo(client, UI.PlayerInfoFormat(target)), AW.g_Scheme.Color_disabled);
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
                }
                else UI.ReplyToCommand(client, bType ? "ActWatch.Reply.Buttons.Can_use" : "ActWatch.Reply.Triggers.Can_touch", command.ChatTrigger, bType ? (byte)0 : (byte)1, UI.PlayerInfo(client, UI.PlayerInfoFormat(target)), AW.g_Scheme.Color_enabled);
            }
            else UI.ReplyToCommand(client, "ActWatch.Reply.No_matching_client", command.ChatTrigger, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning);

            return ECommandAction.Stopped;
        }
    }
}
