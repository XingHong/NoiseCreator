using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseHelper
{
    public static float Random2D(int x, int y)
    {
        int n = 12211 * x + 7549 * y;
        n = (n << 13) ^ n;
        return (1f - (n * (n * n * 15731 + 789221) + 1376312589 & 0x7fffffff) / 1073741824f);
    }

    public static Vector2 RandomVector2D(int x, int y)
    {
        Vector2 res = new Vector2();
        int n = 49993 * x + 17 * x * y;
        n = (n << 13) ^ n;
        res.x = (1f - (n * (n * n * 15731 + 789221) + 1376312589 & 0x7fffffff) / 1073741824f);
        n = 35279 * y + 23 * x * y;
        n = (n << 13) ^ n;
        res.y = (1f - (n * (n * n * 15731 + 789221) + 1376312589 & 0x7fffffff) / 1073741824f);
        return res;
    }
}
