using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Localization;

namespace CompanionPartyLimitsExpanded
{
    public sealed class LimitsSettings : AttributeGlobalSettings<LimitsSettings>
    {
        // Translation IDs are public: translation modules depend on them, so keep each ID once released.
        private const string GroupYourClan = "{=cple_mcm_group_your_clan}Your clan";
        private const string GroupKingdomClans = "{=cple_mcm_group_kingdom_clans}Your kingdom's clans";
        private const string GroupAIClans = "{=cple_mcm_group_ai_clans}AI clans";

        // Shared by every group: the setting does the same thing for whichever clans the group covers.
        private const string UnlimitedCompanionsName = "{=cple_mcm_unlimited_companions_name}Unlimited companions";
        private const string UnlimitedCompanionsHint = "{=cple_mcm_unlimited_companions_hint}Off by default. Removes the companion limit. The slider below is ignored while this is on.";
        private const string CompanionLimitName = "{=cple_mcm_companion_limit_name}Companion limit";
        private const string CompanionLimitHint = "{=cple_mcm_companion_limit_hint}How many companions a clan can have. Default 20. Replaces the base game limit from clan tier and perks.";
        private const string UnlimitedPartiesName = "{=cple_mcm_unlimited_parties_name}Unlimited parties";
        private const string UnlimitedPartiesHint = "{=cple_mcm_unlimited_parties_hint}Off by default. Removes the party limit. The slider below is ignored while this is on.";
        private const string PartyLimitName = "{=cple_mcm_party_limit_name}Party limit";
        private const string PartyLimitHint = "{=cple_mcm_party_limit_hint}How many parties a clan can field, counting the leader's own party. Default 10. Replaces the base game limit from clan tier and perks. Party size is not changed.";

        public override string Id { get { return "CompanionPartyLimitsExpanded"; } }
        public override string DisplayName { get { return new TextObject("{=cple_mcm_title}Companion & Party Limits Expanded").ToString(); } }
        public override string FolderName { get { return "CompanionPartyLimitsExpanded"; } }
        public override string FormatType { get { return "json2"; } }

        public LimitsSettings()
        {
            CompanionLimit = 20;
            PartyLimit = 10;
            KingdomCompanionLimit = 20;
            KingdomPartyLimit = 10;
            AICompanionLimit = 20;
            AIPartyLimit = 10;
        }

        [SettingPropertyBool(UnlimitedCompanionsName, Order = 0, RequireRestart = false, HintText = UnlimitedCompanionsHint)]
        [SettingPropertyGroup(GroupYourClan, GroupOrder = 0)]
        public bool UnlimitedCompanions { get; set; }

        [SettingPropertyInteger(CompanionLimitName, 10, 100, "0", Order = 1, RequireRestart = false, HintText = CompanionLimitHint)]
        [SettingPropertyGroup(GroupYourClan)]
        public int CompanionLimit { get; set; }

        [SettingPropertyBool(UnlimitedPartiesName, Order = 2, RequireRestart = false, HintText = UnlimitedPartiesHint)]
        [SettingPropertyGroup(GroupYourClan)]
        public bool UnlimitedParties { get; set; }

        [SettingPropertyInteger(PartyLimitName, 10, 100, "0", Order = 3, RequireRestart = false, HintText = PartyLimitHint)]
        [SettingPropertyGroup(GroupYourClan)]
        public int PartyLimit { get; set; }

        [SettingPropertyBool("{=cple_mcm_kingdom_clans_name}Your kingdom's clans", IsToggle = true, Order = 0, RequireRestart = false, HintText = "{=cple_mcm_kingdom_clans_hint}Off by default. The other clans of the kingdom you belong to use the limits below instead of the base game's. Does nothing while you are not in a kingdom.")]
        [SettingPropertyGroup(GroupKingdomClans, GroupOrder = 1)]
        public bool KingdomClans { get; set; }

        [SettingPropertyBool(UnlimitedCompanionsName, Order = 1, RequireRestart = false, HintText = UnlimitedCompanionsHint)]
        [SettingPropertyGroup(GroupKingdomClans)]
        public bool KingdomUnlimitedCompanions { get; set; }

        [SettingPropertyInteger(CompanionLimitName, 10, 100, "0", Order = 2, RequireRestart = false, HintText = CompanionLimitHint)]
        [SettingPropertyGroup(GroupKingdomClans)]
        public int KingdomCompanionLimit { get; set; }

        [SettingPropertyBool(UnlimitedPartiesName, Order = 3, RequireRestart = false, HintText = UnlimitedPartiesHint)]
        [SettingPropertyGroup(GroupKingdomClans)]
        public bool KingdomUnlimitedParties { get; set; }

        [SettingPropertyInteger(PartyLimitName, 10, 100, "0", Order = 4, RequireRestart = false, HintText = PartyLimitHint)]
        [SettingPropertyGroup(GroupKingdomClans)]
        public int KingdomPartyLimit { get; set; }

        [SettingPropertyBool("{=cple_mcm_ai_clans_name}AI clans", IsToggle = true, Order = 0, RequireRestart = false, HintText = "{=cple_mcm_ai_clans_hint}Off by default. AI clans use the limits below instead of the base game's. While Your kingdom's clans is on, your kingdom's clans use that group instead. More AI parties make armies larger and can change war decisions and how many ships AI clans keep. Bandits are never affected.")]
        [SettingPropertyGroup(GroupAIClans, GroupOrder = 2)]
        public bool AIClans { get; set; }

        [SettingPropertyBool(UnlimitedCompanionsName, Order = 1, RequireRestart = false, HintText = UnlimitedCompanionsHint)]
        [SettingPropertyGroup(GroupAIClans)]
        public bool AIUnlimitedCompanions { get; set; }

        [SettingPropertyInteger(CompanionLimitName, 10, 100, "0", Order = 2, RequireRestart = false, HintText = CompanionLimitHint)]
        [SettingPropertyGroup(GroupAIClans)]
        public int AICompanionLimit { get; set; }

        [SettingPropertyBool(UnlimitedPartiesName, Order = 3, RequireRestart = false, HintText = UnlimitedPartiesHint)]
        [SettingPropertyGroup(GroupAIClans)]
        public bool AIUnlimitedParties { get; set; }

        [SettingPropertyInteger(PartyLimitName, 10, 100, "0", Order = 4, RequireRestart = false, HintText = PartyLimitHint)]
        [SettingPropertyGroup(GroupAIClans)]
        public int AIPartyLimit { get; set; }
    }
}
