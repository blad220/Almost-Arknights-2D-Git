﻿using MainController.Level.Data;
using MainController.Map.Tile;
using MainController.Operator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MainController.Level
{
    [RequireComponent(typeof(MeshFilter))]
    public class LevelDisplay : MonoBehaviour
    {
        [Header("Main settings")]

        public LevelCreateData levelData;
        public enum SetupMap { OneMesh, MoreCubes }
        public SetupMap setupMap;
        public GameObject objectTile;
        public levelMaterialClass levelMaterial = new levelMaterialClass();
        public OperatorData[] operatorsInGame;

        public string description;
        public int cost;
        public Image levelImage;
        public int[] rewards;
        public int EnemyBrokeThrough;

        [Space(10)]
        [Header("System information")]
        public Vector3[] vertices;
        public int[] triangles;

        [Space(10)]
        public levelList[] levelLists;
        public List<GameObject> cubeList;
        public GameObject cubes;

        private Mesh mesh;

        public void Start()
        {
            //Refresh();
            if(EnemyBrokeThrough <= 0)
            {
                EnemyBrokeThrough = 1;
            }
            MainController.SetBrokeThroughEnemyStart(EnemyBrokeThrough);
        }
        public void Refresh()
        {
        #if UNITY_EDITOR

            if (levelData != null)
            {
                vertices = new Vector3[0];
                triangles = new int[0];

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
                    mesh = new Mesh();

                    GetComponent<MeshFilter>().mesh = mesh;
                    foreach (GameObject cube in cubeList)
                    {

                        #if UNITY_EDITOR
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(cube);
                        };
                        #endif
                    }
                    cubeList.Clear();

                    for (int k = 0; k < levelLists.Length; k++)
                    {
                        for (int n = 0; n < levelLists[k].levelType.Length; n++)
                        {
                            AddToMesh(CreateCube(k, n, 1, levelLists[k].levelType[n]));
                        }
                    }

                    UpdateMesh();
                }
                if (setupMap == SetupMap.MoreCubes)
                {

                    DestroyImmediate(cubes);
                    cubeList.Clear();

                    cubes = new GameObject("Cubes");
                    cubes.transform.SetParent(transform);
                    cubes.transform.localPosition = Vector3.zero;
                    cubes.transform.localScale = new Vector3(1f, 1f, 1f);

                    for (int k = 0; k < levelData.getMapDataCount(); k++)
                    {
                        for (int n = 0; n < levelData.getMapDataCount(k); n++)
                        {
                            GameObject cube;
                            Material[] cubeMaterial = new Material[3];
                            if (objectTile == null)
                            {
                                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                cubeMaterial[0] = cube.GetComponent<MeshRenderer>().material;
                            }
                            else
                            {
                                cube = GameObject.Instantiate(objectTile);
                                cubeMaterial = cube.GetComponent<MeshRenderer>().sharedMaterials;
                            }

                            cubeList.Add(cube);

                            cube.transform.SetParent(cubes.transform);

                            TileDescription tileDescription;

                            if (cube.TryGetComponent(out tileDescription))
                            {

                            }
                            else
                            {
                                tileDescription = cube.AddComponent<TileDescription>();
                            }

                            float curElement = levelData.getMapDataElementbyIndex(k, n);

                            cube.transform.localPosition = new Vector3(n, 0f, k);
                            cube.transform.localScale = new Vector3(1, 1f, 1);

                            cube.isStatic = true;

                            if (curElement == 1)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.LowStandableTyle);
                                cubeMaterial = levelMaterial.groundTile;

                                cube.transform.localPosition = new Vector3(n, 0f, k);
                                cube.transform.localScale = new Vector3(1, 1f, 1);
                            }
                            if (curElement == 2)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.HighStandableTyle);
                                cubeMaterial = levelMaterial.highTile;

                                cube.transform.localPosition = new Vector3(n, 0f, k);
                                cube.transform.localScale = new Vector3(1, 10f, 1);
                            }
                            if (curElement == -1)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.LowStandableTyle);
                                cubeMaterial = levelMaterial.groundTileNoStandable;

                                cube.transform.localPosition = new Vector3(n, 0f, k);
                                cube.transform.localScale = new Vector3(1, 1f, 1);
                            }
                            if (curElement == -2)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.HighStandableTyle);
                                cubeMaterial = levelMaterial.highTileNoStandable;

                                cube.transform.localPosition = new Vector3(n, 0f, k);
                                cube.transform.localScale = new Vector3(1, 10f, 1);
                            }
                            if (curElement == 4)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.NoneStandableTyle);
                                cubeMaterial = levelMaterial.defaultMaterial;

                                cube.transform.localPosition = new Vector3(n, 0f, k);
                                cube.transform.localScale = new Vector3(1, 30f, 1);
                            }
                            if (curElement != 1 && curElement != 2)
                            {
                                tileDescription.SetStartTypeTile(TileDescription.TypeTile.NoneStandableTyle);
                            }

                            cube.name = $"Cube {n}x{k}";

                            if (cubeMaterial == null) cubeMaterial = levelMaterial.defaultMaterial;

                            cube.GetComponent<MeshRenderer>().sharedMaterials = cubeMaterial;
                        }
                    }
                }

            }
        #endif
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

        }

        private MeshShape CreateGrid(int cellSizeX, int cellSizeZ, float[][] heightMap)
        {
            int gridSizeX = heightMap.Length;
            int gridSizeZ = heightMap[0].Length;

            Vector3[,] verticesGrid = new Vector3[cellSizeX, cellSizeZ];

            for (int n = 0; n < heightMap.Length; n++)
            {
                for (int k = 0; k < heightMap[n].Length; k++)
                {
                    verticesGrid[n, k] = new Vector3();
                }
            }

            MeshShape shapeGrid;
            shapeGrid.vertices = new Vector3[] { new Vector3(0, 0, 0), };
            shapeGrid.triangles = new int[] { 0 };
            return shapeGrid;
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
                ////Left
                //4, 5, 0,
                //5, 1, 0,
                ////Right
                //2, 7, 6,
                //2, 3, 7,
                ////Back
                //7, 3, 5,
                //3, 1, 5,
                ////Front
                //6, 0, 2,
                //6, 4, 0,
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

    }
    [System.Serializable]
    public class levelMaterialClass
    {
        public Material[] defaultMaterial;

        public Material[] groundTile;
        public Material[] groundTileNoStandable;
        public Material[] highTile;
        public Material[] highTileNoStandable;
    }
    [Serializable]
    public class levelList
    {
        public float[] levelType;
    }
}