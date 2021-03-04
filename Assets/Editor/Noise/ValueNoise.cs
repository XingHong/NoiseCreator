using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueNoise : BaseNoise, INoiseBase
{
    protected NoiseInfo noiseInfo;
    public ValueNoise(NoiseInfo info) : base(info.width, info.height)
    {
        noiseInfo = info;
        float r;
        for (int i = 0; i < info.height; i++)
        {
            for (int j = 0; j < info.width; j++)
            {
                if (!noiseInfo.isSeamless)
                {
                   r = (CreateNoise(j / (float)info.proportion, i / (float)info.proportion) + 1) / 2f;
                }
                else
                {
                   r = (CreateNoise(j / (float)(info.width - 1), i / (float)(info.height - 1)));
                }
                colors[j + i * info.width] = new Color(r, r, r, 1);
            }
        }
    }


    protected virtual float CreateNoise(float x, float y)
    {
        if (noiseInfo.isSeamless)
        {
            return SeamlessValueNoise(x, y, noiseInfo.period);
        }
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
    }

    protected float SeamlessValueNoise(float x, float y, float period)
    {
        Vector2 p = new Vector2(x * period, y * period);

        int x0 = Mathf.FloorToInt(p.x);
        int x1 = x0 + 1;
        int y0 = Mathf.FloorToInt(p.y);
        int y1 = y0 + 1;

        var d00 = GetValue(x0, y0, period);
        var d10 = GetValue(x1, y0, period);
        var d01 = GetValue(x0, y1, period);
        var d11 = GetValue(x1, y1, period);
        var dx0 = NoiseHelper.EaseCurveInterpolate(d00, d10, p.x - x0);
        var dx1 = NoiseHelper.EaseCurveInterpolate(d01, d11, p.x - x0);
        return NoiseHelper.EaseCurveInterpolate(dx0, dx1, p.y - y0);
    }

    private float GetValue(float x, float y, float period)
    {
        float[] r = NoiseHelper.SeamlessNoise(x, y, period);
        return r[0];
    }
}
