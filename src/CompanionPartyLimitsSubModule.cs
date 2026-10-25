using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        // The patching methods are kept separate and not inlined: if a game update removes a method they name,
        // the failure happens when they are compiled and is caught here instead of stopping the game.
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                PatchClanScreen();
            }
            catch (Exception e)
            {
                ReportProblem("The clan screen could not be patched: " + e.Message);
            }
        }

        // Models are final here, including replacements registered by other mods.
        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            if (!(game.GameType is Campaign))
            {
                return;
            }

            try
            {
                LimitPatches.PatchActiveModel(_harmony);
            }
            catch (Exception e)
            {
                ReportProblem("The limits could not be patched and stay at the base game values: " + e.Message);
            }

            ShowPendingProblems();
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void PatchClanScreen()
        {
            PatchClanScreenMethod(SymbolExtensions.GetMethodInfo((ClanMembersVM vm) => vm.RefreshMembersList()), "MembersPostfix");
            PatchClanScreenMethod(SymbolExtensions.GetMethodInfo((ClanPartiesVM vm) => vm.RefreshPartiesList()), "PartiesPostfix");
        }

        private void PatchClanScreenMethod(MethodInfo target, string postfix)
        {
            try
            {
                _harmony.Patch(target, postfix: new HarmonyMethod(typeof(ClanScreenPatches).GetMethod(postfix, BindingFlags.Static | BindingFlags.NonPublic)));
            }
            catch (Exception e)
            {
                ReportProblem("Clan screen patch " + postfix + " failed: " + e.Message);
            }
        }
    }
}
