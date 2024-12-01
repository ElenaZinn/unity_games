using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    //用来存放顶点数据
    List<Vector3> verts;
    //顶点序号， 组成三角面
    List<int> indices;
    List<Vector2> uvs;


    // Start is called before the first frame update
    void Start()
    {
        verts = new List<Vector3>();
        indices = new List<int>();
        uvs = new List<Vector2>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        
        Generate();
    }

    void Generate()
    {
        ClearMeshData();
        //把数值填写好
        AddMeshData();

        //把数据传递给Mesh, 生成真正的网格
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = indices.ToArray();
        //贴图用
        mesh.uv = uvs.ToArray();

        //自动计算法线
        mesh.RecalculateNormals();
        //自动极端物体的整体边界
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
        //碰撞体专用的mesh, 只负责物体的碰撞外形
        meshCollider.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        Generate();

    }


    void ClearMeshData()
    {
        verts.Clear();
        indices.Clear();
        uvs.Clear();
    }

    void AddMeshData()
    {
        //后
        verts.Add(new Vector3(0, 0, 0));
        verts.Add(new Vector3(0, 1, 0));
        verts.Add(new Vector3(1, 1, 0));
        verts.Add(new Vector3(1, 0, 0));

        uvs.Add(new Vector2(0 / 6f, 0));
        uvs.Add(new Vector2(0 / 6f, 1));
        uvs.Add(new Vector2(1 / 6f, 1));
        uvs.Add(new Vector2(1 / 6f, 0));

        //右
        verts.Add(new Vector3(1, 0, 0));
        verts.Add(new Vector3(1, 1, 0));
        verts.Add(new Vector3(1, 1, 1));
        verts.Add(new Vector3(1, 0, 1));


        uvs.Add(new Vector2(1 / 6f, 0));
        uvs.Add(new Vector2(1 / 6f, 1));
        uvs.Add(new Vector2(2 / 6f, 1));
        uvs.Add(new Vector2(2 / 6f, 0));


        //顶
        verts.Add(new Vector3(0, 1, 0));
        verts.Add(new Vector3(0, 1, 1));
        verts.Add(new Vector3(1, 1, 1));
        verts.Add(new Vector3(1, 1, 0));

        uvs.Add(new Vector2(5 / 6f, 0));
        uvs.Add(new Vector2(4 / 6f, 0));
        uvs.Add(new Vector2(4 / 6f, 1));
        uvs.Add(new Vector2(5 / 6f, 1));
       

        //底
        verts.Add(new Vector3(0, 0, 0));
        verts.Add(new Vector3(1, 0, 0));
        verts.Add(new Vector3(1, 0, 1));
        verts.Add(new Vector3(0, 0, 1));

        uvs.Add(new Vector2(5 / 6f, 0));
        uvs.Add(new Vector2(5 / 6f, 1));
        uvs.Add(new Vector2(6 / 6f, 1));
        uvs.Add(new Vector2(6 / 6f, 0));


        //前
        verts.Add(new Vector3(0, 0, 1));
        verts.Add(new Vector3(1, 0, 1));
        verts.Add(new Vector3(1, 1, 1));
        verts.Add(new Vector3(0, 1, 1));

        uvs.Add(new Vector2(3 / 6f, 0));
        uvs.Add(new Vector2(2 / 6f, 0));
        uvs.Add(new Vector2(2 / 6f, 1));
        uvs.Add(new Vector2(3 / 6f, 1));
        


        //左
        verts.Add(new Vector3(0, 0, 0));
        verts.Add(new Vector3(0, 0, 1));
        verts.Add(new Vector3(0, 1, 1));
        verts.Add(new Vector3(0, 1, 0));

        uvs.Add(new Vector2(4 / 6f, 0));
        uvs.Add(new Vector2(3 / 6f, 0));
        uvs.Add(new Vector2(3 / 6f, 1));
        uvs.Add(new Vector2(4 / 6f, 1));
       




        for ( int i=0; i<=20;i+=4)
        {
            indices.Add(0+i); indices.Add(1 + i); indices.Add(2 + i);
            indices.Add(0 + i); indices.Add(2 + i); indices.Add(3 + i );
        }
      

    }
}
