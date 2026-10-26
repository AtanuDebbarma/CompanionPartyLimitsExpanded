# Changelog

## 1.0.2

- Fixed the red message *Could not find the story's final phase* that appeared when loading a campaign. 1.0.1 looked for that step of the story in the wrong place, so with *AI clans* on, the conspiracy's new lords were still grouped into fewer, larger clans. They are now grouped as in the base game. Nothing else changed.

## 1.0.1

- Recruiting lords works again with higher party limits. The game scores a clan joining a kingdom lower the larger the kingdom's combined party limit is, so even the default limit of 10 made lords much harder to recruit, and *Unlimited parties* made it practically impossible. The scores for a clan joining or leaving a kingdom, and for a kingdom taking a clan, now use the party limits of the clan tier model the game uses, without this mod's change. Actual party limits, the clan screen and War Sails fleet sizes still use the values set here.
- AI kingdoms judge war and peace by real strength again. When deciding whether to declare war or make peace, the game counts each kingdom as at least as strong as its clans' combined party limits, so higher limits made kingdoms look far stronger than their armies, and *Unlimited parties* made one kingdom look unbeatable. These scores, and the scores for mercenaries joining or leaving a kingdom, for a kingdom hiring them and for expelling a clan, now also use the clan tier model's own party limits. The armies clans actually field still count in full.
- Lowering *Companion limit*, or switching off *Unlimited companions*, no longer costs you companions. Whenever your clan has more companions than its limit, the base game warns you, removes a random companion for good each day starting six days later, and cancels quests that would give you a companion. The companion limit now never drops below the number of companions the clan already has, so a lower setting only stops new hires.
- In the story's final phase, the conspiracy's new lords are grouped into clans by the base game's party limits again. With *AI clans* on, higher limits had put them into fewer, larger clans.
- Works alongside [Diplomacy](https://www.nexusmods.com/mountandblade2bannerlord/mods/832): its war, peace, alliance and tribute decisions go through the scores above.
- If one of these patches cannot be applied, a red message in game names it.
- *Removing the mod from a campaign* in the README now explains that the base game removes companions above its own limit, so they should be dismissed before the mod is removed.

## 1.0.0

First release.

- **Companion limit** replaces the base game's limit (clan tier + 3, plus perk bonuses) with a value from 10 to 100, default 20. **Unlimited companions** removes the limit.
- **Party limit** replaces the base game's clan party limit (1 to 3 by clan tier, plus perk bonuses) with a value from 10 to 100, default 10. The leader's own party counts toward it, as in the base game. **Unlimited parties** removes the limit. Party size is not changed.
- Settings in three groups, each with the same four settings. **Your clan** is always active. **Your kingdom's clans** covers the other clans of your kingdom, and **AI clans** covers every other AI clan, including your kingdom's clans while the kingdom group is off. Both AI groups are off by default, so AI clans keep the base game limits until you switch a group on. Bandits are never affected.
- The clan screen shows *Unlimited* when a limit of your clan is removed.
- Works with mods that replace the base game's clan tier model: the limits of the model the game actually uses are changed.
- If a part of the mod cannot attach to the game, a red message in game names it and that limit stays unchanged.
- Translation support. All of the mod's text is in `ModuleData/Languages/EN/cple_strings_en.xml`. [TRANSLATORS.md](TRANSLATORS.md) explains how to publish a translation as a separate mod.
- Translation IDs introduced in this version: `cple_mcm_title`, the `cple_mcm_group_*` group names, a `cple_mcm_*_name` and `cple_mcm_*_hint` pair for every setting, and `cple_unlimited`. The four limit settings use the same IDs in all three groups.
