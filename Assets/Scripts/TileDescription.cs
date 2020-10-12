using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDescription : MonoBehaviour
{
    //Material material;
    public enum TypeTile { HighStandableTyle, LowStandableTyle, NoneStandableTyle, FireTyle }
    public TypeTile typeTile;
    public Transform standByPosition;

    public TypeTile typeTileOnStartGame;

    // Start is called before the first frame update
    void Awake()
    {
        typeTileOnStartGame = typeTile;
    }
    public void Reset()
    {
        typeTile = typeTileOnStartGame;
    }
    public void SetStartTypeTile(TypeTile _typeTile)
    {
        typeTile = _typeTile;
        typeTileOnStartGame = _typeTile;
    }
}