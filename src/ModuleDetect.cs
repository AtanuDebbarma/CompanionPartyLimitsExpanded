using System;
using TaleWorlds.ModuleManager;

namespace CompanionPartyLimitsExpanded
{
    // Soft optional support for Clan Tier Expanded (module id ClanTierExpanded).
    internal static class ModuleDetect
    {
        private const string ClanTierExpandedId = "ClanTierExpanded";

        internal static bool ClanTierExpandedActive()
        {
            try
            {
                return ModuleHelper.IsModuleActive(ClanTierExpandedId);
            }
            catch
            {
                return false;
            }
        }
    }
}
