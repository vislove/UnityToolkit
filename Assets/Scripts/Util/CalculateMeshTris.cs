#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class CalculateMeshTris : MonoBehaviour
{
    [SerializeField] private bool active = true;
 
    private int verts;
    private int tris;
 
    private GUIStyle textStyle = new GUIStyle();
 
    private void Start()
    {
        textStyle.normal.background = null;
        textStyle.normal.textColor = new Color(1.0f,0.5f,0.0f);
        textStyle.fontSize = 30;
 
        if(active)
        {
            Selection.selectionChanged += CalculateVertsAndTris;
            CalculateVertsAndTris();
        }
    }
 
    private void CalculateVertsAndTris()
    {
        GameObject[] objs = Selection.gameObjects;
 
        int verts1 = 0, tris1 = 0, verts2 = 0, tris2 = 0;
 
        foreach(GameObject obj in objs)
        {
            MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();
            foreach(MeshFilter filter in filters)
            {
                tris1 += filter.sharedMesh.triangles.Length;
                verts1 += filter.sharedMesh.vertexCount;
            }
 
            SkinnedMeshRenderer[] renders = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer render in renders)
            {
                tris2 += render.sharedMesh.triangles.Length;
                verts2 += render.sharedMesh.vertexCount;
            }
        }
        tris1 /= 3;
        tris2 /= 3;
 
        verts = verts1 + verts2;
        tris = tris1 + tris2;
 
        Debug.Log(string.Format("verts: {0}+{1}={2}; tris: {3}+{4}={5}",verts1,verts2,verts,tris1,tris2,tris));
    }
 
    private void OnGUI()
    {
        if(active)
            GUILayout.Label(string.Format("verts: {0}; tris: {1}",verts,tris),textStyle);
    }
}

#endif