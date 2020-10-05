using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    public enum SetupMap { OneMesh, MoreCubes }

    public SetupMap setupMap;

    public Material[] levelMaterial = new Material[9];

    private Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[0];
        triangles = new int[0];
        //CreateShape();
        //MeshShape meshCube = CreateCube(0, 0, 1, 1);
        float[][] mass = new float[7][];
        const int col = 13;
        mass[0] = new float[col] { 4, 4, 2, 2, 2, 2, 0, 2, 2, 2, 4, 4, 4 };
        mass[1] = new float[col] { -4, 3, 2, 2, 2, 1, 0, 1, 1, 2, 2, -4, -4 };
        mass[2] = new float[col] { -4, -4, 2, 2, 1, 1, 0, 1, 1, 2, 2, -4, -4 };
        mass[3] = new float[col] { -4, -4, 0, 0, 0, 0, 0, 0, 0, 0, 0, -4, -4 };
        mass[4] = new float[col] { -4, -4, 3, 2, 2, 1, 1, 0, 1, 1, 2, 3, 3 };
        mass[5] = new float[col] { 4, 4, 2, 2, 2, 1, 1, 0, 1, 2, 2, 3, 3 };
        mass[6] = new float[col] { 4, 4, 2, 2, 2, 2, 2, 0, 2, 2, 2, 3, 3 };

        if (setupMap == SetupMap.OneMesh)
        {
            for (int k = 0; k < mass.Length; k++)
            {
                for (int n = 0; n < mass[k].Length; n++)
                {
                    AddToMesh(CreateCube(k, n, 1, mass[k][n]));
                }
            }

            UpdateMesh();
        }
        if (setupMap == SetupMap.MoreCubes)
        {
            for (int k = 0; k < mass.Length; k++)
            {
                for (int n = 0; n < mass[k].Length; n++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = new Vector3(k, mass[k][n], n);

                    if (mass[k][n] <= 1)
                    {
                        cube.transform.position = new Vector3(k, mass[k][n] - ((mass[k][n] + 5) / 2), n);
                        cube.transform.localScale = new Vector3(1, mass[k][n] + 5, 1);
                    }
                    else
                    {
                        cube.transform.position = new Vector3(k, -2f, n);
                        cube.transform.localScale = new Vector3(1, 6f + mass[k][n] / 10, 1);
                    }
                    cube.GetComponent<MeshRenderer>().material = levelMaterial[(int)mass[k][n] + 4];

                    cube.transform.SetParent(transform);
                }
            }
        }
    }

    //void CreateShape()
    //{
    //float verticalsSize = 1f;
    //vertices = new Vector3[]
    //{
    //    new Vector3(0,0,0),
    //    new Vector3(0,verticalsSize,0),
    //    new Vector3(verticalsSize,0,0),
    //    new Vector3(verticalsSize,verticalsSize,0),
    //};

    //    triangles = new int[]
    //    {
    //        0, 1, 2,
    //        1, 3, 2,
    //    };
    //}
    private struct Tile
    {
    }

    private struct MeshShape
    {
        public Vector3[] vertices;
        public int[] triangles;
    }

    private void AddToMesh(MeshShape addMesh)
    {
        for (int i = 0; i < addMesh.triangles.Length; i++)
        {
            addMesh.triangles[i] += vertices.Length;
        }
        vertices = vertices.Concat(addMesh.vertices).ToArray();
        triangles = triangles.Concat(addMesh.triangles).ToArray();

        //int addVerticesN = 0;
        //for(int i = mesh.vertices.Length; i<mesh.vertices.Length + addMech.vertices.Length; i++)
        //{
        //    mesh.vertices[i] = addMech.vertices[addVerticesN];
        //    addVerticesN++;
        //}
        //int addTrianglesN = 0;
        //for (int i = mesh.triangles.Length; i < mesh.triangles.Length + addMech.triangles.Length; i++)
        //{
        //    mesh.vertices[i] = addMech.vertices[addTrianglesN];
        //    addTrianglesN++;
        //}
    }

    private MeshShape CreateCube(float x, float y, float size, float height)
    {
        float x1 = x - (size * 0.5f);
        float x2 = x + (size * 0.5f);
        float y1 = y - (size * 0.5f);
        float y2 = y + (size * 0.5f);

        Vector3[] verticesCube = new Vector3[]
        {
            //Top vertices
            new Vector3(x1, height, y1),
            new Vector3(x1, height, y2),
            new Vector3(x2, height, y1),
            new Vector3(x2, height, y2),

            //Bottom vertices
            new Vector3(x1, 0, y1),
            new Vector3(x1, 0, y2),
            new Vector3(x2, 0, y1),
            new Vector3(x2, 0, y2),
        };

        int[] trianglesCube = new int[]
        {
            //Top
            0, 1, 2,
            1, 3, 2,
            //Left
            4, 5, 0,
            5, 1, 0,
            //Right
            2, 7, 6,
            2, 3, 7,
            //Back
            7, 3, 5,
            3, 1, 5,
            //Front
            6, 0, 2,
            6, 4, 0,
        };
        MeshShape meshCube;
        meshCube.vertices = verticesCube;
        meshCube.triangles = trianglesCube;

        return meshCube;
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void UpdateMesh(MeshShape meshShape)
    {
        mesh.Clear();
        mesh.vertices = meshShape.vertices;
        mesh.triangles = meshShape.triangles;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(setupMap);
    }
}