using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalPerlinNoise : PerlinNoise
{
    public FractalPerlinNoise(NoiseInfo info) : base(info)
    {
    }

    protected override float CreateNoise(float x, float y)
    {
        return FractalPerlinNoise2D(x, y);
    }

    private float FractalPerlinNoise2D(float x, float y)
    {
        var total = 0f;
        for (int i = 0; i < noiseInfo.octaves; i++)
        {
            var frequency = Mathf.Pow(2, i);
            var amplitude = Mathf.Pow(noiseInfo.persistence, i);
            total += PerlinNoise2D(x * frequency, y * frequency) * amplitude;
        }
        return total;
    }
}
