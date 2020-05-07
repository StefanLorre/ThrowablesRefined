using System;
using HarmonyLib;
using TaleWorlds.Core;
using System.Xml;
using System.Reflection;

namespace ThrowablesRefined
{
    [HarmonyPatch]
    internal class CustomMissileSpeed
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(CraftingStatsType, "CalculateMissileSpeed");
        }

        public static float Postfix(float __result, Object __instance)
        {
            ref var wud = ref WeaponUsageDataAccessor(__instance);
            float weaponWeight = WeaponWeightAccessor(__instance);

            switch (wud.WeaponClass)
            {
                case WeaponClass.ThrowingAxe:
                    return ((__result / 3.2f) * axeFactor / (1 + weaponWeight / 1.3f) + axeFlat);
                case WeaponClass.ThrowingKnife:
                    return ((__result / 3.9f) * daggerFactor / (1 + weaponWeight / 2.2f) + daggerFlat);
                case WeaponClass.Javelin:
                    return ((__result / 3.6f) * javelinFactor / (1 + weaponWeight / 2.7f) + javelinFlat);
                default:
                    return __result;
            }
        }

        public static void initValues()
        {
            XmlNode xmlNode = ThrowablesRefinedMain.config.config.ChildNodes[1].SelectSingleNode("MissileSpeedSettings");
            javelinFlat = float.Parse(xmlNode.SelectSingleNode("JavelinFlatBonusSpeed").InnerText);
            javelinFactor = float.Parse(xmlNode.SelectSingleNode("JavelinMultiFactorSpeed").InnerText);
            axeFlat = float.Parse(xmlNode.SelectSingleNode("AxeFlatBonusSpeed").InnerText);
            axeFactor = float.Parse(xmlNode.SelectSingleNode("AxeMultiFactorSpeed").InnerText);
            daggerFlat = float.Parse(xmlNode.SelectSingleNode("DaggerFlatBonusSpeed").InnerText);
            daggerFactor = float.Parse(xmlNode.SelectSingleNode("DaggerMultiFactorSpeed").InnerText);
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
