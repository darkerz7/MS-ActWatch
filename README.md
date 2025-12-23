# [Core]MS-ActWatch for ModSharp
Notify players about button and trigger(Activator) interactions

## Features:
1. Async functions
2. SQLite/MySQL/PostgreSQL support
3. Language setting for players
4. Allows you to set up individual access for admins
5. Keeps logs to a file and discord
6. Online/Offline ban/unban of button press/trigger touch
7. API for interaction with other plugins
8. Allows you to select the player display format

## Required packages:
1. [ModSharp](https://github.com/Kxnrl/modsharp-public)
2. [ClientPreferences](https://github.com/Kxnrl/modsharp-public/tree/master/Sharp.Modules/ClientPreferences)
3. [LocalizerManager](https://github.com/Kxnrl/modsharp-public/tree/master/Sharp.Modules/LocalizerManager)
4. [AnyBaseLib](https://github.com/darkerz7/MS-AnyBaseLib-Shared)

## Installation:
1. Install `ClientPreferences`, `LocalizerManager` and `MS-AnyBaseLib-Shared`
2. Compile or copy MS-ActWatch to `sharp/modules/MS-ActWatch` folger. To compile without EntWatch remove DefineConstants `USE_ENTWATCH` (Project Properties -> Build -> Conditional compilation symbols)
3. Copy and configure the configuration file `db_config.json` and `log_config.json` to `sharp/modules/MS-ActWatch` folger
4. Copy `ActWatch.json` to `sharp/locales` folger
5. Copy and configure `mapsconfig` and `schemes` to `sharp/modules/MS-ActWatch/maps` and `sharp/modules/MS-ActWatch/scheme` folger
6. Compile or copy MS-ActWatch-Shared to `sharp/shared/MS-ActWatch-Shared` folger
7. Add CVARs to server.cfg
8. Restart server

## Admin privileges
Privilege | Description
--- | ---
`aw_reload` | Allows you to reload the plugin config
`aw_ban` | Allows access to Offline list
`bw_ban` | Allows access to button press bans (Command)
`bw_ban_perm` | Allows access to permanent button press bans (Duration 0)
`bw_ban_long` | Allows access to long button press bans (Cvar awc_bbanlong)
`bw_unban` | Allows access to button press unbans (Command)
`bw_unban_perm` | Allows access to permanent button press unbans (Duration 0)
`bw_unban_other` | Allows access to button press unbans from other admins
`tw_ban` | Allows access to trigger touch bans (Command)
`tw_ban_perm` | Allows access to permanent trigger touch bans (Duration 0)
`tw_ban_long` | Allows access to long trigger touch bans (Cvar awc_tbanlong)
`tw_unban` | Allows access to trigger touch unbans (Command)
`tw_unban_perm` | Allows access to permanent trigger touch unbans (Duration 0)
`tw_unban_other` | Allows access to trigger touch unbans from other admins

## CVARs
Cvar | Parameters | Description
--- | --- | ---
`ms_awc_bbantime` | `<0-43200>` | Default button press ban time. 0 - Permanent. (Default 0)
`ms_awc_bbanlong` | `<1-1440000>` | Max button press ban time with once @css/bw_ban privilege. (Default 720)
`ms_awc_bbanreason` | `<string>` | Default button press ban reason. (Default Trolling)
`ms_awc_bunbanreason` | `<string>` | Default button press unban reason. (Default Giving another chance)
`ms_awc_bkeep_expired_ban` | `<false-true>` | Enable/Disable keep expired button press bans. (Default true)
`ms_awc_benable` | `<false-true>` | Enable/Disable button press functionality. (Default true)
`ms_awc_bshow_button` | `<false-true>` | Enable/Disable display of func_(rot_)button presses. (Default true)
`ms_awc_bshow_door` | `<false-true>` | Enable/Disable display of func_door(_rotating) presses. (Default true)
`ms_awc_bshow_physbox` | `<false-true>` | Enable/Disable display of func_physbox presses. (Default true)
`ms_awc_bwatch_button` | `<false-true>` | Enable/Disable watch of func_(rot_)button presses. Do bans affect. (Default true)
`ms_awc_bwatch_door` | `<false-true>` | Enable/Disable watch of func_door(_rotating) presses. Do bans affect. (Default true)
`ms_awc_bwatch_physbox` | `<false-true>` | Enable/Disable watch of func_physbox presses. Do bans affect. (Default true)
`ms_awc_tbantime` | `<0-43200>` | Default trigger touch ban time. 0 - Permanent. (Default 0)
`ms_awc_tbanlong` | `<1-1440000>` | Max trigger touch ban time with once @css/tw_ban privilege. (Default 720)
`ms_awc_tbanreason` | `<string>` | Default trigger touch ban reason. (Default Trolling)
`ms_awc_tunbanreason` | `<string>` | Default trigger touch unban reason. (Default Giving another chance)
`ms_awc_tkeep_expired_ban` | `<false-true>` | Enable/Disable keep expired trigger touch bans. (Default true)
`ms_awc_tenable` | `<false-true>` | Enable/Disable trigger touch functionality. (Default true)
`ms_awc_tshow_once` | `<false-true>` | Enable/Disable display of trigger_once touching. (Default true)
`ms_awc_tshow_multiple` | `<false-true>` | Enable/Disable display of trigger_multiple touching. (Default false)
`ms_awc_twatch_once` | `<false-true>` | Enable/Disable watch of trigger_once touching. Do bans affect.(BUG) When touched by a banned trigger disappears. (Default false)
`ms_awc_twatch_multiple` | `<false-true>` | Enable/Disable watch of trigger_multiple touching. Do bans affect. (Default false)
`ms_awc_offline_clear_time` | `<1-240>` | Time during which data is stored. (Default 30)
`ms_awc_player_format` | `<0-3>` | Changes the way player information is displayed by default (0 - Only Nickname, 1 - Nickname and UserID, 2 - Nickname and SteamID, 3 - Nickname, UserID and SteamID). (Default 3)
`ms_awc_scheme_name` | `<string>` | Filename for the scheme. (Default default.json)
`ms_awc_lower_mapname` | `<false-true>` | Automatically lowercase map name. (Default false)
`ms_awc_server_lang` | `<string>` | Specify the language into which the server messages should be translated. (Default en-us)

## Commands
Client Command | Description
--- | ---
`ms_buttons` | Allows players to toggle the button press display
`ms_triggers` | Allows players to toggle the trigger touch display
`ms_apf` | Allows the player to change the player display format (0 - Only Nickname, 1 - Nickname and UserID, 2 - Nickname and SteamID, 3 - Nickname, UserID and SteamID)
`ms_bstatus` | Allows the player to view the button press ban {null/target}
`ms_trstatus` | Allows the player to view the trigger touch ban {null/target}

## Admin's commands
Admin Command | Privilege | Description
--- | --- | ---
`ms_areloadwl` | `aw_reload` | Reloads whitelist config
`ms_ashowwl` | `aw_reload` | Shows the whitelist
`ms_areloadscheme` | `aw_reload` | Reloads Scheme config
`ms_ashowscheme` | `aw_reload` | Shows the Scheme
`ms_bban` | `bw_ban`+`bw_ban_perm`+`bw_ban_long` | Allows the admin to button press bans for the player `<#userid/name/#steamid> [<time>] [<reason>]`
`ms_bunban` | `bw_unban`+`bw_unban_perm`+`bw_unban_other` | Allows the admin to remove button press ban for a player `<#userid/name/#steamid> [<reason>]`
`ms_bbanlist` | `bw_ban` | Displays a list of button press bans
`ms_trban` | `tw_ban`+`tw_ban_perm`+`tw_ban_long` | Allows the admin to trigger touch bans for the player `<#userid/name/#steamid> [<time>] [<reason>]`
`ms_trunban` | `tw_unban`+`tw_unban_perm`+`tw_unban_other` | Allows the admin to remove trigger touch ban for a player `<#userid/name/#steamid> [<reason>]`
`ms_trbanlist` | `tw_ban` | Displays a list of trigger touch bans
`ms_alist` | `aw_ban` | Shows a list of players including those who have disconnected