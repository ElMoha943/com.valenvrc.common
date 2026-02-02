using UdonSharp;
using UnityEngine;

namespace valenvrc.Common
{
    public class Utility : UdonSharpBehaviour
    {
        public static string GetColorHex(Color c)
        {
            Color32 color2 = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(c.r * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(c.g * 255f), 0, 255), (byte)Mathf.Clamp(Mathf.RoundToInt(c.b * 255f), 0, 255), 1);
            string cs =  string.Format("{0:X2}{1:X2}{2:X2}", color2.r, color2.g, color2.b);
            return cs;
        }
    }
}