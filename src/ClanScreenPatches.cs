using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CompanionPartyLimitsExpanded
{
    // The clan screen only shows the player clan, so "Unlimited" replaces the internal stand-in number there.
    internal static class ClanScreenPatches
    {
        private static string UnlimitedText()
        {
            return new TextObject("{=cple_unlimited}Unlimited").ToString();
        }

        private static void MembersPostfix(ClanMembersVM __instance)
        {
            LimitsSettings settings = LimitsSettings.Instance;
            if (settings == null || !settings.UnlimitedCompanions || Clan.PlayerClan == null)
            {
                return;
            }

            GameTexts.SetVariable("STR1", GameTexts.FindText("str_companions_group"));
            GameTexts.SetVariable("LEFT", Clan.PlayerClan.Companions.Count);
            GameTexts.SetVariable("RIGHT", UnlimitedText());
            GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis"));
            __instance.CompanionsText = GameTexts.FindText("str_STR1_space_STR2").ToString();
        }

        private static void PartiesPostfix(ClanPartiesVM __instance)
        {
            LimitsSettings settings = LimitsSettings.Instance;
            if (settings == null || !settings.UnlimitedParties || Clan.PlayerClan == null)
            {
                return;
            }

            GameTexts.SetVariable("CURRENT", Clan.PlayerClan.WarPartyComponents.Count);
            GameTexts.SetVariable("LIMIT", UnlimitedText());
            __instance.PartiesText = GameTexts.FindText("str_clan_parties").ToString();
        }
    }
}
