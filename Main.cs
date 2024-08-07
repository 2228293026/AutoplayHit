using System;
using System.ComponentModel;
using System.Reflection;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager.Param;
using static UnityModManagerNet.UnityModManager;
using static Hitmargin_AutoplayHit.Main;
using System.Collections.Generic;
using UnityEngine.Windows;
using ADOFAI;
using System.Linq;

namespace Hitmargin_AutoplayHit
{

    public class Main
    {

        public static Harmony Ham;
        public static float value = 0;
        public static bool enable;
        public static bool open;
        public static bool whiteAuto, 
            OldAuto, 
            auto, 
            autoHit, 
            nextTileIsAuto, 
            autoBlink, 
            autoanimation, 
            HideAutotext, 
            ya, 
            na, 
            yb, 
            nb;
        public static Setting setting;
        public static UnityModManager.ModEntry mod;
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            setting = UnityModManager.ModSettings.Load<Setting>(mod);

            value = setting.value;
            enable = setting.enable;
            open = setting.open;
            whiteAuto = setting.whiteAuto;
            OldAuto = setting.OldAuto;
            auto = setting.auto;
            autoHit = setting.autoHit;
            nextTileIsAuto = setting.nextTileIsAuto;
            autoBlink = setting.autoBlink;
            autoanimation = setting.autoanimation;
            HideAutotext = setting.HideAutotext;
            ya = setting.ya;
            na = setting.na;
            yb = setting.yb;
            nb = setting.nb;
            
            Main.Ham = new Harmony(mod.Info.Id);
            Main.Ham.PatchAll(Assembly.GetExecutingAssembly());
            mod.OnGUI = OnGUI;
            mod.OnSaveGUI = OnSaveGUI;
            mod.OnHideGUI = OnHideGUI;

            return true;
        }
        public class Setting : UnityModManager.ModSettings
        {
            public float value;
            public bool enable;
            public bool open;
            public bool whiteAuto,
            OldAuto,
            auto,
            autoHit,
            nextTileIsAuto,
            autoBlink,
            autoanimation,
            HideAutotext,
            ya,
            na, 
            yb, 
            nb;
            public string hexColor = "#FFFFFF";
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("作者: HitMargin");

            GUILayout.BeginHorizontal(); 
            GUILayout.Label("Sliders Enable:", GUILayout.Width(120)); 
            setting.enable = GUILayout.Toggle(setting.enable, "On/Off"); 
            GUILayout.EndHorizontal(); 

            if (setting.enable) 
            {

                GUILayout.BeginHorizontal(); 
                GUILayout.Label("value:", GUILayout.Width(60)); 
                setting.value = GUILayout.HorizontalSlider(setting.value, 0, 10); 
                GUILayout.Label(setting.value.ToString("F1"), GUILayout.Width(40)); 
                GUILayout.EndHorizontal(); 
            }

            setting.whiteAuto = GUILayout.Toggle(setting.whiteAuto, "whiteAuto");
            setting.OldAuto = GUILayout.Toggle(setting.OldAuto, "OldAuto");
            setting.auto = GUILayout.Toggle(setting.auto, "auto");
            setting.autoHit = GUILayout.Toggle(setting.autoHit, "autoHit");
            setting.nextTileIsAuto = GUILayout.Toggle(setting.nextTileIsAuto, "nextTileIsAuto(will die)");
            setting.autoBlink = GUILayout.Toggle(setting.autoBlink, "autoBlink");
            if (setting.autoBlink)
            {
                setting.ya = GUILayout.Toggle(setting.ya, "y");
                if (setting.ya)
                {
                    setting.na = false;
                }
                setting.na = GUILayout.Toggle(setting.na, "n");
                if (setting.na)
                {
                    setting.ya = false;
                }
            }

