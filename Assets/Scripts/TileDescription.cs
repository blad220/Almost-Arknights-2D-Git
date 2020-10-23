using UnityEngine;

namespace MainController.Map.Tile
{
    public class TileDescription : MonoBehaviour
    {
        public enum TypeTile { HighStandableTyle, LowStandableTyle, NoneStandableTyle, FireTyle }
        public TypeTile typeTile;
        public Transform standByPosition;

        public TypeTile typeTileOnStartGame;

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
}