using Hitmargin_AutoplayHit;
using UnityEngine;


namespace AutoplayHit
{
    public class Text : MonoBehaviour
    {
        private void OnGUI()
        {
            bool flag = Main.Settings.TextShadow > 0;
            if (flag)
            {
                GUIStyle guistyle = new GUIStyle();
                guistyle.fontSize = Main.Settings.TextSize;
                guistyle.font = RDString.GetFontDataForLanguage(SystemLanguage.Korean).font;
                guistyle.normal.textColor = Color.black.WithAlpha((float)Main.Settings.TextShadow / 100f);
                GUI.Label(new Rect((float)(Main.Settings.PositionX + 12), (float)(Main.Settings.PositionY - 8), (float)Screen.width, (float)Screen.height), Text.Content, guistyle);
            }
            GUIStyle guistyle2 = new GUIStyle();
            guistyle2.fontSize = Main.Settings.TextSize;
            guistyle2.font = RDString.GetFontDataForLanguage(SystemLanguage.Korean).font;
            guistyle2.normal.textColor = Color.white;
            GUI.Label(new Rect((float)(Main.Settings.PositionX + 10), (float)(Main.Settings.PositionY - 10), (float)Screen.width, (float)Screen.height), Text.Content, guistyle2);
        }
        public static string Content = "";
    }
}