            setting.autoanimation = GUILayout.Toggle(setting.autoanimation, "autoAnimation");
            if (setting.autoanimation)
            {
                setting.yb = GUILayout.Toggle(setting.yb, "y");
                if (setting.yb)
                {
                    setting.nb = false;
                }
                setting.nb = GUILayout.Toggle(setting.nb, "n");
                if (setting.nb)
                {
                    setting.yb = false;
                }
            }
            setting.HideAutotext = GUILayout.Toggle(setting.HideAutotext, "HideAutotext/Oldtext/Debugtext");
            GUILayout.BeginHorizontal(); 
            GUILayout.Label("EnableAutoColor:", GUILayout.Width(120)); 
            setting.open = GUILayout.Toggle(setting.open, "On/Off"); 
            GUILayout.EndHorizontal(); 
            if (setting.open)
            {
                GUILayout.Label("十六进制颜色代码:", GUILayout.Width(120));
                setting.hexColor = GUILayout.TextField(setting.hexColor, GUILayout.Width(100));

                if (ColorUtility.TryParseHtmlString(setting.hexColor, out Color color))
                {
                    GUI.backgroundColor = color;
                    GUILayout.Box("", GUILayout.Width(100), GUILayout.Height(20));
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    GUILayout.Label("无效的十六进制颜色代码!", GUILayout.Width(200));
                }
            }
            GUILayout.EndVertical();
        }
        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            value = setting.value;
            enable = setting.enable;
            open = setting.open;
            whiteAuto = setting.whiteAuto;
            OldAuto = setting.OldAuto;
            auto = setting.auto;
            autoHit = setting.autoHit;
            nextTileIsAuto = setting.nextTileIsAuto;
            autoBlink = setting.autoBlink;
            autoanimation = setting.autoanimation;
            HideAutotext = setting.HideAutotext;
            ya = setting.ya;
            na = setting.na;
            yb = setting.yb;
            nb = setting.nb;
            UnityModManager.ModSettings.Save(setting, modEntry);
        }


        public static void OnHideGUI(UnityModManager.ModEntry modEntry)
        {
            OnSaveGUI(modEntry); 
        }

        public class RandomValueDisplayUGUI : MonoBehaviour
        {
            public Text randomValueText; 
            private System.Random random;
            private float randomValue;

            void Start()
            {
                random = new System.Random();
                UpdateRandomValue();
            }

            void UpdateRandomValue()
            {
                randomValue = (float)(random.NextDouble() + 1.0 - value);
                randomValueText.text = "Random Value: " + randomValue.ToString("F2");
            }
        }

        public static void ApplyCustomColor(Color colors, scnEditor editorInstance)
        {
            
            editorInstance.autoImage.color = colors;
        }

        [HarmonyPatch(typeof(scrController), "Hit")]
        public static class H_scrController_Hit
        {
            [HarmonyPrefix]
            public static bool Prefix(scrController __instance)
            {
                scrMisc.Vibrate(50L);
                if (!__instance.responsive)
                {
                    return false;
                }
                if (ADOBase.isLevelEditor && ADOBase.controller.paused)
                {
                    return false;
                }
                bool flag = __instance.chosenplanet.currfloor.nextfloor != null && __instance.chosenplanet.currfloor.nextfloor.auto;
                __instance.chosenplanet.cachedAngle = __instance.chosenplanet.angle;
                if (__instance.errorMeter && __instance.gameworld && Persistence.hitErrorMeterSize != ErrorMeterSize.Off)
                {
                    float initialNum = (float)(__instance.chosenplanet.cachedAngle - __instance.chosenplanet.targetExitAngle);
                    if (!__instance.isCW)
                    {
                        initialNum *= -1f;
                    }

                    if (!__instance.midspinInfiniteMargin)
                    {
                        if ((flag) && !RDC.useOldAuto)
                        {
                            __instance.errorMeter.AddHit(0f, 1f);
                        }
                        else
                        {

                            System.Random random = new System.Random();

                            float randomValue = (float)(random.NextDouble() + (1.0 - value));


                            __instance.errorMeter.AddHit(randomValue, (float)(__instance.currFloor.nextfloor.marginScale));

                            if (!Main.enable)
                            {
                                __instance.errorMeter.AddHit(initialNum, (float)(__instance.currFloor.nextfloor.marginScale));

                            }
                        }

                    }
                }
                __instance.chosenplanet.next.ChangeFace(true);
                UnityEngine.Object x = __instance.chosenplanet;
                __instance.chosenplanet = __instance.chosenplanet.SwitchChosen();
                bool result = x != __instance.chosenplanet;
                if (ADOBase.playerIsOnIntroScene)
                {
                    return result;
                }
                bool flag2 = __instance.chosenplanet.currfloor.holdLength == -1 || (__instance.chosenplanet.currfloor.holdLength > -1 && __instance.lastCamPulseFloor < __instance.chosenplanet.currfloor.seqID);
                __instance.lastCamPulseFloor = __instance.chosenplanet.currfloor.seqID;
                if (__instance.camy.followMode && flag2)
                {
                    __instance.camy.frompos = __instance.camy.transform.localPosition - __instance.camy.shake.WithZ(0f);
                    __instance.camy.topos = new Vector3(__instance.chosenplanet.transform.position.x, __instance.chosenplanet.transform.position.y, __instance.camy.transform.position.z);
                    __instance.camy.timer = 0f;
                }
                if (__instance.camy.isPulsingOnHit && flag2)
                {
                    __instance.camy.Pulse();
                }
                if (ADOBase.isLevelEditor)
                {
                    bool flag3 = true;
                    if (__instance.currFloor.midSpin || (__instance.currFloor.seqID > 0 && ADOBase.lm.listFloors[__instance.currFloor.seqID - 1].holdLength > -1))
                    {
                        flag3 = false;
                    }
                    if (__instance.currFloor.seqID > 1 && ADOBase.lm.listFloors[__instance.currFloor.seqID - 1].midSpin && ADOBase.lm.listFloors[__instance.currFloor.seqID - 2].holdLength > -1)
                    {
                        flag3 = false;
                    }
                    if (flag3)
                    {
                        scnEditor.instance.OttoBlink();
                    }
                }
                if (__instance.currFloor.midSpin)
                {
                    int count = __instance.planetList.Count;
                    __instance.midspinInfiniteMargin = true;
                    __instance.keyTimes.Add(Time.timeAsDouble);
                }
                else
                {
                    __instance.midspinInfiniteMargin = false;
                }
                __instance.chosenplanet.Update_RefreshAngles();
                //     return result;

                return false;
            }
        }

        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class H_scnEditor_highBPM
        {
            [HarmonyPrefix]
            public static bool Prefix(scnEditor __instance, ref bool __result)
            {

                if (Main.whiteAuto)
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(RDC), "get_useOldAuto")]
        public static class H_RDC_get_useOldAuto
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (Main.OldAuto)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(RDC), "get_auto")]
        public static class H_RDC_get_auto
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (Main.auto)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }
        [HarmonyPatch(typeof(scrMisc), "IsValidHit")]
        public static class H_scrMisc_IsValidHit
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (Main.autoHit)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }
        [HarmonyPatch(typeof(scrController), "get__nextTileIsAuto")]
        public static class H_scrController___nextTileIsAuto
        {
            [HarmonyPrefix]
            public static bool Prefix(ref bool __result)
            {
                if (ADOBase.isLevelEditor)
                {
                    if (Main.nextTileIsAuto)
                    {
                        __result = true;
                        return false;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoBlink")]
        public static class H_scnEditor_OttoBlink_y
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.ya)
                {
                    tempauto = RDC.auto;
                    RDC.auto = true;

                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.ya)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoBlink")]
        public static class H_scnEditor_OttoBlink_n
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.na)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;

                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.na)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class H_scnEditor_OttoUpdate_y
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.yb)
                {
                    tempauto = RDC.auto;
                    RDC.auto = true;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.yb)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class H_scnEditor_OttoUpdate_n
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.nb)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.nb)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scrShowIfDebug), "Update")]
        public static class H_scrShowIfDebug_Update_auto
        {
           
            [HarmonyPrefix]
            public static bool Prefix(scrShowIfDebug __instance)
            {
                if (Main.HideAutotext)
                {
                    return false;
                
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class YourTargetClass_Patch
        {
            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.open)
                {
                    
                    if (ColorUtility.TryParseHtmlString(setting.hexColor, out Color colors))
                    {
                        ApplyCustomColor(colors, __instance);
                    }
                    else
                    {
                        Debug.LogError("无效的十六进制颜色代码。");
                    }
                }
                return;
            }
        }
    }
}
