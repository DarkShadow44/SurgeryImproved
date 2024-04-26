using HarmonyLib;
using Verse;

namespace KeepBlueprintSettings
{
    [StaticConstructorOnStartup]
    public class SurgeryImproved
    {
        static SurgeryImproved()
        {
            var harmony = new Harmony("DarkShadow44.SurgeryImproved.Harmony");
            harmony.PatchAll();
        }
    }
}
