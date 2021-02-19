using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalWhiteNoise : BaseNoise,INoiseBase
{
    private int octaves;
    private float persistence;
    private bool isSmooth;

    public FractalWhiteNoise(int w, int h, int octaves, float persistence, bool isSmooth) :base(w, h)
    {
        this.octaves = octaves;
        this.persistence = persistence;
        this.isSmooth = isSmooth;

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                float r = (FractalWhiteNoise1D(j, i) + 1) / 2f;
                colors[j + i * w] = new Color(r, r, r, 1);
            }
        }
    }

    /// <summary>
    /// 分形白噪声函数，来源 http://m.blog.csdn.net/mahabharata_/article/details/54743672
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private float FractalWhiteNoise1D(float x, float y)
    {
        var total = 0f;
        for (int i = 0; i < octaves; i++)
        {
            var frequency = Mathf.Pow(2, i);
            var amplitude = Mathf.Pow(persistence, i);
            total += InterpolatedNoise(x * frequency, y * frequency) * amplitude;
        }
        return total;
    }

    //插值函数
    private float InterpolatedNoise(float x, float y)
    {
        int ix = Mathf.FloorToInt(x);
        int iy = Mathf.FloorToInt(y);
        float fx = x - ix;
        float fy = y - iy;
        var v1 = isSmooth ? Smooth2D(ix, iy) : NoiseHelper.Random2D(ix, iy);
        var v2 = isSmooth ? Smooth2D(ix + 1, iy) : NoiseHelper.Random2D(ix + 1, iy);
        var v3 = isSmooth ? Smooth2D(ix, iy + 1) : NoiseHelper.Random2D(ix, iy + 1);
        var v4 = isSmooth ? Smooth2D(ix + 1, iy + 1) : NoiseHelper.Random2D(ix + 1, iy + 1);
        var i1 = cosine_interpolate(v1, v2, fx);
        var i2 = cosine_interpolate(v3, v4, fx);
        return cosine_interpolate(i1, i2, fy);
    }

    /// <summary>
    /// 余弦插值函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private float cosine_interpolate(float a, float b, float x)
    {
        float ft = x * Mathf.PI;
        float f = (1 - Mathf.Cos(ft)) * 0.5f;
        return a * (1 - f) + b * f;
    }

    /// <summary>
    /// 对2维基本噪声函数进行光滑处理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private float Smooth2D(int x, int y)
    {
        float res = NoiseHelper.Random2D(x, y) / 4f;
        res += (NoiseHelper.Random2D(x - 1, y) + NoiseHelper.Random2D(x + 1, y) + NoiseHelper.Random2D(x, y - 1) + NoiseHelper.Random2D(x, y + 1)) / 8f;
        res += (NoiseHelper.Random2D(x - 1, y - 1) + NoiseHelper.Random2D(x + 1, y - 1) + NoiseHelper.Random2D(x - 1, y + 1) + NoiseHelper.Random2D(x + 1, y + 1)) / 16f;
        return res;
    }
}
