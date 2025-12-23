using Microsoft.Extensions.Configuration;
using MS_ActWatch_Shared;
using Sharp.Shared;
using Sharp.Shared.Enums;
using Sharp.Shared.Managers;
using Sharp.Shared.Objects;
using Sharp.Shared.Types;

namespace MS_ActWatch_Example
{
    public class ActWatchExampleAPI : IModSharpModule
    {
        public string DisplayName => "ActWatchAPI-Example";
        public string DisplayAuthor => "DarkerZ[RUS]";
#pragma warning disable IDE0060, IDE0290
        public ActWatchExampleAPI(ISharedSystem sharedSystem, string dllPath, string sharpPath, Version version, IConfiguration coreConfiguration, bool hotReload)
#pragma warning restore IDE0060, IDE0290
        {
            _modules = sharedSystem.GetSharpModuleManager();
            _clientmanager = sharedSystem.GetClientManager();
        }

        private readonly ISharpModuleManager _modules;
        private readonly IClientManager _clientmanager;

        public bool Init()
        {
            _clientmanager.InstallCommandCallback("bwtest1", OnBWT1);
            _clientmanager.InstallCommandCallback("bwtest2", OnBWT2);
            _clientmanager.InstallCommandCallback("bwtest3", OnBWT3);
            _clientmanager.InstallCommandCallback("bwtest4", OnBWT4);

            _clientmanager.InstallCommandCallback("twtest1", OnTWT1);
            _clientmanager.InstallCommandCallback("twtest2", OnTWT2);
            _clientmanager.InstallCommandCallback("twtest3", OnTWT3);
            _clientmanager.InstallCommandCallback("twtest4", OnTWT4);
            return true;
        }

        public void Shutdown()
        {
            _clientmanager.RemoveCommandCallback("bwtest1", OnBWT1);
            _clientmanager.RemoveCommandCallback("bwtest2", OnBWT2);
            _clientmanager.RemoveCommandCallback("bwtest3", OnBWT3);
            _clientmanager.RemoveCommandCallback("bwtest4", OnBWT4);

            _clientmanager.RemoveCommandCallback("twtest1", OnTWT1);
            _clientmanager.RemoveCommandCallback("twtest2", OnTWT2);
            _clientmanager.RemoveCommandCallback("twtest3", OnTWT3);
            _clientmanager.RemoveCommandCallback("twtest4", OnTWT4);
        }

        void DisplayButtonBan(SAWAPI_Ban sawPlayer) { PrintToConsole($"Player {sawPlayer.sClientName} was button-banned {sawPlayer.sAdminName}"); }
        void DisplayButtonUnBan(SAWAPI_Ban sawPlayer) { PrintToConsole($"Player {sawPlayer.sClientName} was button-unbanned {sawPlayer.sAdminName}"); }
        void DisplayDoorPressed(IGameClient Player, string sDoorName, string sDoorID) { PrintToConsole($"Player {Player.Name} used door {sDoorName}({sDoorID})"); }
        void DisplayPhysboxPressed(IGameClient Player, string sPhysboxName, string sPhysboxID) { PrintToConsole($"Player {Player.Name} used physbox {sPhysboxName}({sPhysboxID})"); }
        void DisplayButtonPressed(IGameClient Player, string sButtonName, string sButtonID) { PrintToConsole($"Player {Player.Name} used button {sButtonName}({sButtonID})"); }
        void DisplayButtonClientBannedResult(SAWAPI_Ban sawPlayer) { if (sawPlayer.bBanned) PrintToConsole($"You {sawPlayer.sClientName}({sawPlayer.sClientSteamID}) have a bban. Duration: {sawPlayer.iDuration}"); else PrintToConsole($"You have NOT a bban"); }
        void DisplayTriggerBan(SAWAPI_Ban sawPlayer) { PrintToConsole($"Player {sawPlayer.sClientName} was trigger-banned {sawPlayer.sAdminName}"); }
        void DisplayTriggerUnBan(SAWAPI_Ban sawPlayer) { PrintToConsole($"Player {sawPlayer.sClientName} was trigger-unbanned {sawPlayer.sAdminName}"); }
        void DisplayTriggerOnceTouch(IGameClient Player, string sTriggerName, string sTriggerID) { PrintToConsole($"Player {Player.Name} touch the trigger_once {sTriggerName}({sTriggerID})"); }
        void DisplayTriggerMultipleTouch(IGameClient Player, string sTriggerName, string sTriggerID) { PrintToConsole($"Player {Player.Name} touch the trigger_multiple {sTriggerName}({sTriggerID})"); }
        void DisplayTriggerClientBannedResult(SAWAPI_Ban sawPlayer) { if (sawPlayer.bBanned) PrintToConsole($"You {sawPlayer.sClientName}({sawPlayer.sClientSteamID}) have a trban. Duration: {sawPlayer.iDuration}"); else PrintToConsole($"You have NOT a tban"); }

