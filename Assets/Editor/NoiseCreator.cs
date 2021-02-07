using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class NoiseCreator : EditorWindow
{
    private string s_path = "";
    private int s_width = 128;
    private int s_height = 128;
    private Texture2D s_tex = null;

    [MenuItem("Tools/Noise Creator")]
    private static void ShowWindow()
    {
        GetWindow<NoiseCreator>();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("宽:", GUILayout.Width(50));
        s_width = int.Parse(GUILayout.TextField(s_width.ToString()));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("高:", GUILayout.Width(50));
        s_height = int.Parse(GUILayout.TextField(s_height.ToString()));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建白噪声纹理"))
        {
            Create2D(WhiteNoise());
        }
        if (GUILayout.Button("创建分形白噪声纹理"))
        {
            //Create2D(FractalWhiteNoise(s_width, s_height));
        }
        GUILayout.EndHorizontal();
    }

    private Color[] WhiteNoise()
    {
        Color[] colors = new Color[s_width * s_height];
        for (int i = 0; i < s_height; i++)
            for (int j = 0; j < s_width; j++)
            {
                float r = (Random1D(j, i) + 1) / 2f;
                colors[j + i * s_width] = new Color(r, r, r, 1);
            }
        return colors;
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
    private float Random1D(int x, int y)
    {
        int n = 12211 * x + 7549 * y;
        n = (n << 13) ^ n;
        return (1f - (n * (n * n * 15731 + 789221) + 1376312589 & 0x7fffffff) / 1073741824f);
    }

    private void Create2D(Color[] colors)
    {
        s_tex = new Texture2D(s_width, s_height, TextureFormat.ARGB32, false);
        s_tex.SetPixels(colors);
        s_tex.Apply();
        byte[] bytes = s_tex.EncodeToPNG();
        s_path = Application.dataPath + "/noise.png";
        Debug.Log(s_path);
        FileStream fs = new FileStream(s_path, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(bytes);
        fs.Close();
        bw.Close();
        AssetDatabase.Refresh();
    }
}
