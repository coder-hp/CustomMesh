using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[ExecuteInEditMode]
public class DrawMeshCone : MonoBehaviour
{
    public float radius = 1;                    // 半径
    public float height = 1;                    // 高度
    [Range(3,100)]
    public int smooth = 6;                      // 平滑度：边的数量
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
        List<Vector3> list_vertices = new List<Vector3>();      // 顶点
        List<Vector2> list_uv = new List<Vector2>();            // uv
        List<int> list_triangles = new List<int>();             // 三角形

        // 侧面
        {
            list_vertices.Add(new Vector3(0, height, 0));

            // 顶点,为什么i <= smooth?因为需要首尾相接，所以需要多出两个顶点
            for (int i = 0; i <= smooth; i++)
            {
                // 为什么+90?因为0度坐标为(1,0)，而这里希望0度坐标为(0,1)
                // 而且这里的坐标系正方向为逆时针，所以需要+90
                float curAngle = 360f / smooth * i + 90;
                list_vertices.Add(new Vector3(Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0, Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius));
            }

            // uv
            {
                list_uv.Add(new Vector2(0.5f,1));
                for (int i = 0; i <= smooth; i++)
                {
                    list_uv.Add(new Vector2((float)i / smooth, 0));
                }
            }

            // 三角形
            {
                for (int i = 0; i < smooth; i++)
                {
                    list_triangles.Add(0);
                    list_triangles.Add(i + 2);
                    list_triangles.Add(i + 1);
                }

                // 首尾相接
                list_triangles.Add(0);
                list_triangles.Add(smooth + 1);
                list_triangles.Add(smooth);
            }
        }

        // 底部
        {
            int verticesCount = 1 + smooth;

            // 顶点
            {
                list_vertices.Add(new Vector3(0, 0, 0));
                for (int i = 0; i < smooth; i++)
                {
                    float curAngle = 360f / smooth * i + 90;
                    list_vertices.Add(new Vector3(Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0, Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius));
                }
            }

            // uv
            {
                list_uv.Add(new Vector2(0.5f, 0.5f));
                for (int i = 0; i < smooth; i++)
                {
                    float curAngle = 360f / smooth * i + 90;
                    list_uv.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * curAngle) / 2f, Mathf.Sin(Mathf.Deg2Rad * curAngle) / 2f) + new Vector2(0.5f, 0.5f));
                }
            }

            // 三角形,底部需要逆时针
            {
                for (int i = 0; i < smooth - 1; i++)
                {
                    list_triangles.Add(list_vertices.Count - verticesCount + 0 + i + 1);
                    list_triangles.Add(list_vertices.Count - verticesCount + 0 + i + 2);
                    list_triangles.Add(list_vertices.Count - verticesCount + 0);
                }

                // 首尾相接
                list_triangles.Add(list_vertices.Count - verticesCount + smooth);
                list_triangles.Add(list_vertices.Count - verticesCount + 1);
                list_triangles.Add(list_vertices.Count - verticesCount + 0);
            }
        }

        Mesh mesh = new Mesh()
        {
            vertices = list_vertices.ToArray(),
            uv = list_uv.ToArray(),
            triangles = list_triangles.ToArray(),
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
