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
        return PerlinNoise2D(x, y);
    }

    private void InitGradArray(int seed)
    {
        int max = Mathf.Max(noiseInfo.width, noiseInfo.height);
        randomGrads = new Vector2[max + 1];
        randomIndex = new int[max + 1];
        for (int i = 0; i < max + 1; i++)
        {
            randomGrads[i] = NoiseHelper.RandomVector2D(i, seed).normalized;
            randomIndex[i] = i;
        }
        //对随机排列表进行洗牌
        int idx = max;
        int temp = 0;
        while (idx > 0)
        {
            int r = Random.Range(0, max);
            temp = randomIndex[r];
            randomIndex[r] = randomIndex[idx];
            randomIndex[idx] = temp;
            idx--;
        }
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
}
