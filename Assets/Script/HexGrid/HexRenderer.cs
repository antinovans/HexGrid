using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

struct HexSegment
{
    public List<Vector3> vertices {get; set;}
    public List<int> triangles {get; set;}
    public List<Vector2> uvs{get; set;}

    //constructor for triangle faces
    public HexSegment(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }

}
public enum MaterialType
{
    Normal,
    Highlight,
    Available,
    Occupied
}
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
// [ExecuteInEditMode]
public class HexRenderer : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter m_filter;
    private MeshRenderer m_renderer;
    private List<HexSegment> hex_segs;
    [Header("Hex attribute")]
    [SerializeField]
    public float innerRad;
    [SerializeField]
    public float outerRad;
    [SerializeField]
    public float hexHeight;
    public Material normal_mat;
    public Material hightlight_mat;
    public Material available_mat;
    public Material occupied_mat;

    private Material prevMat;

    private void Awake() {
        m_filter = GetComponent<MeshFilter>();
        m_renderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "HexMesh";

        Assert.IsNotNull(mesh);
        
        m_filter.mesh = mesh;
        m_renderer.material = normal_mat;
    }    
    

    private void OnValidate() {
        // if(Application.isPlaying)
        // {
        //     DrawHex();
        //     CombineHex();
        // }
        DrawHex();
        CombineHex();
        
    }
    private void OnEnable() {
        DrawHex();
        CombineHex();
    }

    public void DrawHex()
    {
        hex_segs = new List<HexSegment>();
        //top faces
        for(int segIdx = 0; segIdx < 6; segIdx++)
        {
            hex_segs.Add(CreateHexSeg(outerRad, innerRad, hexHeight/2, hexHeight/2, segIdx, false));
        }
        //bottom faces
        for(int segIdx = 0; segIdx < 6; segIdx++)
        {
            hex_segs.Add(CreateHexSeg(outerRad, innerRad, -hexHeight/2, -hexHeight/2, segIdx, true));
        }

        //glue the middle part together
        //outer part
        for(int segIdx = 0; segIdx < 6; segIdx++)
        {
            hex_segs.Add(CreateHexSeg(outerRad, outerRad, hexHeight/2, -hexHeight/2, segIdx, true));
        }
        //inner part
        for(int segIdx = 0; segIdx < 6; segIdx++)
        {
            hex_segs.Add(CreateHexSeg(innerRad, innerRad, hexHeight/2, -hexHeight/2, segIdx, false));
        }
    }
    public void CombineHex()
    {
        List<Vector3> combined_verts = new List<Vector3>();
        List<int> combined_tris = new List<int>();
        List<Vector2> combined_uvs = new List<Vector2>();

        for(int i = 0; i < hex_segs.Count; i++)
        {
            combined_verts.AddRange(hex_segs[i].vertices);
            combined_uvs.AddRange(hex_segs[i].uvs);
            int offset = i*4;
            foreach(int triangle in hex_segs[i].triangles)
                combined_tris.Add(triangle + offset);
        }

        mesh.vertices = combined_verts.ToArray();
        mesh.uv = combined_uvs.ToArray();
        mesh.triangles = combined_tris.ToArray();
        mesh.RecalculateNormals();

    }
    private HexSegment CreateHexSeg(float outerRad, float innerRad,
                                    float outerHeight, float innerHeight,
                                    int index, bool reversed)
    {
        //x(Ao)  
        //
        //           x(Bo)
        //
        //
        //x(Ai)
        //     x(Bi)
        Vector3 pointAi = GetPointPosOnHex(innerRad, innerHeight, index);
        Vector3 pointBi = GetPointPosOnHex(innerRad, innerHeight, (index < 5) ? index + 1 : 0);
        Vector3 pointBo = GetPointPosOnHex(outerRad, outerHeight, (index < 5) ? index + 1 : 0);
        Vector3 pointAo = GetPointPosOnHex(outerRad, outerHeight, index);
        
        List<Vector3> vertices = new List<Vector3>() {pointAi, pointBi, pointBo, pointAo};
        List<int> triangles = new List<int>() {0, 1, 2, 0, 2, 3};
        List<Vector2> uvs = new List<Vector2>() {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};

        if(reversed)
            vertices.Reverse();

        return new HexSegment(vertices, triangles, uvs);
    }
    private Vector3 GetPointPosOnHex(float hexRad, float height, int index)
    {
        float angleInDeg = 60 * index;
        float angleInRad = Mathf.PI /180.0f * angleInDeg;
        return new Vector3(hexRad * Mathf.Cos(angleInRad), height, hexRad * Mathf.Sin(angleInRad));
    }
    public void SwitchMaterial(MaterialType type)
    {
        switch (type)
        {
            case MaterialType.Normal:
                m_renderer.material = normal_mat;
                prevMat = normal_mat;
                break;
            case MaterialType.Highlight:
                m_renderer.material = hightlight_mat;
                break;
            case MaterialType.Available:
                m_renderer.material = available_mat;
                prevMat = available_mat;
                break;
            case MaterialType.Occupied:
                m_renderer.material = occupied_mat;
                prevMat = occupied_mat;
                break;
            default:
                return;
        }
    }
    public void RevertMaterial()
    {
        m_renderer.material = prevMat;
    }
}
