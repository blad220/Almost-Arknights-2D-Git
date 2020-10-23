using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MainController.Map
{

    [RequireComponent(typeof(MeshFilter))]
    public class MapGenerator : MonoBehaviour
    {
        public enum SetupMap { OneMesh, MoreCubes }

        public SetupMap setupMap;

        public Material[] levelMaterial = new Material[9];

        private Mesh mesh;
        public Vector3[] vertices;
        public int[] triangles;

        struct Tile
        {

        }
        struct MeshShape
        {
            public Vector3[] vertices;
            public int[] triangles;
        }
        void AddToMesh(MeshShape addMesh)
        {
            for (int i = 0; i < addMesh.triangles.Length; i++)
            {
                addMesh.triangles[i] += vertices.Length;
            }
            vertices = vertices.Concat(addMesh.vertices).ToArray();
            triangles = triangles.Concat(addMesh.triangles).ToArray();
        }
        MeshShape CreateCube(float x, float y, float size, float height)
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
        void UpdateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
        void UpdateMesh(MeshShape meshShape)
        {
            mesh.Clear();
            mesh.vertices = meshShape.vertices;
            mesh.triangles = meshShape.triangles;
            mesh.RecalculateNormals();
        }

    }
}