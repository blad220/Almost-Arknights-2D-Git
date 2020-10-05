using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDescription : MonoBehaviour
{
    //Material material;
    public enum TypeTile { HighStandableTyle, LowStandableTyle, NoneStandableTyle, FireTyle }

    public TypeTile typeTile;

    private TypeTile typeTileOnStartGame;

    public Transform standByPosition;

    //пустые функции старта и апдейта
    // Start is called before the first frame update
    private void Awake()
    {
        typeTileOnStartGame = typeTile;
    }

    // Update is called once per frame
    private void Update()
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