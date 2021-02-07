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
    private int s_octaves = 4;
    private float s_persistence = 0.5f;
    private string str_persistence = "0.5";
    private bool isSmooth = true;

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
        GUILayout.Label("倍频(octaves,分形用):", GUILayout.Width(150));
        s_octaves = int.Parse(GUILayout.TextField(s_octaves.ToString()));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("持续度(persistence,分形用):", GUILayout.Width(150));
        str_persistence = GUILayout.TextField(str_persistence);
        float.TryParse(str_persistence, out s_persistence);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        isSmooth = GUILayout.Toggle(isSmooth, "光滑(分形用)");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建白噪声纹理"))
        {
            Create2D(NoiseFactory.GetNoise(s_width, s_height, NoiseType.White));
        }
        if (GUILayout.Button("创建分形白噪声纹理"))
        {
            Create2D(NoiseFactory.GetFractalNoise(s_width, s_height, s_octaves, s_persistence, isSmooth, NoiseType.FractalWhite));
        }
        GUILayout.EndHorizontal();
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
