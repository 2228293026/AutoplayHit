using SA.GoogleDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hitmargin_AutoplayHit;
using UnityEngine;
using UnityModManagerNet;

namespace AutoplayHit
{
    public class MainSettings : UnityModManager.ModSettings, IDrawable
    {
        public float value;
        public bool enable;
        public bool open;
        public bool opena;
        public string AutoText;
        public string OldText;
        public string AutoTileText;
        public bool whiteAuto,
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
        public string hexColor = "#FFFFFF";
        [Draw("X")]
        public int PositionX = 0;

        [Draw("Y")]
        public int PositionY = 0;

        [Draw("Size")]
        public int TextSize = 50;

        [Draw("Show(1~100)", Min = 0.0, Max = 100.0)]
        public int TextShadow = 50;

        [Draw("")]
        public string TextTemplate = "Mod MS : <ms>\nTrue MS : <truems>";

        [Draw("")]
        public string NotPlayingText = "Not Playing";

        public string Autotext;
        public void OnChange()
        {
        }


        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Text", Array.Empty<GUILayoutOption>());
            Main.Settings.TextTemplate = GUILayout.TextArea(Main.Settings.TextTemplate, Array.Empty<GUILayoutOption>());
            GUILayout.Label("Not Playing Text", Array.Empty<GUILayoutOption>());
            Main.Settings.NotPlayingText = GUILayout.TextArea(Main.Settings.NotPlayingText, Array.Empty<GUILayoutOption>());
            //Extensions.Draw<MainMain.Settingss>(Main.Main.Settingss, modEntry);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X Position:", GUILayout.Width(100));

            Main.Settings.PositionX = (int)GUILayout.HorizontalSlider(Main.Settings.PositionX, 0, 2000);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Y Position:", GUILayout.Width(100));
            Main.Settings.PositionY = (int)GUILayout.HorizontalSlider(Main.Settings.PositionY, 0, 1000);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Text Size:", GUILayout.Width(100));
            Main.Settings.TextSize = (int)GUILayout.HorizontalSlider(Main.Settings.TextSize, 10, 100);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Text Shadow:", GUILayout.Width(100));
            Main.Settings.TextShadow = (int)GUILayout.HorizontalSlider(Main.Settings.TextShadow, 0, 100);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(); // 开始一行
            GUILayout.Label("Num Enable:", GUILayout.Width(120));
            Main.Settings.enable = GUILayout.Toggle(Main.Settings.enable, "On/Off");
            GUILayout.EndHorizontal(); // 结束一行

            if (Main.Settings.enable)
            {
                Main.Settings.TrueHit = false;
                Main.Settings.TrueHitNodie = false;
                GUILayout.BeginHorizontal(); 
                GUILayout.Label("Value:", GUILayout.Width(60));
                string inputFieldText = GUILayout.TextField(Main.Settings.value.ToString("F2"), GUILayout.Width(100));
                GUILayout.Label(Main.Settings.value.ToString("F2"), GUILayout.Width(40));
                GUILayout.EndHorizontal();

                // 检查输入是否为有效的浮点数并更新设置
                if (float.TryParse(inputFieldText, out float numericValue))
                {
                    Main.Settings.value = numericValue;
                }
                else
                {
                    Main.Settings.value = 1.0f;
                }


            }
            Main.Settings.whiteAuto = GUILayout.Toggle(Main.Settings.whiteAuto, "whiteAuto");
            Main.Settings.OldAuto = GUILayout.Toggle(Main.Settings.OldAuto, "OldAuto");
            Main.Settings.auto = GUILayout.Toggle(Main.Settings.auto, "auto");
            Main.Settings.autoHit = GUILayout.Toggle(Main.Settings.autoHit, "autoHit");
            Main.Settings.nextTileIsAuto = GUILayout.Toggle(Main.Settings.nextTileIsAuto, "nextTileIsAuto(will die)");
            GUILayout.BeginHorizontal(); // 开始一行
            GUILayout.Label("autoBlink:", GUILayout.Width(120)); // 开关标签
            Main.Settings.autoBlink = GUILayout.Toggle(Main.Settings.autoBlink, "On/Off"); // 开关按钮
            GUILayout.EndHorizontal(); // 结束一行
            if (Main.Settings.autoBlink)
            {
                Main.Settings.ya = GUILayout.Toggle(Main.Settings.ya, "y");
                if (Main.Settings.ya)
                {
                    Main.Settings.na = false;
                }
                Main.Settings.na = GUILayout.Toggle(Main.Settings.na, "n");
                if (Main.Settings.na)
                {
                    Main.Settings.ya = false;
                }
            }

