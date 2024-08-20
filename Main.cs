using System;
using System.ComponentModel;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Diagnostics;
using UnityEngine.UI;
using SA.GoogleDoc;
using System.Threading;
using static UnityModManagerNet.UnityModManager.Param;
using static UnityModManagerNet.UnityModManager;
using static Hitmargin_AutoplayHit.Main;
using System.Collections.Generic;
using UnityEngine.Windows;
using ADOFAI;
using System.Linq;
using AutoplayHit;

namespace Hitmargin_AutoplayHit
{

    public class Main
    {
        public static MainSettings Settings { get; private set; }
        public static AutoplayHit.Text text;
        public static double RandomValue { get; private set; }
        internal static bool IsEnabled { get; private set; }
        public static Harmony Ham;
        public static string AutoText = "status.autoplay";
        public static string OldText = " (old)";
        public static string AutoTileText = "status.autoTile";
        public static float value = 0;
        public static bool enable;
        public static bool open;
        public static bool opena;
        public static bool whiteAuto,
            OldAuto,
            auto,
            autoHit,
            nextTileIsAuto,
            autoBlink,
            autoanimation,
            ya,
            na,
            yb,
            nb,
            PurePerfectSfxSound,
            AlwaysCountdown,
            TrueHit,
            TrueHitNodie
            ;
        public static UnityModManager.ModEntry mod;
        private static object highBPM;

        public static void Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            mod.OnToggle = delegate (UnityModManager.ModEntry ent, bool v)
            {
                Main.IsEnabled = v;
                if (v)
                {
                    Settings = UnityModManager.ModSettings.Load<MainSettings>(mod);
                    value = Settings.value;
                    enable = Settings.enable;
                    open = Settings.open;
                    opena = Settings.opena;
                    whiteAuto = Settings.whiteAuto;
                    OldAuto = Settings.OldAuto;
                    auto = Settings.auto;
                    autoHit = Settings.autoHit;
                    nextTileIsAuto = Settings.nextTileIsAuto;
                    autoBlink = Settings.autoBlink;
                    autoanimation = Settings.autoanimation;
                    ya = Settings.ya;
                    na = Settings.na;
                    yb = Settings.yb;
                    nb = Settings.nb;
                    PurePerfectSfxSound = Settings.PurePerfectSfxSound;
                    AlwaysCountdown = Settings.AlwaysCountdown;
                    AutoText = Settings.Autotext;
                    OldText = Settings.OldText;
                    AutoTileText = Settings.AutoTileText;
                    TrueHit = Settings.TrueHit;
                    TrueHitNodie = Settings.TrueHitNodie;
                    Main.Ham = new Harmony(mod.Info.Id);
                    Main.Ham.PatchAll(Assembly.GetExecutingAssembly());
                    mod.OnGUI = MainSettings.OnGUI;
                    mod.OnSaveGUI = MainSettings.OnSaveGUI;
                    mod.OnHideGUI = MainSettings.OnHideGUI;
                    Main.text = new GameObject().AddComponent<AutoplayHit.Text>();
                    UnityEngine.Object.DontDestroyOnLoad(Main.text);
                }
                else
                {
                    Main.Ham.UnpatchAll(mod.Info.Id);
                    Main.Ham = null;
                    UnityEngine.Object.DestroyImmediate(Main.text);
                    Main.text = null;
                }
                return true;
            };
        }

        double MeasureSpeed()
        {
            return Stopwatch.GetTimestamp() % 10000000; // 模拟一个变化的速度值
        }

