using Verse;

namespace SurgeryImproved
{
    internal class Settings : ModSettings
    {
        public bool allowSurgery100Percent = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref allowSurgery100Percent, "allowSurgery100Percent");
            base.ExposeData();
        }
    }
}
