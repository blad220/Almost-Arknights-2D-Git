using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDescription : MonoBehaviour
{
    //Material material;
    public enum TypeTile { HighStandableTyle, LowStandableTyle, NoneStandableTyle, FireTyle }
    public TypeTile typeTile;

    public Transform standByPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnValidate()
    {
        //Material materialTemp = GetComponent<Renderer>().material;
        //if(materialTemp != null)
        //{
        //    material = materialTemp;
        //}
        //else
        //{
        //    materialTemp = material;
        //}

        //if(typeTile != TypeTile.DefultTyle)
        //{

        //}
    }

}
