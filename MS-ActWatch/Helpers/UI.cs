using Sharp.Shared.Enums;
using Sharp.Shared.GameEntities;
using Sharp.Shared.Objects;
using System.Globalization;

namespace MS_ActWatch.Helpers
{
    static class UI
    {
        public static void AWChatActivationNotify(IBaseEntity entity, IGameClient client, bool bType) //true - button
        {
            string[] sPlayerInfoFormat = PlayerInfoFormat(client);
            string sEntityID = !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}";
            PrintToConsole(ServerLocalizer.Format(CultureInfo.GetCultureInfo(Cvar.ServerLanguage), ReplaceColorTags(bType ? "ActWatch.Chat.Button" : "ActWatch.Chat.Trigger", false), ReplaceColorTags(sPlayerInfoFormat[3], false), "", "", entity.Name, "", "", sEntityID), 4);

            Task.Run(() =>
            {
                LogManager.SystemAction(ReplaceColorTags(bType ? "ActWatch.Chat.Button" : "ActWatch.Chat.Trigger", false), true, ReplaceColorTags(sPlayerInfoFormat[3], false), "", "", entity.Name, "", "", sEntityID);
            });

            if (AW.g_Scheme != null)
            {
                foreach (var pair in AW.g_AWPlayer)
                {
                    if (pair.Key is { IsValid: true, IsFakeClient: false, IsHltv: false } cl && (bType && pair.Value.Buttons || !bType && pair.Value.Triggers))
                    {
                        ReplyToCommand(cl, bType ? "ActWatch.Chat.Button" : "ActWatch.Chat.Trigger", true, bType ? (byte)0 : (byte)1, PlayerInfo(cl, sPlayerInfoFormat), bType ? AW.g_Scheme.Color_use_button : AW.g_Scheme.Color_use_trigger, AW.g_Scheme.Color_entity_name, entity.Name, AW.g_Scheme.Color_warning, AW.g_Scheme.Color_entity_id, sEntityID);
                    }
                }
            }
        }

        public static void AWChatAdminBan(string[] sPIF_admin, string[] sPIF_player, string sReason, bool bAction, bool bType)
        {
            string sType = bType ? "Buttons" : "Triggers";
            AWAdminInfo(bAction ? $"ActWatch.Chat.Admin.{sType}.Banned" : $"ActWatch.Chat.Admin.{sType}.Unrestricted", "", ReplaceColorTags(sPIF_admin[3], false), "", ReplaceColorTags(sPIF_player[3], false));
            AWAdminInfo("ActWatch.Chat.Admin.Reason", "", sReason);

            if (AW.g_Scheme == null) return;

            foreach (var pair in AW.g_AWPlayer.ToList())
            {
                ReplyToCommand(pair.Key, bAction ? $"ActWatch.Chat.Admin.{sType}.Banned" : $"ActWatch.Chat.Admin.{sType}.Unrestricted", true, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, PlayerInfo(pair.Key, sPIF_admin), bAction ? AW.g_Scheme.Color_disabled : AW.g_Scheme.Color_enabled, PlayerInfo(pair.Key, sPIF_player));
                ReplyToCommand(pair.Key, "ActWatch.Chat.Admin.Reason", true, bType ? (byte)0 : (byte)1, AW.g_Scheme.Color_warning, sReason);
            }
        }

        public static void AWSysInfo(string sMessage, int iColor = 15, params object[] arg)
        {
            PrintToConsole(ServerLocalizer.Format(CultureInfo.GetCultureInfo(Cvar.ServerLanguage), ReplaceColorTags(sMessage, false), arg), iColor);

            Task.Run(() =>
            {
                LogManager.SystemAction(ReplaceColorTags(sMessage, false), true, arg);
            });
        }

        public static void AWSysInfoServerInit(string sMessage, int iColor = 15, params object[] arg)
        {
            PrintToConsole(ServerLocalizer.Format(CultureInfo.GetCultureInfo(Cvar.ServerLanguage), ReplaceColorTags(sMessage, false), arg), iColor);

            Task.Run(() =>
            {
                LogManager.SystemAction(ReplaceColorTags(sMessage, false), false, arg);
            });
        }

        public static void AWAdminInfo(string sMessage, params object[] arg)
        {
            PrintToConsole(ServerLocalizer.Format(CultureInfo.GetCultureInfo(Cvar.ServerLanguage), ReplaceColorTags(sMessage, false), arg), 2);

            Task.Run(() =>
            {
                LogManager.AdminAction(ReplaceColorTags(sMessage, false), arg);
            });
        }

        public static void CvarChangeNotify(string sCvarName, string sCvarValue, bool bClientNotify)
        {
            PrintToConsole(ServerLocalizer.Format(CultureInfo.GetCultureInfo(Cvar.ServerLanguage), "ActWatch.Cvar.Notify", [sCvarName, sCvarValue]), 3);

            Task.Run(() =>
            {
                LogManager.CvarAction(sCvarName, sCvarValue);
            });

            Task.Run(() =>
            {
                if (bClientNotify && AW.g_Scheme != null)
                {
                    foreach (var pair in AW.g_AWPlayer)
                    {
                        if (pair.Key is { } client) ReplyToCommand(client, "ActWatch.Cvar.Notify.Clients", true, 2, [AW.g_Scheme.Color_warning, AW.g_Scheme.Color_name, sCvarName, sCvarValue]);
                    }
                }
            });
        }

