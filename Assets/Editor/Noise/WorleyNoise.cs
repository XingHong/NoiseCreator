using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 算法参考：
/// https://blog.csdn.net/yolon3000/article/details/78386701
/// https://github.com/noobdawn/NoiseCreator/blob/3286890bed883ef00a4397cb2a3d43b578023318/Editor/NoiseCreator.cs#L400
/// </summary>
public class WorleyNoise : BaseNoise, INoiseBase
{
    protected NoiseInfo noiseInfo;
    private List<Vector2> randomPoint = new List<Vector2>();
    private Vector2 worleySpace = Vector2.one;

    public WorleyNoise(NoiseInfo info) : base(info.width, info.height)
    {
        noiseInfo = info;
        PrepareRandomPoints(info.width / info.worleySize, info.height / info.worleySize, info.maxPoint);
        for (int i = 0; i < info.height; i++)
        {
            for (int j = 0; j < info.width; j++)
            {
                //float r = (CreateNoise(j / noiseInfo.worleySize, i / noiseInfo.worleySize) + 1) / 2.0f;
                float r = CreateNoise(j / noiseInfo.worleySize, i / noiseInfo.worleySize);

                colors[j + i * info.width] = new Color(r, r, r, 1);
            }
        }
    }

    private void PrepareRandomPoints(float x, float y, int num)
    {
        randomPoint.Clear();
        worleySpace = new Vector2(x, y);
        for (int i = 0; i < num; i++)
        {
            randomPoint.Add(new Vector2(
                Random.Range(0f, x),
                Random.Range(0f, y)
                ));
        }
    }

    protected virtual float CreateNoise(float x, float y)
    {
        return WorleyNoise2D(x, y);
    }

    private float WorleyNoise2D(float x, float y)
    {
        Vector2 p = new Vector2(x, y);
        float res = worleySpace.magnitude;
        foreach (Vector2 rp in randomPoint)
        {
            var distanceRp = (p - rp).magnitude;
            res = distanceRp < res ? distanceRp : res;
        }
        return 1 - 4f * res / worleySpace.magnitude;
        //return res / worleySpace.magnitude;
    }
}
