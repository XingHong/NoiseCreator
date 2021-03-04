using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : BaseNoise, INoiseBase
{
    protected NoiseInfo noiseInfo;
    private Vector2[] randomGrads;
    private int[] randomIndex;

    public PerlinNoise(NoiseInfo info) : base(info.width, info.height)
    {
        noiseInfo = info;
        InitGradArray(info.seed);
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
                    r = (CreateNoise(j / (float)(info.width - 1), i / (float)(info.height - 1)) * 0.5f + 0.5f);
                }
                colors[j + i * info.width] = new Color(r, r, r, 1);
            }
        }
    }

    protected virtual float CreateNoise(float x, float y)
    {
        if (noiseInfo.isSeamless)
        {
            SeamlessValueNoise(x, y, noiseInfo.period);
        }
        return PerlinNoise2D(x, y);
    }

    private void InitGradArray(int seed)
    {
        int max = Mathf.Max(noiseInfo.width, noiseInfo.height);
        randomGrads = noiseInfo.isSeamless ? NoiseHelper.GetSeamlessRandomGradsArray(seed) : NoiseHelper.GetRandomGradsArray(max + 1, seed);
        randomIndex = NoiseHelper.GetRandomIndexArray(max + 1);
    }

    /// <summary>
    /// 柏林噪音，原理来自 http://www.twinklingstar.cn/2015/2581/classical-perlin-noise/
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    protected float PerlinNoise2D(float x, float y)
    {
        int max = Mathf.Max(noiseInfo.width, noiseInfo.height);
        //获取诸方格顶点
        int x0 = Mathf.FloorToInt(x) % max;
        int x1 = (Mathf.FloorToInt(x) + 1) % max;
        int y0 = Mathf.FloorToInt(y) % max;
        int y1 = (Mathf.FloorToInt(y) + 1) % max;
        Vector2 v00 = new Vector2(x - x0, y - y0);
        Vector2 v10 = new Vector2(x - x1, y - y0);
        Vector2 v01 = new Vector2(x - x0, y - y1);
        Vector2 v11 = new Vector2(x - x1, y - y1);
        var s = Vector2.Dot(randomGrads[(x0 + randomIndex[y0]) % max], v00);
        var t = Vector2.Dot(randomGrads[(x1 + randomIndex[y0]) % max], v10);
        var u = Vector2.Dot(randomGrads[(x0 + randomIndex[y1]) % max], v01);
        var v = Vector2.Dot(randomGrads[(x1 + randomIndex[y1]) % max], v11);
        var a = s + (t - s) * NoiseHelper.EaseCurveInterpolate(0, 1, x - x0);
        var b = u + (v - u) * NoiseHelper.EaseCurveInterpolate(0, 1, x - x0);
        return a + (b - a) * NoiseHelper.EaseCurveInterpolate(0, 1, y - y0);
    }

    protected float SeamlessValueNoise(float x, float y, float period)
    {
        Vector2 p = new Vector2(x * period, y * period);
        int x0 = Mathf.FloorToInt(p.x);
        int x1 = (Mathf.FloorToInt(p.x) + 1);
        int y0 = Mathf.FloorToInt(p.y);
        int y1 = (Mathf.FloorToInt(p.y) + 1);
        Vector2 v00 = new Vector2(p.x - x0, p.y - y0);
        Vector2 v10 = new Vector2(p.x - x1, p.y - y0);
        Vector2 v01 = new Vector2(p.x - x0, p.y - y1);
        Vector2 v11 = new Vector2(p.x - x1, p.y - y1);
        float d00 = Vector2.Dot(v00, GetRandomGrad(v00, period));
        float d10 = Vector2.Dot(v10, GetRandomGrad(v10, period));
        float d01 = Vector2.Dot(v01, GetRandomGrad(v01, period));
        float d11 = Vector2.Dot(v11, GetRandomGrad(v11, period));
        float dx0 = NoiseHelper.EaseCurveInterpolate2(d00, d10, v00.x);
        float dx1 = NoiseHelper.EaseCurveInterpolate2(d01, d11, v00.x);
        float dxy = NoiseHelper.EaseCurveInterpolate2(dx0, dx1, v00.y);
        return dxy;
    }

    private Vector2 GetRandomGrad(Vector2 v, float period)
    {
        float[] r = NoiseHelper.SeamlessNoise(v.x, v.y, period);
        float i = r[0];
        float index = i - Mathf.FloorToInt(i);
        index *= randomGrads.Length;
        return randomGrads[Mathf.FloorToInt(index)];
    }
}
