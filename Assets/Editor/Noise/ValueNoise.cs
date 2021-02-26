using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueNoise : BaseNoise, INoiseBase
{
    protected NoiseInfo noiseInfo;
    public ValueNoise(NoiseInfo info) : base(info.width, info.height)
    {
        noiseInfo = info;
        for (int i = 0; i < info.height; i++)
        {
            for (int j = 0; j < info.width; j++)
            {
                float r = (CreateNoise(j / (float)info.proportion, i / (float)info.proportion) + 1) / 2f;
                colors[j + i * info.width] = new Color(r, r, r, 1);
            }
        }
    }


    protected virtual float CreateNoise(float x, float y)
    {
        return ValueNoise2D(x, y);
    }

    /// <summary>
    /// 参考：https://blog.csdn.net/candycat1992/article/details/50346469
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    protected float ValueNoise2D(float x, float y)
    {
        int x0 = Mathf.FloorToInt(x);
        int x1 = x0 + 1;
        int y0 = Mathf.FloorToInt(y);
        int y1 = y0 + 1;

        var s = NoiseHelper.Random2D(x0 * (1993 * x0), y0 * (1993 * y0));
        var t = NoiseHelper.Random2D(x1 * (1993 * x1), y0 * (1993 * y0));
        var u = NoiseHelper.Random2D(x0 * (1993 * x0), y1 * (1993 * y1));
        var v = NoiseHelper.Random2D(x1 * (1993 * x1), y1 * (1993 * y1));
        var a = s + (t - s) * NoiseHelper.EaseCurveInterpolate(0, 1, x - x0);
        var b = u + (v - u) * NoiseHelper.EaseCurveInterpolate(0, 1, x - x0);
        return a + (b - a) * NoiseHelper.EaseCurveInterpolate(0, 1, y - y0);
        /*var d00 = SeamlessNoise(x0, y0);
        var d10 = SeamlessNoise(x1, y0);
        var d01 = SeamlessNoise(x0, y1);
        var d11 = SeamlessNoise(x1, y1);
        var dx0 = NoiseHelper.EaseCurveInterpolate(d00, d10, x - x0);
        var dx1 = NoiseHelper.EaseCurveInterpolate(d01, d11, x - x0);
        return NoiseHelper.EaseCurveInterpolate(dx0, dx1, y - y0);*/
    }

    private float SeamlessNoise(float x, float y)
    {
        float x0 = Mathf.Cos(x * 2 * Mathf.PI);
        float x1 = Mathf.Sin(x * 2 * Mathf.PI);
        float x2 = Mathf.Cos(y * 2 * Mathf.PI);
        float x3 = Mathf.Sin(y * 2 * Mathf.PI);
        float[] r4 = NoiseHelper.Random4D(x0, x1, x2, x3);
        return r4[0];
    }
}
