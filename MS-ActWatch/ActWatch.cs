using Microsoft.Extensions.Configuration;
using MS_ActWatch.Helpers;
using MS_ActWatch_Shared;
#if (USE_ENTWATCH)
using MS_EntWatch_Shared;
#endif
using Sharp.Modules.ClientPreferences.Shared;
using Sharp.Modules.LocalizerManager.Shared;
using Sharp.Shared;
using Sharp.Shared.Enums;
using Sharp.Shared.GameEntities;
using Sharp.Shared.Hooks;
using Sharp.Shared.Listeners;
using Sharp.Shared.Managers;
using Sharp.Shared.Objects;
using System.Runtime.CompilerServices;

[assembly: DisableRuntimeMarshalling]

namespace MS_ActWatch
{
    public partial class ActWatch : IModSharpModule, IGameListener, IEntityListener, IClientListener
    {
        public string DisplayName => "ActWatch";
        public string DisplayAuthor => "DarkerZ[RUS]";
#pragma warning disable IDE0060
        public ActWatch(ISharedSystem sharedSystem, string dllPath, string sharpPath, Version version, IConfiguration coreConfiguration, bool hotReload)
#pragma warning restore IDE0060
        {
            _modSharp = sharedSystem.GetModSharp();
            _modules = sharedSystem.GetSharpModuleManager();
            _convars = sharedSystem.GetConVarManager();
            _clients = sharedSystem.GetClientManager();
            _entities = sharedSystem.GetEntityManager();
            _physicsQuery = sharedSystem.GetPhysicsQueryManager();
            _dllPath = dllPath;
            _sharpPath = sharpPath;
            _virtualHook = sharedSystem.GetHookManager().CreateVirtualHook();
        }
#pragma warning disable CA2211
        public static IModSharp? _modSharp;
        private static ISharpModuleManager? _modules;
        private readonly IConVarManager _convars;
        public static IClientManager? _clients;
        public static IEntityManager? _entities;
        public static IPhysicsQueryManager? _physicsQuery;
        public static string? _dllPath;
        public static string? _sharpPath;
        private IDisposable? _callback;
        private readonly IVirtualHook _virtualHook;
#pragma warning restore CA2211

        private static IModSharpModuleInterface<ILocalizerManager>? _localizer;
        private IModSharpModuleInterface<IClientPreference>? _icp;
#if (USE_ENTWATCH)
        private static IModSharpModuleInterface<IEntWatchAPI>? _ientwatch;
#endif
        public bool Init()
        {
            RegisterCvars();
            if (!Hook_Triggers_StartTouch()) return false;
            if (!Hook_Triggers_EndTouch()) return false;
            if (!Hook_Triggers_Touch()) return false;
            _modSharp!.InstallGameListener(this);
            _entities!.InstallEntityListener(this);
            _clients!.InstallClientListener(this);
            RegCommands();
            RegAdminCommands();
            return true;
        }

        public void PostInit()
        {
            _modules!.RegisterSharpModuleInterface<IActWatchAPI>(this, IActWatchAPI.Identity, AW.g_cAWAPI);
            _entities!.HookEntityOutput("trigger_once", "OnStartTouch");
            _entities!.HookEntityOutput("trigger_multiple", "OnStartTouch");
            _entities!.HookEntityOutput("func_button", "OnPressed");
            _entities!.HookEntityOutput("func_rot_button", "OnPressed");
            _entities!.HookEntityOutput("func_door", "OnOpen");
            _entities!.HookEntityOutput("func_door_rotating", "OnOpen");
            _entities!.HookEntityOutput("func_physbox", "OnPlayerUse");
        }

        public void OnAllModulesLoaded()
        {
            GetClientPrefs();
            GetLocalizer()?.LoadLocaleFile("ActWatch");
            ServerLocalizer.LoadLocaleFile("ActWatch");
#if (USE_ENTWATCH)
            GetEntWatch();
#endif
            AW.InitTimers();
            ActBan.ActBanDB.Init_DB();
        }

        public void OnLibraryConnected(string name)
        {
            if (name.Equals("ClientPreferences")) GetClientPrefs();
#if (USE_ENTWATCH)
            else if (name.Equals("EntWatch")) GetEntWatch();
#endif
        }

        public void OnLibraryDisconnect(string name)
        {
            if (name.Equals("ClientPreferences")) _icp = null;
#if (USE_ENTWATCH)
            else if (name.Equals("EntWatch")) _icp = null;
#endif
        }

        private void OnCookieLoad(IGameClient client)
        {
            LoadClientPrefs(client);
        }

        public void Shutdown()
        {
            LogManager.UnInit();
            _modSharp!.RemoveGameListener(this);
            _entities!.RemoveEntityListener(this);
            _clients!.RemoveClientListener(this);
            _callback?.Dispose();
            AdminCmdsManager.UnRegCommands();
            UnRegCommands();
            AW.RemoveTimers();
            UnRegisterCvars();
            ActBan.ActBanDB.db?.AnyDB.UnSet();
        }

        public void OnGameActivate() //OnMapStart
        {
            OnMapStart_Listener();
        }

        public void OnGameDeactivate() //OnMapEnd
        {
            OnMapEnd_Listener();
        }

        public void OnRoundRestart() //OnRoundPreStart
        {
            OnEventRoundStart();
        }

        public void OnClientPutInServer(IGameClient client)
        {
            OnEventPlayerConnectFull(client);
        }

        public void OnClientDisconnecting(IGameClient client, NetworkDisconnectionReason reason)
        {
            OnEventPlayerDisconnect(client);
        }

        public EHookAction OnEntityFireOutput(IBaseEntity entity, string output, IBaseEntity? activator, float delay)
        {
            if (entity.Classname.StartsWith('t')) return OnTriggerTouch(entity, activator);
            return OnButtonPressed(entity, activator);
        }

        public static ILocalizerManager? GetLocalizer()
        {
            if (_localizer?.Instance is null)
            {
                _localizer = _modules!.GetOptionalSharpModuleInterface<ILocalizerManager>(ILocalizerManager.Identity);
            }
            return _localizer?.Instance;
        }

        private IClientPreference? GetClientPrefs()
        {
            if (_icp?.Instance is null)
            {
                _icp = _modules!.GetOptionalSharpModuleInterface<IClientPreference>(IClientPreference.Identity);
                if (_icp?.Instance is { } instance) _callback = instance.ListenOnLoad(OnCookieLoad);
            }
            return _icp?.Instance;
        }

#if (USE_ENTWATCH)
        private static IEntWatchAPI? GetEntWatch()
        {
            if (_ientwatch?.Instance is null)
            {
                _ientwatch = _modules!.GetOptionalSharpModuleInterface<IEntWatchAPI>(IEntWatchAPI.Identity);
            }
            return _ientwatch?.Instance;
        }
#endif
        int IGameListener.ListenerVersion => IGameListener.ApiVersion;
        int IGameListener.ListenerPriority => 0;
        int IEntityListener.ListenerVersion => IEntityListener.ApiVersion;
        int IEntityListener.ListenerPriority => 0;
        int IClientListener.ListenerVersion => IClientListener.ApiVersion;
        int IClientListener.ListenerPriority => 0;
    }
}
