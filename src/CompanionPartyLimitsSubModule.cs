using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CompanionPartyLimitsExpanded
{
    public class CompanionPartyLimitsSubModule : MBSubModuleBase
    {
        private const string LogPrefix = "[Companion & Party Limits Expanded] ";

        private static readonly List<string> PendingProblems = new List<string>();

        private readonly Harmony _harmony = new Harmony("companion.party.limits.expanded");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            PatchClanScreen(SymbolExtensions.GetMethodInfo((ClanMembersVM vm) => vm.RefreshMembersList()), "MembersPostfix");
            PatchClanScreen(SymbolExtensions.GetMethodInfo((ClanPartiesVM vm) => vm.RefreshPartiesList()), "PartiesPostfix");
        }

        // Models are final here, including replacements registered by other mods.
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            if (game.GameType is Campaign)
            {
                LimitPatches.PatchActiveModel(_harmony);
                ShowPendingProblems();
            }
        }

        // Debug.Print does not reach the game logs, so problems are also shown in game.
        internal static void ReportProblem(string message)
        {
            Debug.Print(LogPrefix + message);
            PendingProblems.Add(message);
        }

        private static void ShowPendingProblems()
        {
            foreach (string message in PendingProblems)
            {
                InformationManager.DisplayMessage(new InformationMessage(LogPrefix + message, Colors.Red));
            }

            PendingProblems.Clear();
        }

        private void PatchClanScreen(System.Reflection.MethodInfo target, string postfix)
        {
            try
            {
                _harmony.Patch(target, postfix: new HarmonyMethod(typeof(ClanScreenPatches).GetMethod(postfix, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)));
            }
            catch (Exception e)
            {
                ReportProblem("Clan screen patch " + postfix + " failed: " + e.Message);
            }
        }
    }
}
