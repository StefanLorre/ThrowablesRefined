using System;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace ThrowablesRefined

{
    public class ThrowablesRefinedMain : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony.DEBUG = true;
            FileLog.Reset();
            Harmony harmony = new Harmony("mod.throwablesrefined.luna");
            try
            {
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                string str = "Error patching:\n";
                string message = ex.Message;
                string str2 = " \n\n";
                Exception innerException = ex.InnerException;
                FileLog.Log("ThrowablesRefined fucked up" + str + message + str2 + ((innerException != null) ? innerException.Message : null));
            }

            CustomStackAmount.initValues();
            CustomMissileDamage.initValues();
            CustomMissileSpeed.initValues();
        }

        public static ThrowablesRefinedConfig config = new ThrowablesRefinedConfig();
    }
}


