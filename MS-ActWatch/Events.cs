using MS_ActWatch.ActBan;
using MS_ActWatch.Helpers;
using Sharp.Shared.Enums;
using Sharp.Shared.GameEntities;
using Sharp.Shared.Objects;
using System.Runtime.InteropServices;

namespace MS_ActWatch
{
    public partial class ActWatch
    {
        private static void OnMapStart_Listener()
        {
            AW.LoadScheme();
            AW.LoadWhiteList();

            Task.Run(() =>
            {
                LogManager.SystemAction("ActWatch.Info.ChangeMap", true, _modSharp!.GetMapName()!);
            });
        }

        private static void OnMapEnd_Listener()
        {
            if (AW.g_WhiteList != null)
            {
                AW.g_WhiteList.Buttons.Clear();
                AW.g_WhiteList.Triggers.Clear();
                AW.g_WhiteList = null;
            }
        }

        private static void OnEventRoundStart()
        {
            foreach (var client in _clients!.GetGameClients(true).ToArray())
            {
                if (AW.CheckDictionary(client))
                {
                    AW.g_AWPlayer[client].TriggerBannedPlayer.bTriggerBanned = AW.g_AWPlayer[client].TriggerBannedPlayer.bBanned;
                }
            }
        }

        private void LoadClientPrefs(IGameClient client)
        {
            if (client == null || !client.IsValid || GetClientPrefs() is not { } cp || !cp.IsLoaded(client.SteamId)) return;

            if (AW.CheckDictionary(client))
            {
                //PlayerInfo Format
                if (cp.GetCookie(client.SteamId, "AW_PInfo_Format") is { } cookie_pliformat)
                {
                    string sValue = cookie_pliformat.GetString();
                    if (string.IsNullOrEmpty(sValue) || !Int32.TryParse(sValue, out int iValue)) iValue = Cvar.PlayerFormat;
                    AW.g_AWPlayer[client].PFormatPlayer = iValue;
                }
                else
                {
                    cp.SetCookie(client.SteamId, "AW_PInfo_Format", $"{Cvar.PlayerFormat}");
                    AW.g_AWPlayer[client].PFormatPlayer = Cvar.PlayerFormat;
                }
                //Buttons
                if (cp.GetCookie(client.SteamId, "AW_Buttons") is { } cookie_buttons)
                {
                    string sValue = cookie_buttons.GetString();
                    if (string.IsNullOrEmpty(sValue)) AW.g_AWPlayer[client].Buttons = false;
                    else AW.g_AWPlayer[client].Buttons = !string.Equals(sValue, "0");
                }
                else
                {
                    cp.SetCookie(client.SteamId, "AW_Buttons", "0");
                    AW.g_AWPlayer[client].Buttons = false;
                }
                //Triggers
                if (cp.GetCookie(client.SteamId, "AW_Triggers") is { } cookie_triggers)
                {
                    string sValue = cookie_triggers.GetString();
                    if (string.IsNullOrEmpty(sValue)) AW.g_AWPlayer[client].Triggers = false;
                    else AW.g_AWPlayer[client].Triggers = !string.Equals(sValue, "0");
                }
                else
                {
                    cp.SetCookie(client.SteamId, "AW_Triggers", "0");
                    AW.g_AWPlayer[client].Triggers = false;
                }
            }
        }

        private static void OnEventPlayerConnectFull(IGameClient client)
        {
            OfflineFunc.PlayerConnectFull(client);

            if (client.IsValid)
            {
                AW.CheckDictionary(client); //Add EWPlayer
            }

            ActBanPlayer.GetBan(client, true, true); //Set Button Ban
            ActBanPlayer.GetBan(client, false, true); //Set Trigger Ban
        }

        private static void OnEventPlayerDisconnect(IGameClient client)
        {
            OfflineFunc.PlayerDisconnect(client);

            AW.g_AWPlayer.Remove(client);   //Remove AWPlayer
        }

        private static unsafe delegate* unmanaged<nint, nint, bool> CBaseEntity_StartTouch;
        private unsafe bool Hook_Triggers_StartTouch()
        {
            var vFuncIndex = _modSharp!.GetGameData().GetVFuncIndex("CBaseEntity::StartTouch");
            _virtualHook.Prepare("server", "CTriggerOnce", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_StartTouch));
            if (!_virtualHook.Install()) return false;
            _virtualHook.Prepare("server", "CTriggerMultiple", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_StartTouch));
            if (!_virtualHook.Install()) return false;
            CBaseEntity_StartTouch = (delegate* unmanaged<nint, nint, bool>)_virtualHook.Trampoline;