            GUILayout.BeginHorizontal(); // 开始一行
            GUILayout.Label("autoAnimation:", GUILayout.Width(120)); // 开关标签
            Main.Settings.autoanimation = GUILayout.Toggle(Main.Settings.autoanimation, "On/Off"); // 开关按钮
            GUILayout.EndHorizontal(); // 结束一行
            if (Main.Settings.autoanimation)
            {
                Main.Settings.yb = GUILayout.Toggle(Main.Settings.yb, "y");
                if (Main.Settings.yb)
                {
                    Main.Settings.nb = false;
                }
                Main.Settings.nb = GUILayout.Toggle(Main.Settings.nb, "n");
                if (Main.Settings.nb)
                {
                    Main.Settings.yb = false;
                }
            }
            Main.Settings.PurePerfectSfxSound = GUILayout.Toggle(Main.Settings.PurePerfectSfxSound, "autoPurePerfectSfxSound");
            Main.Settings.AlwaysCountdown = GUILayout.Toggle(Main.Settings.AlwaysCountdown, "AlwaysCountdown");
            Main.Settings.TrueHit = GUILayout.Toggle(Main.Settings.TrueHit, "TrueHit");
            if (Main.Settings.TrueHit)
            {
                Main.Settings.enable = false;
                Main.Settings.TrueHitNodie = false;
            }
            Main.Settings.TrueHitNodie = GUILayout.Toggle(Main.Settings.TrueHitNodie, "TrueHit (No die)");
            if (Main.Settings.TrueHitNodie)
            {
                Main.Settings.enable = false;
                Main.Settings.TrueHit = false;
            }
            GUILayout.BeginHorizontal(); // 开始一行
            GUILayout.Label("TextChange:", GUILayout.Width(120)); // 开关标签
            Main.Settings.opena = GUILayout.Toggle(Main.Settings.opena, "On/Off"); // 开关按钮
            GUILayout.EndHorizontal(); // 结束一行
            if (Main.Settings.opena)
            {
                GUILayout.Label("AutoTextChange:", GUILayout.Width(120));
                Main.Settings.Autotext = GUILayout.TextField(Main.Settings.Autotext, GUILayout.Width(100));
                GUILayout.Label("OldTextChange:", GUILayout.Width(120));
                Main.Settings.OldText = GUILayout.TextField(Main.Settings.OldText, GUILayout.Width(100));
                GUILayout.Label("AutoTileTextChange:", GUILayout.Width(130));
                Main.Settings.AutoTileText = GUILayout.TextField(Main.Settings.AutoTileText, GUILayout.Width(100));

            }
            GUILayout.BeginHorizontal(); // 开始一行
            GUILayout.Label("EnableAutoColor:", GUILayout.Width(120)); // 开关标签
            Main.Settings.open = GUILayout.Toggle(Main.Settings.open, "On/Off"); // 开关按钮
            GUILayout.EndHorizontal(); // 结束一行
            if (Main.Settings.open)
            {
                GUILayout.Label("十六进制颜色代码:", GUILayout.Width(120));
                Main.Settings.hexColor = GUILayout.TextField(Main.Settings.hexColor, GUILayout.Width(100));

                // 尝试解析颜色并显示预览
                if (ColorUtility.TryParseHtmlString(Main.Settings.hexColor, out Color color))
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
            Main.Settings.Save(modEntry);
            Main.value = Main.Settings.value;
            Main.enable = Main.Settings.enable;
            Main.open = Main.Settings.open;
            Main.opena = Main.Settings.opena;
            Main.whiteAuto = Main.Settings.whiteAuto;
            Main.OldAuto = Main.Settings.OldAuto;
            Main.auto = Main.Settings.auto;
            Main.autoHit = Main.Settings.autoHit;
            Main.nextTileIsAuto = Main.Settings.nextTileIsAuto;
            Main.autoBlink = Main.Settings.autoBlink;
            Main.autoanimation = Main.Settings.autoanimation;
            Main.ya = Main.Settings.ya;
            Main.na = Main.Settings.na;
            Main.yb = Main.Settings.yb;
            Main.nb = Main.Settings.nb;
            Main.PurePerfectSfxSound = Main.Settings.PurePerfectSfxSound;
            Main.AlwaysCountdown = Main.Settings.AlwaysCountdown;
            Main.AutoText = Main.Settings.Autotext;
            Main.OldText = Main.Settings.OldText;
            Main.AutoTileText = Main.Settings.AutoTileText;
            Main.TrueHit = Main.Settings.TrueHit;
            Main.TrueHitNodie = Main.Settings.TrueHitNodie;
            UnityModManager.ModSettings.Save(Main.Settings, modEntry);
        }


        public static void OnHideGUI(UnityModManager.ModEntry modEntry)
        {
            OnSaveGUI(modEntry);
        }
    }
}