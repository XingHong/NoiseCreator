using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplexNoise : BaseNoise, INoiseBase
{
    NoiseInfo noiseInfo;
    private Vector2[] randomGrads;
    private int[] randomIndex;

    public SimplexNoise(NoiseInfo info) : base(info.width, info.height)
    {
        noiseInfo = info;
        InitGradArray(info.seed);
        for (int i = 0; i < info.height; i++)
        {
            for (int j = 0; j < info.width; j++)
            {
                float r = (CreateNoise(new Vector2(j / (float)info.proportion, i / (float)info.proportion)) + 1) / 2f ;
                colors[j + i * info.width] = new Color(r, r, r, 1);
            }
        }
    }

    private void InitGradArray(int seed)
    {
        int max = Mathf.Max(noiseInfo.width, noiseInfo.height);
        randomGrads = NoiseHelper.GetRandomGradsArray(max + 1, seed);
        randomIndex = NoiseHelper.GetRandomIndexArray(max + 1);
    }

    protected virtual float CreateNoise(Vector2 p)
    {
        return SimplexNoise2D(p);
    }

    /// <summary>
    /// 参考：https://zhuanlan.zhihu.com/p/240763739
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    protected float SimplexNoise2D(Vector2 p)
    {
        int max = Mathf.Max(noiseInfo.width, noiseInfo.height);
        float F = 0.366025404f;  // (sqrt(3)-1)/2;
        float G = 0.211324865f; // (3-sqrt(3))/6;
        //这里的P是2d单形网格里的输入点，而i是超立方体网格中的0，0点
        Vector2 i = Vector2.one;
        i.x = Mathf.FloorToInt(p.x + (p.x + p.y) * F);
        i.y = Mathf.FloorToInt(p.y + (p.x + p.y) * F);
        //变形前输入点到(0, 0)点的距离向量
        Vector2 a = Vector2.one;
        a.x = p.x - (i.x - (i.x + i.y) * G);
        a.y = p.y - (i.y - (i.x + i.y) * G);
        Vector2 o = a.x < a.y ? new Vector2(0, 1) : new Vector2(1, 0);
        //变形前输入点到(1, 0)点或(0, 1)点的距离向量
        Vector2 b = Vector2.one;
        b.x = a.x - o.x + G;
        b.y = a.y - o.y + G;
        //变形前输入点到(1, 1)点的距离向量
        Vector2 c = Vector2.one;
        c.x = a.x - 1.0f + 2.0f * G;
        c.y = a.y - 1.0f + 2.0f * G;
        //Simplex noise公式
        Vector3 h = Vector3.one;
        h.x = Mathf.Max(0.5f - a.sqrMagnitude, 0.0f);
        h.y = Mathf.Max(0.5f - b.sqrMagnitude, 0.0f);
        h.z = Mathf.Max(0.5f - c.sqrMagnitude, 0.0f);
        
        Vector3 n = Vector3.one;
        n.x = pow4(h.x) * Vector2.Dot(a, hash22(i, max));
        n.y = pow4(h.y) * Vector2.Dot(b, hash22(new Vector2(i.x + o.x, i.y + o.y), max));
        n.z = pow4(h.z) * Vector2.Dot(c, hash22(new Vector2(i.x + 1.0f, i.y + 1.0f), max));

        Vector3 v = Vector3.one * 70.0f;
        return Vector3.Dot(v, n);
    }

    private float pow4(float a)
    {
        return a * a * a * a;
    }

    private Vector2 hash22(Vector2 v2, int max)
    {
        int xIndex = (int)v2.x % max;
        int yIndex = (int)v2.y % max;
        int index = randomIndex[(xIndex + randomIndex[yIndex]) % max];
        return randomGrads[index];
    }
}