            return true;
        }
        [UnmanagedCallersOnly]
        private static unsafe bool Hook_CBaseEntity_StartTouch(nint @ent, nint @act)
        {
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@ent) is not { IsValidEntity: true } entity) return false;
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@act) is not { IsValidEntity: true } activator) return false;

            if (AW.g_WhiteList != null && AW.g_WhiteList.Triggers.Count > 0)
            {
                foreach (var triggerid in AW.g_WhiteList.Triggers)
                {
                    if (string.Equals(triggerid, entity.HammerId, StringComparison.OrdinalIgnoreCase)) return CBaseEntity_StartTouch(@ent, @act);
                }
            }

            if ((Cvar.TriggerWatchOnce || Cvar.TriggerWatchMultiple) && activator.AsPlayerPawn() is { } pawn && pawn.GetController() is { } player && player.GetGameClient() is { } client && AW.CheckDictionary(client) && AW.g_AWPlayer[client].TriggerBannedPlayer.bTriggerBanned)
            {
                if (Cvar.TriggerWatchOnce && entity.Classname.StartsWith("trigger_o")) return false;
                else if (Cvar.TriggerWatchMultiple) return false;
            }
            return CBaseEntity_StartTouch(@ent, @act);
        }

        private static unsafe delegate* unmanaged<nint, nint, bool> CBaseEntity_EndTouch;
        private unsafe bool Hook_Triggers_EndTouch()
        {
            var vFuncIndex = _modSharp!.GetGameData().GetVFuncIndex("CBaseEntity::EndTouch");
            _virtualHook.Prepare("server", "CTriggerOnce", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_EndTouch));
            if (!_virtualHook.Install()) return false;
            _virtualHook.Prepare("server", "CTriggerMultiple", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_EndTouch));
            if (!_virtualHook.Install()) return false;
            CBaseEntity_EndTouch = (delegate* unmanaged<nint, nint, bool>)_virtualHook.Trampoline;

            return true;
        }
        [UnmanagedCallersOnly]
        private static unsafe bool Hook_CBaseEntity_EndTouch(nint @ent, nint @act)
        {
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@ent) is not { IsValidEntity: true } entity) return false;
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@act) is not { IsValidEntity: true } activator) return false;

            if (AW.g_WhiteList != null && AW.g_WhiteList.Triggers.Count > 0)
            {
                foreach (var triggerid in AW.g_WhiteList.Triggers)
                {
                    if (string.Equals(triggerid, entity.HammerId, StringComparison.OrdinalIgnoreCase)) return CBaseEntity_EndTouch(@ent, @act);
                }
            }

            if ((Cvar.TriggerWatchOnce || Cvar.TriggerWatchMultiple) && activator.AsPlayerPawn() is { } pawn && pawn.GetController() is { } player && player.GetGameClient() is { } client && AW.CheckDictionary(client) && AW.g_AWPlayer[client].TriggerBannedPlayer.bTriggerBanned)
            {
                if (Cvar.TriggerWatchOnce && entity.Classname.StartsWith("trigger_o")) return false;
                else if (Cvar.TriggerWatchMultiple) return false;
            }
            return CBaseEntity_EndTouch(@ent, @act);
        }

        private static unsafe delegate* unmanaged<nint, nint, bool> CBaseEntity_Touch;
        private unsafe bool Hook_Triggers_Touch()
        {
            var vFuncIndex = _modSharp!.GetGameData().GetVFuncIndex("CBaseEntity::Touch");
            _virtualHook.Prepare("server", "CTriggerOnce", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_Touch));
            if (!_virtualHook.Install()) return false;
            _virtualHook.Prepare("server", "CTriggerMultiple", vFuncIndex, (nint)(delegate* unmanaged<nint, nint, bool>)(&Hook_CBaseEntity_Touch));
            if (!_virtualHook.Install()) return false;
            CBaseEntity_Touch = (delegate* unmanaged<nint, nint, bool>)_virtualHook.Trampoline;

            return true;
        }
        [UnmanagedCallersOnly]
        private static unsafe bool Hook_CBaseEntity_Touch(nint @ent, nint @act)
        {
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@ent) is not { IsValidEntity: true } entity) return false;
            if (_entities!.MakeEntityFromPointer<IBaseEntity>(@act) is not { IsValidEntity: true } activator) return false;

            if (AW.g_WhiteList != null && AW.g_WhiteList.Triggers.Count > 0)
            {
                foreach(var triggerid in AW.g_WhiteList.Triggers)
                {
                    if (string.Equals(triggerid, entity.HammerId, StringComparison.OrdinalIgnoreCase)) return CBaseEntity_Touch(@ent, @act);
                }
            }

            if ((Cvar.TriggerWatchOnce || Cvar.TriggerWatchMultiple) && activator.AsPlayerPawn() is { } pawn && pawn.GetController() is { } player && player.GetGameClient() is { } client && AW.CheckDictionary(client) && AW.g_AWPlayer[client].TriggerBannedPlayer.bTriggerBanned)
            {
                if (Cvar.TriggerWatchOnce && entity.Classname.StartsWith("trigger_o")) return false;
                else if (Cvar.TriggerWatchMultiple) return false;
            }
            return CBaseEntity_Touch(@ent, @act);
        }

        private static EHookAction OnTriggerTouch(IBaseEntity entity, IBaseEntity? activator)
        {
            if (!Cvar.TriggerGlobalEnable || activator == null || !activator.IsValid() || !activator.IsPlayerPawn) return default;

            if (activator.AsBasePlayerPawn() is { } pawn && pawn.GetController() is { } player && player.GetGameClient() is { } client)
            {
                if (entity.Classname.StartsWith("trigger_o"))//trigger_once
                {
                    if (Cvar.TriggerShowOnce) UI.AWChatActivationNotify(entity, client, false);
                    AW.g_cAWAPI.TriggerOnTriggerOnceTouch(client, entity.Name, !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}");
                }
                else //trigger_multiple
                {
                    if (Cvar.TriggerShowMultiple) UI.AWChatActivationNotify(entity, client, false);
                    AW.g_cAWAPI.TriggerOnTriggerMultipleTouch(client, entity.Name, !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}");
                }
            }

            return default;
        }

        private static EHookAction OnButtonPressed(IBaseEntity entity, IBaseEntity? activator)
        {
            if (!Cvar.ButtonGlobalEnable || activator == null || !activator.IsValid() || !activator.IsPlayerPawn) return default;

            bool bWatch = true;
            if (AW.g_WhiteList != null && AW.g_WhiteList.Buttons.Count > 0)
            {
                foreach (var buttonid in AW.g_WhiteList.Buttons)
                {
                    if (string.Equals(buttonid, entity.HammerId, StringComparison.OrdinalIgnoreCase))
                    {
                        bWatch = false;
                        break;
                    }
                }
            }

            //restrict + check EW
#if (USE_ENTWATCH)
            if (GetEntWatch() is { } _api && _api.Native_EntWatch_IsButtonSpecialItem(entity)) return default;
#endif

            if (activator.AsBasePlayerPawn() is { } pawn && pawn.GetController() is { } player && player.GetGameClient() is { } client)
            {
                if (entity.Classname.StartsWith("func_d"))//func_door, func_door_rotating
                {
                    if (bWatch && Cvar.ButtonWatchDoor && AW.CheckDictionary(client) && AW.g_AWPlayer[client].ButtonBannedPlayer.bBanned) return EHookAction.SkipCallReturnOverride;
                    if (Cvar.ButtonShowDoor) UI.AWChatActivationNotify(entity, client, true);
                    AW.g_cAWAPI.ButtonOnDoorPressed(client, entity.Name, !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}");
                } else if (entity.Classname.StartsWith("func_p"))//func_physbox
                {
                    if (bWatch && Cvar.ButtonWatchPhysbox && AW.CheckDictionary(client) && AW.g_AWPlayer[client].ButtonBannedPlayer.bBanned) return EHookAction.SkipCallReturnOverride;
                    if (Cvar.ButtonShowPhysbox) UI.AWChatActivationNotify(entity, client, true);
                    AW.g_cAWAPI.ButtonOnPhysboxPressed(client, entity.Name, !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}");
                }
                else //func_button, func_rot_button
                {
                    if (bWatch && Cvar.ButtonWatchButton && AW.CheckDictionary(client) && AW.g_AWPlayer[client].ButtonBannedPlayer.bBanned) return EHookAction.SkipCallReturnOverride;
                    if (Cvar.ButtonShowButton) UI.AWChatActivationNotify(entity, client, true);
                    AW.g_cAWAPI.ButtonOnButtonPressed(client, entity.Name, !string.IsNullOrWhiteSpace(entity.HammerId) ? entity.HammerId : $"_{entity.Index}");
                }
            }
            return default;
        }

        public static void TimerRetry() => ActBanDB.CheckConnection();

        public static void TimerUnban()
        {
            int iTime = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            if (Cvar.ButtonGlobalEnable) ActBanDB.OfflineUnban(iTime, true);
            if (Cvar.TriggerGlobalEnable) ActBanDB.OfflineUnban(iTime, false);

            Task.Run(() =>
            {
                OfflineFunc.TimeToClear();
            });

            //Update (Un)Bans
            if (Cvar.ButtonGlobalEnable)
            {
                Task.Run(() =>
                {
                    Parallel.ForEach(AW.g_AWPlayer, (pair) =>
                    {
                        if (pair.Value.ButtonBannedPlayer.iDuration > 0 && pair.Value.ButtonBannedPlayer.iTimeStamp_Issued < iTime) pair.Value.ButtonBannedPlayer.bBanned = false;
                        ActBanPlayer.GetBan(pair.Key, true);
                    });
                });
            }

            if (Cvar.TriggerGlobalEnable)
            {
                Task.Run(() =>
                {
                    Parallel.ForEach(AW.g_AWPlayer, (pair) =>
                    {
                        if (pair.Value.TriggerBannedPlayer.iDuration > 0 && pair.Value.TriggerBannedPlayer.iTimeStamp_Issued < iTime) pair.Value.TriggerBannedPlayer.bBanned = false;
                        ActBanPlayer.GetBan(pair.Key, false);
                    });
                });
            }
        }
    }
}
