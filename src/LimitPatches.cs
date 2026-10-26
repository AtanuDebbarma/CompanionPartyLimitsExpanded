using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using StoryMode.Quests.ThirdPhase;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Localization;

namespace CompanionPartyLimitsExpanded
{
    internal static class LimitPatches
    {
        // Stands in for "no limit". The diplomacy model sums and scales party limits, so int.MaxValue would overflow.
        internal const int Unlimited = 1000;

        private static readonly HashSet<MethodBase> Patched = new HashSet<MethodBase>();

        // Above zero while the AI scores recruiting lords, hiring mercenaries, or going to war or making peace, and while
        // the story's final phase groups the conspiracy's new lords into clans. Those treat party limits as a measure of
        // how strong a clan or kingdom is, or how many lords a clan holds, so they are given the active model's own
        // limits instead of the expanded ones.
        [ThreadStatic]
        private static int _baseLimitsDepth;

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
            PatchDiplomacyScores(harmony);

            try
            {
                PatchConspiracyClans(harmony);
            }
            catch (Exception e)
            {
                CompanionPartyLimitsSubModule.ReportProblem("The story's conspiracy clans could not be patched: " + e.Message);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void PatchConspiracyClans(Harmony harmony)
        {
            UseBaseLimits(
                harmony,
                AccessTools.Method(typeof(DefeatTheConspiracyQuestBehavior), "InitializeFinalPhase"),
                "Could not find the story's final phase; with AI clans on, the conspiracy may form fewer, larger clans.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void PatchDiplomacyScores(Harmony harmony)
        {
            DiplomacyModel model = Campaign.Current.Models.DiplomacyModel;
            if (model == null)
            {
                CompanionPartyLimitsSubModule.ReportProblem("No diplomacy model found; AI diplomacy sees the expanded party limits.");
                return;
            }

            Type type = model.GetType();
            TextObject reason;
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfClanToJoinKingdom(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfClanToLeaveKingdom(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfKingdomToGetClan(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfKingdomToSackClan(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfMercenaryToJoinKingdom(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfMercenaryToLeaveKingdom(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfKingdomToHireMercenary(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfDeclaringWar(null, null, null, out reason, false))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfDeclaringPeace(null, null))));
            PatchDiplomacyScore(harmony, Override(type, SymbolExtensions.GetMethodInfo((DiplomacyModel m) => m.GetScoreOfDeclaringPeaceForClan(null, null, null, out reason, false))));
        }

        private static void PatchDiplomacyScore(Harmony harmony, MethodInfo target)
        {
            UseBaseLimits(harmony, target, "Could not find an AI diplomacy score; AI diplomacy may see the expanded party limits.");
        }

        private static void UseBaseLimits(Harmony harmony, MethodInfo target, string missingMessage)
        {
            if (target == null || target.IsAbstract)
            {
                CompanionPartyLimitsSubModule.ReportProblem(missingMessage);
                return;
            }

            if (!Patched.Add(target))
            {
                return;
            }

            try
            {
                harmony.Patch(
                    target,
                    prefix: new HarmonyMethod(typeof(LimitPatches).GetMethod("BaseLimitsPrefix", BindingFlags.Static | BindingFlags.NonPublic)),
                    finalizer: new HarmonyMethod(typeof(LimitPatches).GetMethod("BaseLimitsFinalizer", BindingFlags.Static | BindingFlags.NonPublic)));
            }
            catch (Exception e)
            {
                Patched.Remove(target);
                CompanionPartyLimitsSubModule.ReportProblem("Patching " + target.DeclaringType + "." + target.Name + " failed: " + e.Message);
            }
        }

        private static void BaseLimitsPrefix(out bool __state)
        {
            _baseLimitsDepth++;
            __state = true;
        }

        // A finalizer runs even when the score throws, so the depth always returns to where it was.
        private static Exception BaseLimitsFinalizer(Exception __exception, bool __state)
        {
            if (__state)
            {
                _baseLimitsDepth--;
            }

            return __exception;
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

            int limit;
            switch (GroupOf(__0, settings))
            {
                case ClanGroup.Player:
                    limit = settings.UnlimitedCompanions ? Unlimited : settings.CompanionLimit;
                    break;
                case ClanGroup.Kingdom:
                    limit = settings.KingdomUnlimitedCompanions ? Unlimited : settings.KingdomCompanionLimit;
                    break;
                case ClanGroup.AI:
                    limit = settings.AIUnlimitedCompanions ? Unlimited : settings.AICompanionLimit;
                    break;
                default:
                    return;
            }

            // The base game permanently removes companions above the limit a week after warning, so a lowered
            // setting stops new hires but never drops below the companions the clan already has.
            __result = Math.Max(limit, __0.Companions.Count);
        }

        private static void PartyLimitPostfix(Clan __0, ref int __result)
        {
            LimitsSettings settings = LimitsSettings.Instance;
            if (settings == null || _baseLimitsDepth > 0)
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
