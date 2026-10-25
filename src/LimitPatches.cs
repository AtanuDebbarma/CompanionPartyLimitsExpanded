using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace CompanionPartyLimitsExpanded
{
    internal static class LimitPatches
    {
        // Stands in for "no limit". The diplomacy model sums and scales party limits, so int.MaxValue would overflow.
        internal const int Unlimited = 1000;

        private static readonly HashSet<MethodBase> Patched = new HashSet<MethodBase>();

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void PatchActiveModel(Harmony harmony)
        {
            ClanTierModel model = Campaign.Current != null ? Campaign.Current.Models.ClanTierModel : null;
            if (model == null)
            {
                CompanionPartyLimitsSubModule.ReportProblem("No clan tier model found; limits are unchanged.");
                return;
            }

            Type type = model.GetType();
            Patch(harmony, Override(type, SymbolExtensions.GetMethodInfo((ClanTierModel m) => m.GetCompanionLimit(null))), "CompanionLimitPostfix");
            Patch(harmony, Override(type, SymbolExtensions.GetMethodInfo((ClanTierModel m) => m.GetPartyLimitForTier(null, 0))), "PartyLimitPostfix");
        }

        // Replacement models from other mods override the base method, so patch the version the active model runs.
        private static MethodInfo Override(Type type, MethodInfo baseMethod)
        {
            return type.GetMethod(baseMethod.Name, baseMethod.GetParameters().Select(p => p.ParameterType).ToArray());
        }

        private static void Patch(Harmony harmony, MethodInfo target, string postfix)
        {
            if (target == null || target.IsAbstract)
            {
                CompanionPartyLimitsSubModule.ReportProblem("Could not find the method for " + postfix + "; that limit is unchanged.");
                return;
            }

            if (!Patched.Add(target))
            {
                return;
            }

            try
            {
                harmony.Patch(target, postfix: new HarmonyMethod(typeof(LimitPatches).GetMethod(postfix, BindingFlags.Static | BindingFlags.NonPublic)));
            }
            catch (Exception e)
            {
                Patched.Remove(target);
                CompanionPartyLimitsSubModule.ReportProblem("Patching " + target.DeclaringType + "." + target.Name + " failed: " + e.Message);
            }
        }

        private enum ClanGroup
        {
            None,
            Player,
            Kingdom,
            AI
        }

        // __0 instead of a parameter name: replacement models may name the clan parameter differently.
        private static void CompanionLimitPostfix(Clan __0, ref int __result)
        {
            LimitsSettings settings = LimitsSettings.Instance;
            if (settings == null)
            {
                return;
            }

            switch (GroupOf(__0, settings))
            {
                case ClanGroup.Player:
                    __result = settings.UnlimitedCompanions ? Unlimited : settings.CompanionLimit;
                    break;
                case ClanGroup.Kingdom:
                    __result = settings.KingdomUnlimitedCompanions ? Unlimited : settings.KingdomCompanionLimit;
                    break;
                case ClanGroup.AI:
                    __result = settings.AIUnlimitedCompanions ? Unlimited : settings.AICompanionLimit;
                    break;
            }
        }

        private static void PartyLimitPostfix(Clan __0, ref int __result)
        {
            LimitsSettings settings = LimitsSettings.Instance;
            if (settings == null)
            {
                return;
            }

            switch (GroupOf(__0, settings))
            {
                case ClanGroup.Player:
                    __result = settings.UnlimitedParties ? Unlimited : settings.PartyLimit;
                    break;
                case ClanGroup.Kingdom:
                    __result = settings.KingdomUnlimitedParties ? Unlimited : settings.KingdomPartyLimit;
                    break;
                case ClanGroup.AI:
                    __result = settings.AIUnlimitedParties ? Unlimited : settings.AIPartyLimit;
                    break;
            }
        }

        private static ClanGroup GroupOf(Clan clan, LimitsSettings settings)
        {
            if (clan == null || clan.IsBanditFaction)
            {
                return ClanGroup.None;
            }

            Clan playerClan = Clan.PlayerClan;
            if (clan == playerClan)
            {
                return ClanGroup.Player;
            }

            Kingdom playerKingdom = playerClan != null ? playerClan.Kingdom : null;
            if (settings.KingdomClans && playerKingdom != null && clan.Kingdom == playerKingdom)
            {
                return ClanGroup.Kingdom;
            }

            return settings.AIClans ? ClanGroup.AI : ClanGroup.None;
        }
    }
}
