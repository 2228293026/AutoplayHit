using System;
using Hitmargin_AutoplayHit;
using HarmonyLib;

namespace AutoplayHit
{
    [HarmonyPatch(typeof(scrController), "FailAction")]
    internal static class ChangeTextOnFail
    {
        private static void Prefix(scrController __instance)
        {
            Text.Content = Main.Settings.NotPlayingText;
        }
    }
}