        private ECommandAction OnBWT1(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                _api.Native_ButtonWatch_IsClientBanned(sSteamID);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWT2(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                SAWAPI_Ban ban = new()
                {
                    sAdminName = "Api",
                    sAdminSteamID = "SERVER",
                    iDuration = 5,
                    sReason = "Test Button Api Ban",
                    sClientName = client.Name,
                    sClientSteamID = sSteamID
                };
                ban.iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + ban.iDuration * 60;
                _api.Native_ButtonWatch_BanClient(ban);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWT3(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                SAWAPI_Ban ban = new()
                {
                    sAdminName = "Api",
                    sAdminSteamID = "SERVER",
                    sReason = "Test Button Api UnBan",
                    sClientName = client.Name,
                    sClientSteamID = sSteamID
                };
                ban.iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + ban.iDuration * 60;
                _api.Native_ButtonWatch_UnbanClient(ban);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnBWT4(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                _api.Native_ButtonWatch_UpdateStatusBanClient(client);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnTWT1(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                _api.Native_TriggerWatch_IsClientBanned(sSteamID);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnTWT2(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                SAWAPI_Ban ban = new()
                {
                    sAdminName = "Api",
                    sAdminSteamID = "SERVER",
                    iDuration = 5,
                    sReason = "Test Trigger Api Ban",
                    sClientName = client.Name,
                    sClientSteamID = sSteamID
                };
                ban.iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + ban.iDuration * 60;
                _api.Native_TriggerWatch_BanClient(ban);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnTWT3(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                string? sSteamID = ConvertSteamID64ToSteamID(client.SteamId.ToString());
                if (string.IsNullOrEmpty(sSteamID))
                {
                    PrintToConsole("Failed SteamID");
                    return ECommandAction.Stopped;
                }
                SAWAPI_Ban ban = new()
                {
                    sAdminName = "Api",
                    sAdminSteamID = "SERVER",
                    sReason = "Test Trigger Api UnBan",
                    sClientName = client.Name,
                    sClientSteamID = sSteamID
                };
                ban.iTimeStamp_Issued = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) + ban.iDuration * 60;
                _api.Native_TriggerWatch_UnbanClient(ban);
            }

            return ECommandAction.Stopped;
        }

        private ECommandAction OnTWT4(IGameClient client, StringCommand command)
        {
            if (client.IsValid && GetActWatch() is { } _api)
            {
                _api.Native_TriggerWatch_UpdateStatusBanClient(client);
            }

            return ECommandAction.Stopped;
        }

        public static void PrintToConsole(string sMessage)
        {
            Console.ForegroundColor = (ConsoleColor)8;
            Console.Write("[");
            Console.ForegroundColor = (ConsoleColor)6;
            Console.Write("ActWatch:TestAPI");
            Console.ForegroundColor = (ConsoleColor)8;
            Console.Write("] ");
            Console.ForegroundColor = (ConsoleColor)13;
            Console.WriteLine(sMessage);
            Console.ResetColor();
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

        //Init IActWatchAPI
        public void OnAllModulesLoaded() => GetActWatch();
        public void OnLibraryConnected(string name)
        {
            if (name.Equals("ActWatch")) GetActWatch();
        }
        public void OnLibraryDisconnect(string name)
        {
            if (name.Equals("ActWatch"))
            {
                if (_iactwatch?.Instance is { } _api)
                {
                    _api.Forward_ButtonWatch_OnClientBanned -= DisplayButtonBan;
                    _api.Forward_ButtonWatch_OnClientUnbanned -= DisplayButtonUnBan;
                    _api.Forward_ButtonWatch_OnDoorPressed -= DisplayDoorPressed;
                    _api.Forward_ButtonWatch_OnPhysboxPressed -= DisplayPhysboxPressed;
                    _api.Forward_ButtonWatch_OnButtonPressed -= DisplayButtonPressed;
                    _api.Forward_ButtonWatch_IsClientBannedResult -= DisplayButtonClientBannedResult;
                    _api.Forward_TriggerWatch_OnClientBanned -= DisplayTriggerBan;
                    _api.Forward_TriggerWatch_OnClientUnbanned -= DisplayTriggerUnBan;
                    _api.Forward_TriggerWatch_OnTriggerOnceTouch -= DisplayTriggerOnceTouch;
                    _api.Forward_TriggerWatch_OnTriggerMultipleTouch -= DisplayTriggerMultipleTouch;
                    _api.Forward_TriggerWatch_IsClientBannedResult -= DisplayTriggerClientBannedResult;
                }
                _iactwatch = null;
            }
        }
        private IModSharpModuleInterface<IActWatchAPI>? _iactwatch;
        private IActWatchAPI? GetActWatch()
        {
            if (_iactwatch?.Instance is null)
            {
                _iactwatch = _modules.GetOptionalSharpModuleInterface<IActWatchAPI>(IActWatchAPI.Identity);
                if (_iactwatch?.Instance is { } _api)
                {
                    _api.Forward_ButtonWatch_OnClientBanned += DisplayButtonBan;
                    _api.Forward_ButtonWatch_OnClientUnbanned += DisplayButtonUnBan;
                    _api.Forward_ButtonWatch_OnDoorPressed += DisplayDoorPressed;
                    _api.Forward_ButtonWatch_OnPhysboxPressed += DisplayPhysboxPressed;
                    _api.Forward_ButtonWatch_OnButtonPressed += DisplayButtonPressed;
                    _api.Forward_ButtonWatch_IsClientBannedResult += DisplayButtonClientBannedResult;
                    _api.Forward_TriggerWatch_OnClientBanned += DisplayTriggerBan;
                    _api.Forward_TriggerWatch_OnClientUnbanned += DisplayTriggerUnBan;
                    _api.Forward_TriggerWatch_OnTriggerOnceTouch += DisplayTriggerOnceTouch;
                    _api.Forward_TriggerWatch_OnTriggerMultipleTouch += DisplayTriggerMultipleTouch;
                    _api.Forward_TriggerWatch_IsClientBannedResult += DisplayTriggerClientBannedResult;
                }
            }
            return _iactwatch?.Instance;
        }
    }
}
