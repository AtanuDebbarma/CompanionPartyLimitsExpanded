# Companion & Party Limits Expanded

A Mount & Blade II: Bannerlord mod that replaces the base game's companion limit and clan party limit with values you set in [Mod Configuration Menu (MCM)][mcm].

**Download:** [Companion & Party Limits Expanded on Nexus Mods][nexus]

## Why this mod exists

In the base game, a clan can have clan tier + 3 companions, plus one for each of two perks, and can field 1 to 3 parties depending on its tier. Growing a large clan of companions and party leaders means waiting for clan tiers and picking specific perks. This mod lets you set both limits yourself, or remove them.

## Base game vs. this mod

| Limit | Base game | With this mod (defaults) |
|---|---|---|
| Companions | Clan tier + 3, +1 for the Leadership perk *We Pledge Our Swords*, +1 for the Charm perk *Camaraderie* | 20, adjustable from 10 to 100, or unlimited |
| Clan parties | 1 below clan tier 3, 2 below tier 5, otherwise 3, plus perk bonuses. Minor factions use their own scale | 10, adjustable from 10 to 100, or unlimited |
| Party size (troops per party) | Unchanged | Unchanged |

The leader's own party counts toward the party limit, as in the base game.

## Mod options ([MCM][mcm])

All settings are under **Options > Mod Options > Companion & Party Limits Expanded** and apply without restarting. There are three groups, each with the same four settings:

- *Unlimited companions* (off by default) and *Companion limit* (10 to 100, default 20);
- *Unlimited parties* (off by default) and *Party limit* (10 to 100, default 10).

| Group | Covers | Default |
|---|---|---|
| **Your clan** | Your own clan | Always active |
| **Your kingdom's clans** | The other clans of the kingdom you belong to. Does nothing while you are not in a kingdom | Off |
| **AI clans** | Every other AI clan, including your kingdom's clans while *Your kingdom's clans* is off | Off |

The two AI groups are off by default to keep the game's balance: until you switch a group on, its clans keep the base game limits, whatever its sliders are set to. More AI parties make armies larger and can change war decisions. Bandits are never affected.

The clan screen shows *Unlimited* when a limit of your clan is removed.

AI diplomacy is unaffected. When the game decides whether a clan joins or leaves a kingdom, whether mercenaries join or leave a kingdom, and whether to declare war or make peace, it uses the base game's party limits. Higher limits do not make lords refuse to join your kingdom or make a kingdom look stronger than its armies; the parties clans actually field still count in full. In the story's final phase, the conspiracy's new lords are also grouped into clans by the base game's party limits.

The values replace the base game limits instead of adding to them. A setting below what your clan tier and perks would give lowers your limit. Lowering a limit or switching a group off never removes companions or parties you already have; you just cannot add more until you are under it. While a clan has more companions than its setting, its companion limit equals the number it has, because the base game removes companions above the limit.

## Requirements and versions

| Dependency | Tested version | Expected to work with |
|---|---|---|
| Mount & Blade II: Bannerlord | 1.4.8 | 1.4.x |
| War Sails (NavalDLC), optional | 1.2.8 | 1.2.x |
| [Harmony][harmony] | 2.4.2 | 2.x |
| [ButterLib][butterlib] | 2.12.0 | 2.x |
| [UIExtenderEx][uiextenderex] | 3.0.3 | 3.x, as required by MCM 5.13 |
| [Mod Configuration Menu v5 (MCM)][mcm] | 5.13.1 | 5.x |

This mod needs [Harmony][harmony] and [MCM][mcm] directly. [MCM][mcm] in turn needs [ButterLib][butterlib] and [UIExtenderEx][uiextenderex].

The mod changes the game only through the base game's public classes and methods. If a Bannerlord update changes one of them, the affected limit stays at the base game value and a red message in game names it, instead of the game crashing.

## Compatibility

- **Other mods that change the companion limit or the party limit may conflict with this one.** Whichever change runs last wins, so one of the two mods may seem to do nothing. Use only one mod for these limits.
- **Mods that replace the base game's clan tier model or diplomacy model** work: this mod changes the models the game actually uses.
- **Mods that only read the limits**, for example to check whether you can hire another companion, see the values set here.
- **War Sails** is supported without being required. With *Your kingdom's clans* or *AI clans* on, the AI clans they cover keep more ships, because War Sails bases their ideal fleet size on the party limit.
- **[Diplomacy][diplomacy]** works alongside this mod. It does not replace the models this mod changes, and the two mods patch different parts of the game. Its war, peace, alliance and tribute decisions go through the base game's scores, which use the base game's party limits as described above, and its own strength figures count the troops clans actually field.

