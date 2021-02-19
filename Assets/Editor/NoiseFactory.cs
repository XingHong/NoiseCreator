using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoiseType { White, FractalWhite, Perlin, FractalPerlin, Value, FractalValue, Simplex, FractalSimplex };

public static class NoiseFactory
{

    static public Color[] GetNoise(NoiseInfo info, NoiseType nType)
    {
        INoiseBase noise = null;
        switch (nType)
        {
            case NoiseType.White:
                noise = new WiteNoise(info.width, info.height);
                break;
            case NoiseType.FractalWhite:
                noise = new FractalWhiteNoise(info.width, info.height, info.octaves, info.persistence, info.isSmooth);
                break;
            case NoiseType.Perlin:
                noise = new PerlinNoise(info);
                break;
            case NoiseType.FractalPerlin:
                noise = new FractalPerlinNoise(info);
                break;
            case NoiseType.Value:
                noise = new ValueNoise(info);
                break;
            case NoiseType.FractalValue:
                noise = new FractalValueNoise(info);
                break;
            case NoiseType.Simplex:
                noise = new SimplexNoise(info);
                break;
            default:
                break;
        }
        return noise.GetColorResult();
    }
}
