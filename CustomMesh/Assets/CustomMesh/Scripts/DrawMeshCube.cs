using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawMeshCube : MonoBehaviour
{
    public Vector3 size = new Vector3(1, 1, 1);
    public bool isEnableMeshCollider = false;

    MeshFilter meshFilter;
    MeshCollider meshCollider = null;

    [ExecuteInEditMode]
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    [ExecuteInEditMode]
    void Update()
    {
        refreshMesh();
    }

    void refreshMesh()
    {
        Vector3[] vertices = new Vector3[24]{

            // 正面
            new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f),

            // 右面
            new Vector3(size.x / 2f, size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f),

            // 左面
            new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f),

            // 上面
            new Vector3(size.x / 2f, size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f),

            // 下面
            new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f),

            // 后面
            new Vector3(size.x / 2f, size.y / 2f, size.z / 2f),
            new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f),
            new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f),
        };

        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i += 4)
        {
            uv[i] = new Vector2(1, 1);
            uv[i + 1] = new Vector2(1, 0);
            uv[i + 2] = new Vector2(0, 0);
            uv[i + 3] = new Vector2(0, 1);
        }

        int[] triangles = {
            0,1,2,0,2,3,        // 正
            4,5,6,4,6,7,        // 右
            8,9,10,8,10,11,     // 左
            12,13,14,12,14,15,  // 上
            16,18,17,16,19,18,  // 下(在背面，需要逆时针)
            20,22,21,20,23,22,  // 后(在背面，需要逆时针)
        };

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