## Translations

The mod ships in English. All of its text, including every [MCM][mcm] setting and hint, is in `ModuleData/Languages/EN/cple_strings_en.xml`. Anyone may publish a translation as a separate mod that requires Companion & Party Limits Expanded, without changing its files. [TRANSLATORS.md](TRANSLATORS.md) explains how.

## Installation

1. Install [Harmony][harmony], [ButterLib][butterlib], [UIExtenderEx][uiextenderex] and [MCM][mcm].
2. Extract the `CompanionPartyLimitsExpanded` folder into `Mount & Blade II Bannerlord\Modules`.
3. Start the game launcher. Enable Companion & Party Limits Expanded and place it after [Harmony][harmony] and [MCM][mcm].

The mod can be added to an existing campaign.

## Removing the mod from a campaign

The mod adds nothing to saves, so a save still loads without it, and the base game limits return. Parties above the base game limit stay, but you cannot create more until you are under it.

Companions are different. With more companions than the base game limit (clan tier + 3, plus perk bonuses), the base game warns you, removes a random companion for good each day starting six days later, and cancels quests that would give you a companion. Before removing the mod, dismiss companions down to the base game limit.

## Building from source

The repository contains only source code. The TaleWorlds, [Harmony][harmony] and [MCM][mcm] DLLs are not included; the build references them from your game installation.

Requirements:

- Windows with .NET Framework 4.7.2 or later.
- Visual Studio 2019 or later, or the Visual Studio Build Tools. The .NET Framework's own MSBuild also works.
- Bannerlord with [Harmony][harmony] and [MCM][mcm] installed.

The code is limited to C# 5 so it builds with the .NET Framework compiler too.

### Point the build at your game

Use one of the following:

- Copy `CompanionPartyLimitsExpanded.user.props.example` to `CompanionPartyLimitsExpanded.user.props` and set `BannerlordDir`. Git ignores this file.
- Set the `BANNERLORD_DIR` environment variable.
- Pass `/p:BannerlordDir="..."` to MSBuild.

If none is set, the default Steam folder `C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord` is used.

### Build

```bat
build.cmd
```

The DLL is written to `bin\Win64_Shipping_Client\CompanionPartyLimitsExpanded.dll`, so the repository folder has the same layout as the installed module.

To also copy `SubModule.xml`, `ModuleData` and the DLL into the game's `Modules\CompanionPartyLimitsExpanded` folder after each build, set `DeployToGame` to `true` in your `CompanionPartyLimitsExpanded.user.props`, or run:

```bat
build.cmd /p:DeployToGame=true
```

Close the game first, otherwise the DLL is locked.

## Repository layout

```
CompanionPartyLimitsExpanded.csproj               Project file
CompanionPartyLimitsExpanded.user.props.example   Template for local build settings
build.cmd                                         Finds MSBuild and builds the project
SubModule.xml                                     Module definition loaded by the game
ModuleData/Languages/EN/                          English text, the template for translations
src/                                              C# source
TRANSLATORS.md                                    How to make a translation mod
```

## License and credit

Copyright (C) 2026 AtanuDebbarma.

Companion & Party Limits Expanded is free software under the [GNU General Public License v3.0](LICENSE), with an additional attribution term described in [NOTICE](NOTICE):

- **Using or modifying this code:** if you publish a mod or other work that uses or is based on this code, you must credit **Companion & Party Limits Expanded by AtanuDebbarma** with a link to [this repository][repo], in your source code and in your mod's documentation. Your work must also be released under the GPL v3.
- **Using it as inspiration:** if this project inspired your work without you copying its code, a credit and link are kindly requested.

Mount & Blade II: Bannerlord is a trademark of TaleWorlds Entertainment. This project is not affiliated with or endorsed by TaleWorlds.

[nexus]: https://www.nexusmods.com/mountandblade2bannerlord/mods/14288
[diplomacy]: https://www.nexusmods.com/mountandblade2bannerlord/mods/832
[mcm]: https://www.nexusmods.com/mountandblade2bannerlord/mods/612
[butterlib]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2018?tab=description
[harmony]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2006?tab=description
[uiextenderex]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2102?tab=description
[repo]: https://github.com/AtanuDebbarma/CompanionPartyLimitsExpanded
