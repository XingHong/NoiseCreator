using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalValueNoise : ValueNoise
{
    public FractalValueNoise(NoiseInfo info) : base(info)
    {
    }

    protected override float CreateNoise(float x, float y)
    {
        var total = 0f;
        if (!noiseInfo.isSeamless)
        {
            for (int i = 0; i < noiseInfo.octaves; i++)
            {
                var frequency = Mathf.Pow(2, i);
                var amplitude = Mathf.Pow(noiseInfo.persistence, i);
                total += ValueNoise2D(x * frequency, y * frequency) * amplitude;
            }
        }
        else
        {
            float period = noiseInfo.period;
            float t = 0;
            for (int i = 0; i < noiseInfo.octaves; i++)
            {
                var frequency = Mathf.Pow(2, i);
                var amplitude = Mathf.Pow(noiseInfo.persistence, i);
                total += SeamlessValueNoise(x , y , period) * amplitude;
                period *= 2;
                t += amplitude;
            }
            total /= t;
        }
        return total;
    }
}
