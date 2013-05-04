using UnityEngine;
using System.Collections;

public static class Utils {

    public static float MOVE_PADDING = 0.001f;

    public static float ZINC = 0.0001f;

    private static float pixelsPerVicPixel = (float)Screen.height / 288f;

    //public static ACTUAL_PIXELS_PER_TILE;

    public static float VIC2PIX
    {
        get
        {
            return Utils.pixelsPerVicPixel;
        }
    }

    //public Utils()
    //{
    //    // target is 288 height soooo
    //    Utils.pixelsPerVicPixel = (float)Screen.height / 288f;
    // }

}
