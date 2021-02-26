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

    public static float[] Random4D(float x, float y, float z, float w)
    {
        Vector4 p = new Vector4(x, y, z, w);
        Vector4 v1 = new Vector4(114.5f, 141.9f, 198.1f, 175.5f);
        Vector4 v2 = new Vector4(364.3f, 648.8f, 946.4f, 431.7f);
        Vector4 v3 = new Vector4(190.3f, 233.5f, 716.9f, 362.0f);
        Vector4 v4 = new Vector4(273.1f, 558.4f, 113.05f, 285.4f);
        Vector4 resultP = new Vector4(
            Vector4.Dot(p, v1),
            Vector4.Dot(p, v2),
            Vector4.Dot(p, v3),
            Vector4.Dot(p, v4)
            );
        float[] result = new float[4];
        for (int i = 0; i < 4; ++i)
        {
            result[i] = Frac(Sin(resultP[i]) * 643.1f);
        }
        return result;
    }

    //参考unity Mathematics库
    private static float Sin(float x)
    {
        return (float)System.Math.Sin(x);
    }

    private static float Frac(float x)
    {
        return x - Mathf.FloorToInt(x);
    }

    /// <summary>
    /// 缓和曲线插值，用的是传统的方法
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="x">比例，需在0~1之间</param>
    /// <returns></returns>
    public static float EaseCurveInterpolate(float a, float b, float x)
    {
        if (x >= 1) return b;
        if (x <= 0) return a;
        return a + (b - a) * ((3 - 2 * x) * x * x);
    }

    /// <summary>
    /// 缓和曲线插值，用的是新的方法
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float EaseCurveInterpolate2(float a, float b, float x)
    {
        if (x >= 1) return b;
        if (x <= 0) return a;
        return a + (b - a) * ((6 * x * x - 15 * x + 10) * x * x * x);
    }

    /// <summary>
    /// 获取随机梯度数组
    /// </summary>
    /// <param name="length"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static Vector2[] GetRandomGradsArray(int length, int seed)
    {
        Vector2[] arr = new Vector2[length];
        for (int i = 0; i < length; ++i)
        {
            arr[i] = RandomVector2D(i, seed).normalized;
        }
        return arr;
    }

    /// <summary>
    /// 获取随机数组
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static int[] GetRandomIndexArray(int length)
    {
        int[] arr = new int[length];
        for (int i = 0; i < length - 1; ++i)
        {
            arr[i] = i;
        }

        //对随机排列表进行洗牌
        int idx = length - 1;
        int temp = 0;
        while (idx > 0)
        {
            int r = Random.Range(0, length - 1);
            temp = arr[r];
            arr[r] = arr[idx];
            arr[idx] = temp;
            idx--;
        }
        return arr;
    }
}
