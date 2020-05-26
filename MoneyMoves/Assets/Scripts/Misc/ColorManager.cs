using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    #region Singleton
    private static ColorManager instance = null;

    public static ColorManager _instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ColorManager>();

                if (instance == null)
                {
                    instance = new ColorManager();
                }
            }

            return instance;
        }
    }
    #endregion

    public Color RandomBrightColor()
    {
        List<Color> colors = new List<Color>()
        {
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow
        };

        return colors[Random.Range(0, colors.Count)];
    }
}
