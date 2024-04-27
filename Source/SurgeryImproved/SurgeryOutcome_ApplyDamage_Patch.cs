using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace KeepBlueprintSettings
{
    [HarmonyPatch(typeof(SurgeryOutcome))]
    [HarmonyPatch("ApplyDamage")]
    static class SurgeryOutcome_ApplyDamage_Patch
    {
        static void ApplyDamage(Pawn patient, BodyPartRecord part, int totalDamage, bool preventDestruction)
        {
            if (part == null)
            {
                part = patient.RaceProps.body.corePart;
            }
            while (patient.health.hediffSet.GetPartHealth(part) <= 0)
            {
                part = part.parent ?? patient.RaceProps.body.corePart;
            }

            if (part == patient.RaceProps.body.corePart)
            {
                preventDestruction = true;
            }

            if (patient.health.WouldLosePartAfterAddingHediff(HediffDefOf.Cut, part, totalDamage))
            {
                float partHealth = patient.health.hediffSet.GetPartHealth(part);
                if (partHealth / part.def.GetMaxHealth(patient) >= 0.7 && preventDestruction)
                {
                    Random random = new Random();
                    var min = (int)(partHealth * 0.7);
                    if (min == (int)partHealth)
                    {
                        return;
                    }
                    totalDamage = random.Next(min, (int)partHealth);
                }
            }
            DamageDef def = Rand.Element(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
            DamageInfo dinfo = new DamageInfo(def, totalDamage, 0f, -1f, null, part);
            dinfo.SetIgnoreArmor(true);
            dinfo.SetIgnoreInstantKillProtection(true);
            dinfo.SetAllowDamagePropagation(false);
            patient.TakeDamage(dinfo);
        }

        static bool Prefix(SurgeryOutcome __instance, Pawn patient, BodyPartRecord part)
        {
            ApplyDamage(patient, part, __instance.totalDamage, __instance.applyEffectsToPart);
            if (__instance.totalDamage > 10)
            {
                part = part.parent;
                while (part != null && Rand.Value < 0.25)
                {
                    ApplyDamage(patient, part, __instance.totalDamage, true);
                    part = part.parent;
                }
            }
            return false;
        }
    }
}
