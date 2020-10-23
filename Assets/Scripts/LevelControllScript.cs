using MainController.Level;
using MainController.Map.Tile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainController
{

    [RequireComponent(typeof(LevelDisplay))]
    public class LevelControllScript : MonoBehaviour
    {
        public Color highlightColor = Color.white;
        public bool showLowGround;
        public bool showHighGround;

        public List<GameObject> changeGameobjectLow = new List<GameObject>();
        public ArrayList beforeColorLow = new ArrayList();
        public List<GameObject> changeGameobjectHigh = new List<GameObject>();
        public ArrayList beforeColorHigh = new ArrayList();

        private void Update()
        {
            if (showLowGround)
            {
                if (changeGameobjectLow.Count == 0)
                {
                    foreach (GameObject cube in GetComponent<LevelDisplay>().cubeList)
                    {
                        if (cube.GetComponent<TileDescription>().typeTile == TileDescription.TypeTile.LowStandableTyle)
                        {
                            beforeColorLow.Add(cube.GetComponent<Renderer>().materials[1].GetColor("_BaseColor"));
                            cube.GetComponent<Renderer>().materials[1].SetColor("_BaseColor", highlightColor);
                            changeGameobjectLow.Add(cube);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < changeGameobjectLow.Count; i++)
                {
                    changeGameobjectLow[i].GetComponent<Renderer>().materials[1].SetColor("_BaseColor", (Color)beforeColorLow[i]);
                }

                changeGameobjectLow.Clear();
                beforeColorLow.Clear();
            }

            if (showHighGround)
            {
                if (changeGameobjectHigh.Count == 0)
                {
                    foreach (GameObject cube in GetComponent<LevelDisplay>().cubeList)
                    {
                        if (cube.GetComponent<TileDescription>().typeTile == TileDescription.TypeTile.HighStandableTyle)
                        {
                            beforeColorHigh.Add(cube.GetComponent<Renderer>().materials[1].GetColor("_BaseColor"));
                            //Debug.Log(cube.GetComponent<Renderer>().materials[1].GetColor("_BaseColor"));
                            cube.GetComponent<Renderer>().materials[1].SetColor("_BaseColor", highlightColor);
                            changeGameobjectHigh.Add(cube);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < changeGameobjectHigh.Count; i++)
                {
                    changeGameobjectHigh[i].GetComponent<Renderer>().materials[1].SetColor("_BaseColor", (Color)beforeColorHigh[i]);
                }

                changeGameobjectHigh.Clear();
                beforeColorHigh.Clear();
            }

        }
    }
}