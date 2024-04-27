using HarmonyLib;
using KeepBlueprintSettings;
using RimWorld;
using Verse;

namespace SurgeryImproved
{
    [HarmonyPatch(typeof(SurgeryOutcomeComp_ClampToRange))]
    [HarmonyPatch(nameof(SurgeryOutcomeComp_ClampToRange.AffectQuality))]
    internal class SurgeryOutcomeComp_ClampToRange_AffectQuality_Patch
    {
        static void Prefix(SurgeryOutcomeComp_ClampToRange __instance)
        {
            float max = SurgeryImprovedMod.settings.allowSurgery100Percent ? 1 : 0.98f;
            __instance.range = new FloatRange(0, max);
        }
    }
}
