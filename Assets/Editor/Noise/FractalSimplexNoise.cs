using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalSimplexNoise : SimplexNoise {

    public FractalSimplexNoise(NoiseInfo info) : base(info)
    {
    }

    protected override float CreateNoise(Vector2 p)
    {
        return FractalSimplexNoise2D(p);
    }

    private float FractalSimplexNoise2D(Vector2 p)
    {
        var total = 0f;
        for (int i = 0; i < noiseInfo.octaves; i++)
        {
            var frequency = Mathf.Pow(2, i);
            var amplitude = Mathf.Pow(noiseInfo.persistence, i);
            total += SimplexNoise2D(p * frequency) * amplitude;
        }
        return total;
    }
}
