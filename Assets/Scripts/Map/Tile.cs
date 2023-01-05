using UnityEngine;

namespace Map
{
    public class Tile
    {
        public Vector2Int Position { get; }
        public TileType TileType { get; }

        public Tile(int row, int column, TileType tileType)
        {
            TileType = tileType;
            Position = new Vector2Int(row, column);
        }
    }
}