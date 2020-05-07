using System;
using HarmonyLib;
using TaleWorlds.Core;
using System.Xml;
using System.Reflection;

namespace ThrowablesRefined
{
    [HarmonyPatch]
    internal class CustomMissileDamage
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(CraftingStatsType, "CalculateMissileDamage");
        }

        public static void Postfix(ref float damage, Object __instance)
        {
            ref var wud = ref WeaponUsageDataAccessor(__instance);
            float weaponWeight = WeaponWeightAccessor(__instance);

            switch (wud.WeaponClass)
            {
                case WeaponClass.ThrowingAxe:
                    damage = ((damage / 2f) * axeFactor * (1 + weaponWeight / 50) + axeFlat);
                    return;
                case WeaponClass.ThrowingKnife:
                    damage = ((damage / 3.3f) * daggerFactor * (1 + weaponWeight / 15) + daggerFlat);
                    return;
                case WeaponClass.Javelin:
                    damage = ((damage / 9f) * javelinFactor * (1 + weaponWeight / 12) + javelinFlat);
                    return;
                default:
                    damage = 0f;
                    return;
            }
        }

        public static void initValues()
        {
            XmlNode xmlNode = ThrowablesRefinedMain.config.config.ChildNodes[1].SelectSingleNode("MissileDamageSettings");
            javelinFlat = float.Parse(xmlNode.SelectSingleNode("JavelinFlatBonusDamage").InnerText);
            javelinFactor = float.Parse(xmlNode.SelectSingleNode("JavelinMultiFactorDamage").InnerText);
            axeFlat = float.Parse(xmlNode.SelectSingleNode("AxeFlatBonusDamage").InnerText);
            axeFactor = float.Parse(xmlNode.SelectSingleNode("AxeMultiFactorDamage").InnerText);
            daggerFlat = float.Parse(xmlNode.SelectSingleNode("DaggerFlatBonusDamage").InnerText);
            daggerFactor = float.Parse(xmlNode.SelectSingleNode("DaggerMultiFactorDamage").InnerText);
        }

        public static float javelinFlat = 0f;
        public static float javelinFactor = 1f;
        public static float axeFlat = 0f;
        public static float axeFactor = 1f;
        public static float daggerFlat = 0f;
        public static float daggerFactor = 1f;

        private static Type CraftingStatsType
            = Type.GetType("TaleWorlds.Core.Crafting, TaleWorlds.Core, Version=1.0.0.0, Culture=neutral")
            .GetNestedType("CraftedItemGenerationHelper", BindingFlags.NonPublic)
            .GetNestedType("CraftingStats", BindingFlags.NonPublic);

        private static AccessTools.FieldRef<Object, WeaponUsageData> WeaponUsageDataAccessor
            = AccessTools.FieldRefAccess<object, WeaponUsageData>(
                CraftingStatsType
                .GetField("_weaponUsageData", BindingFlags.NonPublic | BindingFlags.Instance)
            );

        private static AccessTools.FieldRef<Object, float> WeaponWeightAccessor
            = AccessTools.FieldRefAccess<object, float>(
                CraftingStatsType
                .GetField("_currentWeaponWeight", BindingFlags.NonPublic | BindingFlags.Instance)
            );

    }
}
