using System;
using HarmonyLib;
using TaleWorlds.Core;
using System.Xml;

namespace ThrowablesRefined
{
    [HarmonyPatch(typeof(WeaponComponentData))]
    [HarmonyPatch("set_MaxDataValue")]
    internal class CustomStackAmount
    {
        public static void Prefix(ref short value, WeaponComponentData __instance)
        {
            switch (__instance.WeaponClass)
            {
                case WeaponClass.Arrow:
                case WeaponClass.Bolt:
                case WeaponClass.Cartridge:
                case WeaponClass.Stone:
                case WeaponClass.Boulder:
                case WeaponClass.SmallShield:
                case WeaponClass.LargeShield:
                    return;
                case WeaponClass.ThrowingAxe:
                    value = (short)Math.Round((double)(((float)value + (float)CustomStackAmount.axeFlat) * CustomStackAmount.axeFactor), MidpointRounding.AwayFromZero);
                    return;
                case WeaponClass.ThrowingKnife:
                    value = (short)Math.Round((double)(((float)value + (float)CustomStackAmount.daggerFlat) * CustomStackAmount.daggerFactor), MidpointRounding.AwayFromZero);
                    return;
                case WeaponClass.Javelin:
                    value = (short)Math.Round((double)(((float)value + (float)CustomStackAmount.javelinFlat) * CustomStackAmount.javelinFactor), MidpointRounding.AwayFromZero);
                    return;
            }
            return;
        }

        public static void initValues()
        {
            XmlNode xmlNode = ThrowablesRefinedMain.config.config.ChildNodes[1].SelectSingleNode("StackSizeSettings");
            javelinFlat = float.Parse(xmlNode.SelectSingleNode("JavelinFlatBonus").InnerText);
            javelinFactor = float.Parse(xmlNode.SelectSingleNode("JavelinMultiFactor").InnerText);
            axeFlat = float.Parse(xmlNode.SelectSingleNode("AxeFlatBonus").InnerText);
            axeFactor = float.Parse(xmlNode.SelectSingleNode("AxeMultiFactor").InnerText);
            daggerFlat = float.Parse(xmlNode.SelectSingleNode("DaggerFlatBonus").InnerText);
            daggerFactor = float.Parse(xmlNode.SelectSingleNode("DaggerMultiFactor").InnerText);
        }

        public static float javelinFlat = 0f;
        public static float javelinFactor = 1f;
        public static float axeFlat = 0f;
        public static float axeFactor = 1f;
        public static float daggerFlat = 0f;
        public static float daggerFactor = 1f;

    }
}