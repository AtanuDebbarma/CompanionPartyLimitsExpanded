# Changelog

## 1.0.0

First release.

- **Companion limit** replaces the base game's limit (clan tier + 3, plus perk bonuses) with a value from 10 to 100, default 20. **Unlimited companions** removes the limit.
- **Party limit** replaces the base game's clan party limit (1 to 3 by clan tier, plus perk bonuses) with a value from 10 to 100, default 10. The leader's own party counts toward it, as in the base game. **Unlimited parties** removes the limit. Party size is not changed.
- Settings in three groups, each with the same four settings. **Your clan** is always active. **Your kingdom's clans** covers the other clans of your kingdom, and **AI clans** covers every other AI clan, including your kingdom's clans while the kingdom group is off. Both AI groups are off by default, so AI clans keep the base game limits until you switch a group on. Bandits are never affected.
- The clan screen shows *Unlimited* when a limit is removed.
- Works with mods that replace the base game's clan tier model: the limits of the model the game actually uses are changed.
- If a part of the mod cannot attach to the game, a red message in game names it and that limit stays unchanged.
- Translation support. All of the mod's text is in `ModuleData/Languages/EN/cple_strings_en.xml`. [TRANSLATORS.md](TRANSLATORS.md) explains how to publish a translation as a separate mod.
- Translation IDs introduced in this version: `cple_mcm_title`, the `cple_mcm_group_*` group names, a `cple_mcm_*_name` and `cple_mcm_*_hint` pair for every setting, and `cple_unlimited`. The four limit settings use the same IDs in all three groups.
