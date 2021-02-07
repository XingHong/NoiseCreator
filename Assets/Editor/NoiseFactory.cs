using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoiseType { White, FractalWhite };

public static class NoiseFactory
{
    static public Color[] GetNoise(int w, int h, NoiseType nType)
    {
        INoiseBase noise = null;

        switch (nType)
        {
            case NoiseType.White:
                noise = new WiteNoise(w, h);
                break;
            default:
                break;
        }

        return noise.GetColorResult();
    }

    static public Color[] GetFractalNoise(int w, int h, int octaves, float persistence, bool isSmooth, NoiseType nType)
    {
        INoiseBase noise = null;

        switch (nType)
        {
            case NoiseType.FractalWhite:
                noise = new FractalWhiteNoise(w, h, octaves, persistence, isSmooth);
                break;
            default:
                break;
        }

        return noise.GetColorResult();
    }
}