        public static void Update()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.LeftArrow:
                        // 如果按下左箭头键，减少数值
                        Settings.value -= 0.1f;
                        break;
                    case KeyCode.RightArrow:
                        // 如果按下右箭头键，增加数值
                        Settings.value += 0.1f;
                        break;
                }
            }
        }

        public static void ApplyCustomColor(Color colors, scnEditor __instance)
        {
            FieldInfo isOttoBlinkingField = typeof(scnEditor).GetField("isOttoBlinking", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo grayColorField = typeof(scnEditor).GetField("grayColor", BindingFlags.NonPublic | BindingFlags.Instance);
            bool isOttoBlinking = (bool)isOttoBlinkingField.GetValue(__instance);
            Color grayColor = (Color)grayColorField.GetValue(__instance);
            FieldInfo autoPetTimeField = typeof(scnEditor).GetField("autoPetTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float autoPetTime = (float)autoPetTimeField.GetValue(__instance);
            System.Reflection.PropertyInfo highBPMProperty = typeof(scnEditor).GetProperty("highBPM", BindingFlags.NonPublic | BindingFlags.Instance);
            bool highBPM = (bool)highBPMProperty.GetValue(__instance);
            System.Reflection.PropertyInfo pausedProperty = typeof(scnEditor).GetProperty("paused", BindingFlags.NonPublic | BindingFlags.Instance);
            bool paused = (bool)highBPMProperty.GetValue(__instance);
            MethodInfo OttoPetUpdate = typeof(scnEditor).GetMethod("OttoPetUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
            if (RDEditorUtils.CheckPointerInObject(__instance.buttonAuto))
            {
                OttoPetUpdate.Invoke(__instance, null);
            }
            else
            {
                autoPetTime = 0f;
            }
            if (!isOttoBlinking)
            {
                if (RDC.auto)
                {
                    if (!__instance.autoFailed)
                    {
                        if (autoPetTime < 1.5f)
                        {
                            __instance.autoImage.sprite = ((highBPM && paused) ? __instance.autoSprites[7] : __instance.autoSprites[1]);
                        }
                        else
                        {
                            __instance.autoImage.sprite = __instance.autoSprites[9];
                        }
                    }
                    else
                    {
                        __instance.autoImage.sprite = __instance.autoSprites[6];
                    }
                    __instance.autoImage.color = colors;
                    return;
                }
                __instance.autoImage.sprite = ((highBPM && paused) ? __instance.autoSprites[8] : __instance.autoSprites[0]);
                __instance.autoImage.color = grayColor * colors;
            }
        }
        public static float GenerateRandomValue()
        {
            System.Random random = new System.Random();
            return (float)(random.NextDouble() * 0.5 - value);
        }

        public static float TrueMs(scrController __instance)
        {
            return (float)(__instance.chosenplanet.cachedAngle - __instance.chosenplanet.targetExitAngle);
        }

        public static double ModMs(scrController __instance)
        {
            return RandomValue;
        }

        [HarmonyPatch(typeof(scrController), "Hit")]
        public static class H_scrController_Hit
        {

            [HarmonyPrefix]
            public static bool Prefix(scrController __instance)
            {
                if (Main.enable)
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
                        float num = (float)(__instance.chosenplanet.cachedAngle - __instance.chosenplanet.targetExitAngle);
                        if (!__instance.isCW)
                        {
                            num *= -1f;
                        }

                        if (!__instance.midspinInfiniteMargin)
                        {

                            if (!Main.enable)
                            {
                                if ((RDC.auto || flag) && !RDC.useOldAuto)
                                {
                                    __instance.errorMeter.AddHit(0f, 1f);
                                }
                                else
                                {
                                    __instance.errorMeter.AddHit(num, (float)__instance.currFloor.nextfloor.marginScale);
                                }
                            }
                            else
                            {
                                RandomValue = GenerateRandomValue(); // 调用方法并更新随机值属性
                                __instance.errorMeter.AddHit((float)RandomValue, (float)(__instance.currFloor.nextfloor.marginScale));
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
                return true;
            }
        }

        [HarmonyPatch(typeof(scnEditor), "get_highBPM")]
        public static class H_scnEditor_highBPM
        {
            [HarmonyPostfix]
            public static void Postfix(scnEditor __instance, ref bool __result)
            {

                if (Main.whiteAuto)
                {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(RDC), "get_useOldAuto")]
        public static class H_RDC_get_useOldAuto
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (Main.OldAuto)
                {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(RDC), "get_auto")]
        public static class H_RDC_get_auto
        {

            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (Main.auto)
                {
                    __result = true;
                }
            }
        }
        [HarmonyPatch(typeof(scrMisc), "IsValidHit")]
        public static class H_scrMisc_IsValidHit
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (Main.autoHit)
                {
                    __result = true;
                }
            }
        }
        [HarmonyPatch(typeof(scrController), "get__nextTileIsAuto")]
        public static class H_scrController___nextTileIsAuto
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                if (ADOBase.isLevelEditor && Main.nextTileIsAuto)
                {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoBlink")]
        public static class H_scnEditor_OttoBlink_y
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (ADOBase.isLevelEditor && Main.ya && Main.autoBlink)
                {
                    tempauto = RDC.auto;
                    RDC.auto = true;

                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.ya && Main.autoBlink)
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
                if (ADOBase.isLevelEditor && Main.na && Main.autoBlink)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;

                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.na && Main.autoBlink)
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
                if (ADOBase.isLevelEditor && Main.yb && Main.autoanimation)
                {
                    tempauto = RDC.auto;
                    RDC.auto = true;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.yb && Main.autoanimation)
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
                if (ADOBase.isLevelEditor && Main.nb && Main.autoanimation)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.nb && Main.autoanimation)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scnEditor), "OttoUpdate")]
        public static class YourTargetClass_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(scnEditor __instance)
            {
                if (Main.open)
                {

                    // 应用颜色
                    if (ColorUtility.TryParseHtmlString(Settings.hexColor, out Color colors))
                    {
                        ApplyCustomColor(colors, __instance);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("无效的十六进制颜色代码。");
                    }
                    return false;
                }
                return true;

            }
        }

        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        public class H_scrController_OnLandOnPortal
        {
            private static bool tempauto;
            [HarmonyPrefix]
            public static void Prefix()
            {
                if (ADOBase.isLevelEditor && Main.PurePerfectSfxSound)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.PurePerfectSfxSound)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scnGame), "Play")]
        public class H_scnGame_Play
        {
            private static bool tempauto;
            [HarmonyPrefix]
            public static void Prefix()
            {
                if (ADOBase.isLevelEditor && Main.AlwaysCountdown)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;
                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ADOBase.isLevelEditor && Main.AlwaysCountdown)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scrShowIfDebug), "Update")]
        public static class H_scrShowIfDebug_Update
        {
            [HarmonyPrefix]
            public static bool Prefix(scrShowIfDebug __instance)
            {
                if (Main.opena)
                {
                    FieldInfo txtField = typeof(scrShowIfDebug).GetField("txt", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo defaultColorField = typeof(scrShowIfDebug).GetField("defaultColor", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo orangeColorField = typeof(scrShowIfDebug).GetField("orangeColor", BindingFlags.NonPublic | BindingFlags.Instance);
                    Color defaultColorValue = (Color)defaultColorField.GetValue(__instance);
                    Color orangeColorValue = (Color)orangeColorField.GetValue(__instance);
                    if (txtField != null)
                    {
                        UnityEngine.UI.Text txtComponent = (UnityEngine.UI.Text)txtField.GetValue(__instance);

                        if (txtComponent != null)
                        {
                            if (RDC.noHud || GCS.d_recording)
                            {
                                if (txtComponent != null)
                                {
                                    txtComponent.enabled = false;
                                    return false;
                                }
                                return false;
                            }

                            txtComponent.color = defaultColorValue;
                            if (RDC.auto && RDC.debug)
                            {
                                txtComponent.enabled = true;
                                txtComponent.text = string.Empty;
                                return false;
                            }
                            if (RDC.auto)
                            {
                                txtComponent.enabled = true;
                                if (RDC.useOldAuto)
                                {
                                    txtComponent.text = AutoText + OldText;
                                    return false;
                                }
                                txtComponent.text = AutoText;
                                return false;
                            }
                            else
                            {
                                if (!RDC.auto && scrController.instance.currFloor && scrController.instance.currFloor.nextfloor && scrController.instance.currFloor.nextfloor.showStatusText && !ADOBase.sceneName.IsTaro())
                                {
                                    txtComponent.enabled = true;
                                    txtComponent.text = AutoTileText;
                                    txtComponent.color = orangeColorValue;
                                    return false;
                                }
                                if (RDC.debug)
                                {
                                    txtComponent.enabled = true;
                                    txtComponent.text = "Debug Mode";
                                    return false;
                                }
                                if (txtComponent.enabled)
                                {
                                    txtComponent.enabled = false;
                                    txtComponent.text = string.Empty;
                                }
                                return false;
                            }
                        }
                    }
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(scrController), "Hit")]
        public static class H_scrController_Hit_TrueHit_y
        {
            private static bool tempauto;

            [HarmonyPrefix]
            public static void Prefix(scnEditor __instance)
            {
                if (Main.TrueHit)
                {
                    tempauto = RDC.auto;
                    RDC.auto = false;

                }
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                if (Main.TrueHit)
                {
                    RDC.auto = tempauto;
                }
            }
        }

        [HarmonyPatch(typeof(scrController), "Hit")]
        public static class H_scrController_Hit_TrueHit_nodie
        {
            [HarmonyPrefix]
            public static bool Prefix(scrController __instance)
            {
                if (Main.TrueHitNodie)
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
                        float num = (float)(__instance.chosenplanet.cachedAngle - __instance.chosenplanet.targetExitAngle);
                        if (!__instance.isCW)
                        {
                            num *= -1f;
                        }

                        if (!__instance.midspinInfiniteMargin)
                        {

                            if (!Main.TrueHitNodie)
                            {
                                if ((RDC.auto || flag) && !RDC.useOldAuto)
                                {
                                    __instance.errorMeter.AddHit(0f, 1f);
                                }
                                else
                                {
                                    __instance.errorMeter.AddHit(num, (float)__instance.currFloor.nextfloor.marginScale);
                                }
                            }
                            else
                            {
                                
                                __instance.errorMeter.AddHit(num, (float)(__instance.currFloor.nextfloor.marginScale));
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
                return true;
            }
        }

    }
}
