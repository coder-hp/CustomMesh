using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawMeshPlane : MonoBehaviour
{
    public Vector2 size = new Vector2(1, 1);
    public bool isEnableMeshCollider = false;

    MeshFilter meshFilter = null;
    MeshCollider meshCollider = null;

    [ExecuteInEditMode]
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        refreshMesh();
    }

    [ExecuteInEditMode]
    void Update()
    {
        refreshMesh();
    }

    void refreshMesh()
    {
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-size.x / 2f, size.y / 2f, 0),
            new Vector3(size.x / 2f, size.y / 2f, 0),
            new Vector3(size.x / 2f, -size.y / 2f, 0),
            new Vector3(-size.x / 2f, -size.y / 2f, 0)
        };

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0,0)
        };

        // 按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
        int[] triangles = { 0, 1, 3, 1, 2, 3 };

        Mesh mesh = new Mesh()
        {
            vertices = vertices,
            uv = uv,
            triangles = triangles,
        };

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // 碰撞盒
        {
            meshCollider.enabled = isEnableMeshCollider;
            if (isEnableMeshCollider)
            {
                // mesh改了之后，sharedMesh必须重新赋值一遍，不然不会生效
                meshCollider.sharedMesh = mesh;
            }
        }
    }
}
