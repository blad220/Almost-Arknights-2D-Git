using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
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
                        beforeColorLow.Add(cube.GetComponent<Renderer>().materials[1].color);
                        cube.GetComponent<Renderer>().materials[1].color = highlightColor;
                        changeGameobjectLow.Add(cube);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < changeGameobjectLow.Count; i++)
            {
                changeGameobjectLow[i].GetComponent<Renderer>().materials[1].color = (Color)beforeColorLow[i];
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
                        beforeColorHigh.Add(cube.GetComponent<Renderer>().materials[1].color);
                        cube.GetComponent<Renderer>().materials[1].color = highlightColor;
                        changeGameobjectHigh.Add(cube);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < changeGameobjectHigh.Count; i++)
            {
                changeGameobjectHigh[i].GetComponent<Renderer>().materials[1].color = (Color)beforeColorHigh[i];
            }

            changeGameobjectHigh.Clear();
            beforeColorHigh.Clear();
        }


        //changeGameobject = new List<GameObject>();
        //beforeColor = new ArrayList();
        //if (showLowGround ^ showHighGround)
        //{
        //    //beforeColor = new ArrayList();
        //    //changeGameobject = new List<GameObject>();
        //    int n = 0;
        //    foreach (GameObject cube in GetComponent<LevelDisplay>().cubeList)
        //    {
        //        if (cube.GetComponent<TileDescription>().typeTile == TileDescription.TypeTile.LowStandableTyle && showLowGround)
        //        {
        //            beforeColorLow.Add(cube.GetComponent<Renderer>().material.color);
        //            cube.GetComponent<Renderer>().material.color = highlightColor;
        //            changeGameobjectLow.Add(cube);

        //            n++;
        //        }
        //        if (cube.GetComponent<TileDescription>().typeTile == TileDescription.TypeTile.HighStandableTyle && showHighGround)
        //        {
        //            beforeColorHigh.Add(cube.GetComponent<Renderer>().material.color);
        //            cube.GetComponent<Renderer>().material.color = highlightColor;
        //            changeGameobjectHigh.Add(cube);
        //            n++;
        //        }
        //    }
            
        //} else 
        //if(showLowGround && showHighGround) {

        //}
        //else
        //{
        //    for(int i=0; i< changeGameobject.Count; i++)
        //    {
        //        changeGameobject[i].GetComponent<Renderer>().material.color = (Color)beforeColor[i];
        //    }

        //    changeGameobject.Clear();
        //    beforeColor.Clear();
        //    //beforeColor = new Color[0];
        //}
        
    }
}
