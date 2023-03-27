using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class DrawMeshArch : MonoBehaviour
{
    public bool isEnableMeshCollider = false;
    public float radius = 1;
    public float thickness = 0.5f;  // 厚度
    public int smooth = 6;          // 平滑度：边的数量
    [Range(0,360)]
    public float size = 120;        // 圆环角度
    public float depth = 1;         // 深度(Z轴)

    MeshFilter meshFilter;
    MeshCollider meshCollider;

    [ExecuteInEditMode]
    void Start()
    {
        meshCollider = gameObject.GetComponent<MeshCollider>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
    }

    [ExecuteInEditMode]
    void Update()
    {
        refreshMesh();
    }

    public void refreshMesh()
    {
        if(smooth <= 1 || (smooth % 2 != 0))
        {
            return;
        }

        float ereryAngle = size / smooth;

        List<Vector3> list_vertices = new List<Vector3>();      // 顶点
        List<Vector2> list_uv = new List<Vector2>();            // uv
        List<int> list_triangles = new List<int>();             // 三角形

        // 正面
        {
            int verticesCount = smooth / 2 * 4 + 2;

            // 顶点
            for (int i = 0; i < verticesCount; i += 2)
            {
                float curAngle = -size / 2 + (i / 2) * ereryAngle;
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), 0));
            }

            // uv
            for (int i = 0; i < verticesCount; i += 2)
            {
                float u_pos = (i / 2) / (float)smooth;
                list_uv.Add(new Vector2(u_pos, 0));
                list_uv.Add(new Vector2(u_pos, 1));
            }

            // 三角形,按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
            int trianglesCount = smooth * 2 * 3;
            for (int i = 0; i <= trianglesCount - 6; i += 6)
            {
                int firstNum = i / 3;
                list_triangles.Add(firstNum);
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 1);
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 3);
                list_triangles.Add(firstNum + 1);
            }
        }

        // 右面
        {
            // 顶点
            {
                float curAngle = size / 2;
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), 0));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, depth));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), depth));
            }

            // uv
            {
                list_uv.Add(new Vector2(0, 1));
                list_uv.Add(new Vector2(0, 0));
                list_uv.Add(new Vector2(1, 1));
                list_uv.Add(new Vector2(1, 0));
            }

            // 三角形,按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
            {
                int firstNum = list_triangles[list_triangles.Count - 2] + 1;
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 1);
                list_triangles.Add(firstNum);
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 3);
                list_triangles.Add(firstNum + 1);
            }
        }

        // 左面
        {
            // 顶点
            {
                float curAngle = -size / 2;
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, depth));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), depth));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), 0));
            }

            // uv
            {
                list_uv.Add(new Vector2(0, 1));
                list_uv.Add(new Vector2(0, 0));
                list_uv.Add(new Vector2(1, 1));
                list_uv.Add(new Vector2(1, 0));
            }

            // 三角形,按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
            {
                int firstNum = list_triangles[list_triangles.Count - 2] + 1;
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 1);
                list_triangles.Add(firstNum);
                list_triangles.Add(firstNum + 2);
                list_triangles.Add(firstNum + 3);
                list_triangles.Add(firstNum + 1);
            }
        }

        // 上面
        {
            int verticesCount = smooth / 2 * 4 + 2;

            // 顶点
            for (int i = 0; i < verticesCount; i += 2)
            {
                float curAngle = -size / 2 + (i / 2) * ereryAngle;
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, depth));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * radius, Mathf.Cos(Mathf.Deg2Rad * curAngle) * radius, 0));
            }

            // uv
            for (int i = 0; i < verticesCount; i += 2)
            {
                float u_pos = (i / 2) / (float)smooth;
                list_uv.Add(new Vector2(u_pos, 1));
                list_uv.Add(new Vector2(u_pos, 0));
            }

            // 三角形,按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
            {
                int trianglesCount = smooth * 2 * 3;
                int curIndex = list_triangles[list_triangles.Count - 2] + 1;
                for (int i = 0; i <= trianglesCount - 6; i += 6)
                {
                    int firstNum = curIndex + i / 3;
                    list_triangles.Add(firstNum);
                    list_triangles.Add(firstNum + 2);
                    list_triangles.Add(firstNum + 1);
                    list_triangles.Add(firstNum + 2);
                    list_triangles.Add(firstNum + 3);
                    list_triangles.Add(firstNum + 1);
                }
            }
        }

        // 下面
        {
            int verticesCount = smooth / 2 * 4 + 2;

            // 顶点
            for (int i = 0; i < verticesCount; i += 2)
            {
                float curAngle = -size / 2 + (i / 2) * ereryAngle;
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), 0));
                list_vertices.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * curAngle) * (radius - thickness), Mathf.Cos(Mathf.Deg2Rad * curAngle) * (radius - thickness), depth));
            }

            // uv
            for (int i = 0; i < verticesCount; i += 2)
            {
                float u_pos = (i / 2) / (float)smooth;
                list_uv.Add(new Vector2(u_pos, 1));
                list_uv.Add(new Vector2(u_pos, 0));
            }

            // 三角形,按照顶点顺时针方向，如果逆时针则需要翻转到背面才能看到贴图
            {
                int trianglesCount = smooth * 2 * 3;
                int curIndex = list_triangles[list_triangles.Count - 2] + 1;
                for (int i = 0; i <= trianglesCount - 6; i += 6)
                {
                    int firstNum = curIndex + i / 3;
                    list_triangles.Add(firstNum);
                    list_triangles.Add(firstNum + 2);
                    list_triangles.Add(firstNum + 1);
                    list_triangles.Add(firstNum + 2);
                    list_triangles.Add(firstNum + 3);
                    list_triangles.Add(firstNum + 1);
                }
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

        //mesh.vertices = list_vertices.ToArray();
        //mesh.uv = list_uv.ToArray();
        //mesh.triangles = list_triangles.ToArray();

        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
        //mesh.RecalculateTangents();

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
