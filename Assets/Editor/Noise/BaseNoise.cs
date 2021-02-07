using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNoise: INoiseBase
{

    protected Color[] colors;

    public BaseNoise(int w, int h)
    {
        colors = new Color[w * h];
    }

    public Color[] GetColorResult()
    {
        return colors;
    }
}
