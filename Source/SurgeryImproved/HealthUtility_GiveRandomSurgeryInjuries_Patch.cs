using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace KeepBlueprintSettings
{
    [HarmonyPatch(typeof(HealthUtility))]
    [HarmonyPatch(nameof(HealthUtility.GiveRandomSurgeryInjuries))]
    static class HealthUtility_GiveRandomSurgeryInjuries_Patch
    {
        static bool Prefix(Pawn p, int totalDamage, BodyPartRecord operatedPart)
        {
            if (operatedPart == null)
            {
                operatedPart = p.RaceProps.body.corePart;
            }
            while (p.health.hediffSet.GetPartHealth(operatedPart) <= 0)
            {
                operatedPart = operatedPart.parent ?? p.RaceProps.body.corePart;
            }

            if (p.health.WouldLosePartAfterAddingHediff(HediffDefOf.Cut, operatedPart, totalDamage))
            {
                float partHealth = p.health.hediffSet.GetPartHealth(operatedPart);
                if (partHealth / operatedPart.def.GetMaxHealth(p) >= 0.75)
                {
                    Random random = new Random();
                    totalDamage = random.Next((int)(partHealth * 0.7), (int)partHealth);
                }
            }
            DamageDef def = Rand.Element(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
            DamageInfo dinfo = new DamageInfo(def, totalDamage, 0f, -1f, null, operatedPart);
            dinfo.SetIgnoreArmor(true);
            dinfo.SetIgnoreInstantKillProtection(true);
            p.TakeDamage(dinfo);
            return false;
        }
    }
}
