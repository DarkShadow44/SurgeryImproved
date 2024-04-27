using HarmonyLib;
using SurgeryImproved;
using UnityEngine;
using Verse;

namespace KeepBlueprintSettings
{
    [StaticConstructorOnStartup]
    internal class SurgeryImprovedMod : Mod
    {
        public static Settings settings;

        static SurgeryImprovedMod()
        {
            var harmony = new Harmony("DarkShadow44.SurgeryImproved.Harmony");
            harmony.PatchAll();
        }

        public SurgeryImprovedMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Allow surgery to reach 100% success rate", ref settings.allowSurgery100Percent, "");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Surgery Improved";
        }
    }
}
