using System;
using Hitmargin_AutoplayHit;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.AudioSettings;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace AutoplayHit
{
    [HarmonyPatch(typeof(scrController), "PlayerControl_Update")]
    internal static class ChangeText
    {
        private static void Prefix(scrController __instance)
        {
            bool flag = !scrController.instance.paused && scrConductor.instance.isGameWorld;
            if (flag)
            {
                double Ms = Main.ModMs(__instance);
                double TrueMs = Main.TrueMs(__instance);
                   Text.Content = Main.Settings.TextTemplate.Replace("<ms>", Ms.ToString()).Replace("<truems>", TrueMs.ToString()); ;
                }
                else
                {
                    Text.Content = Main.Settings.NotPlayingText;
                }
            }
    }
}