        public static void ReplyToCommand(IGameClient client, string sMessage, bool bChat, byte iTag, params object[] arg) //0 - Buttons, 1 - Triggers, 2 - ActWacth
        {
            if (client is { IsValid: true, IsFakeClient: false, IsHltv: false } && client.GetPlayerController() is { } player && ActWatch.GetLocalizer() is { } lm)
            {
                var localizer = lm.GetLocalizer(client);
                switch(iTag)
                {
                    case 0: { player.Print(bChat ? HudPrintChannel.Chat : HudPrintChannel.Console, ReplaceColorTags($" {localizer.Format("ActWatch.Chat.Tag.Button")} {localizer.Format(sMessage, arg)}", bChat)); break; }
                    case 1: { player.Print(bChat ? HudPrintChannel.Chat : HudPrintChannel.Console, ReplaceColorTags($" {localizer.Format("ActWatch.Chat.Tag.Trigger")} {localizer.Format(sMessage, arg)}", bChat)); break; }
                    default: { player.Print(bChat ? HudPrintChannel.Chat : HudPrintChannel.Console, ReplaceColorTags($" {localizer.Format("ActWatch.Chat.Tag.ActWatch")} {localizer.Format(sMessage, arg)}", bChat)); break; }
                }
                
            }
        }

        public static string PlayerInfo(IGameClient? client, string[] sPlayerInfoFormat)
        {
            if (client != null)
            {
                if (AW.g_AWPlayer[client].PFormatPlayer < 0 || AW.g_AWPlayer[client].PFormatPlayer > 3) return sPlayerInfoFormat[Cvar.PlayerFormat];
                return sPlayerInfoFormat[AW.g_AWPlayer[client].PFormatPlayer];
            }
            return sPlayerInfoFormat[3];
        }
        public static string[] PlayerInfoFormat(IGameClient client)
        {
            if (client != null)
            {
                string[] sResult = new string[4];
                sResult[0] = $"{AW.g_Scheme?.Color_name}{ReplaceSpecial(client.Name)}{AW.g_Scheme?.Color_warning}";
                sResult[1] = $"{sResult[0]}[{AW.g_Scheme?.Color_steamid}#{client.UserId}{AW.g_Scheme?.Color_warning}]";
                sResult[2] = $"{sResult[0]}[{AW.g_Scheme?.Color_steamid}#{AW.ConvertSteamID64ToSteamID(client.SteamId.ToString())}{AW.g_Scheme?.Color_warning}]";
                sResult[3] = $"{sResult[0]}[{AW.g_Scheme?.Color_steamid}#{client.UserId}{AW.g_Scheme?.Color_warning}|{AW.g_Scheme?.Color_steamid}#{AW.ConvertSteamID64ToSteamID(client.SteamId.ToString())}{AW.g_Scheme?.Color_warning}]";
                return sResult;
            }
            return PlayerInfoFormat("Console", "Server");
        }

        public static string[] PlayerInfoFormat(string sName, string sSteamID)
        {
            string[] sResult = new string[4];
            sResult[0] = $"{AW.g_Scheme?.Color_name}{ReplaceSpecial(sName)}{AW.g_Scheme?.Color_warning}";
            sResult[1] = sResult[0];
            sResult[2] = $"{AW.g_Scheme?.Color_name}{ReplaceSpecial(sName)}{AW.g_Scheme?.Color_warning}[{AW.g_Scheme?.Color_steamid}{sSteamID}{AW.g_Scheme?.Color_warning}]";
            sResult[3] = sResult[2];
            return sResult;
        }

        public static void PrintToConsole(string sMessage, int iColor = 1)
        {
            Console.ForegroundColor = (ConsoleColor)8;
            Console.Write("[");
            Console.ForegroundColor = (ConsoleColor)6;
            Console.Write("ActWatch");
            Console.ForegroundColor = (ConsoleColor)8;
            Console.Write("] ");
            Console.ForegroundColor = (ConsoleColor)iColor;
            Console.WriteLine(sMessage, false);
            Console.ResetColor();
            /* Colors:
				* 0 - No color		1 - White		2 - Red-Orange		3 - Orange
				* 4 - Yellow		5 - Dark Green	6 - Green			7 - Light Green
				* 8 - Cyan			9 - Sky			10 - Light Blue		11 - Blue
				* 12 - Violet		13 - Pink		14 - Light Red		15 - Red */
        }

        public static string ReplaceSpecial(string input)
        {
            input = input.Replace("{", "[");
            input = input.Replace("}", "]");

            return input;
        }

        public static string ReplaceColorTags(string input, bool bChat = true)
        {
            for (var i = 0; i < colorPatterns.Length; i++)
                input = input.Replace(colorPatterns[i], bChat ? colorReplacements[i] : "");

            return input;
        }

        readonly static string[] colorPatterns =
        [
            "{default}", "{darkred}", "{purple}", "{green}", "{lightgreen}", "{lime}", "{red}", "{grey}", "{team}", "{red2}",
            "{olive}", "{a}", "{lightblue}", "{blue}", "{d}", "{pink}", "{darkorange}", "{orange}", "{darkblue}", "{gold}",
            "{white}", "{yellow}", "{magenta}", "{silver}", "{bluegrey}", "{lightred}", "{cyan}", "{gray}", "{lightyellow}",
        ];
        readonly static string[] colorReplacements =
        [
            "\x01", "\x02", "\x03", "\x04", "\x05", "\x06", "\x07", "\x08", "\x03", "\x0F",
            "\x06", "\x0A", "\x0B", "\x0C", "\x0D", "\x0E", "\x0F", "\x10", "\x0C", "\x10",
            "\x01", "\x09", "\x0E", "\x0A", "\x0D", "\x0F", "\x03", "\x08", "\x06"
        ];
    }
}
