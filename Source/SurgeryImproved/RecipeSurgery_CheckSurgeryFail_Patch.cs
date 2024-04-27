using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
namespace SurgeryImproved
{
    [HarmonyPatch(typeof(Recipe_Surgery))]
    [HarmonyPatch("CheckSurgeryFail")]
    internal class RecipeSurgery_CheckSurgeryFail_Patch
    {
        public static void DropIngredients(Pawn surgeon, List<Thing> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if (ingredient.def.isTechHediff)
                {
                    var health = ingredient.HitPoints - (int)(ingredient.MaxHitPoints * Math.Min(0.25, Rand.Value * 0.5));
                    if (health > 0)
                    {
                        var item = GenSpawn.Spawn(ingredient.def, surgeon.Position, surgeon.Map);
                        item.HitPoints = health;
                    }
                }
            }
        }

        static bool Prefix(ref bool __result, Recipe_Surgery __instance, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
        {
            var recipe = __instance.recipe;

            SurgeryOutcome outcome = recipe.surgeryOutcomeEffect?.GetOutcome(recipe, surgeon, patient, ingredients, part, bill);
            if (outcome != null && outcome.failure)
            {
                if (recipe.addsHediffOnFailure != null)
                {
                    patient.health.AddHediff(recipe.addsHediffOnFailure, part);
                }

                // Recreate items
                if (outcome.applyEffectsToPart)
                {
                    DropIngredients(surgeon, ingredients);
                }

                __result = true;
                return false;
            }

            __result = false;
            return false;
        }
    }
}
