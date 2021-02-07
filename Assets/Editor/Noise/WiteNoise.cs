using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiteNoise : BaseNoise, INoiseBase
{

    public WiteNoise(int w, int h): base(w, h)
    {
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                float r = (NoiseHelper.Random2D(j, i) + 1) / 2f;
                colors[j + i * w] = new Color(r, r, r, 1);
            }
        }
    }

    private float Random1D()
    {
        return Random.Range(0f, 1f);
    }

    private float Random1D(int px)
    {
        int n = px;
        n = (n << 13) ^ n;
        return (1f - (n * (n * n * 15731 + 789221) + 1376312589 & 0x7fffffff) / 1073741824f);
    }

}
