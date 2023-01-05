using Enums;
using UnityEngine;

namespace Map
{
    public class Tile
    {
        public Vector2Int Position { get; }
        public TileType TileType { get; }
        public bool OnBorder { get; }

        public Tile(int row, int column, TileType tileType, bool onBorder)
        {
            TileType = tileType;
            Position = new Vector2Int(row, column);
            OnBorder = onBorder;
        }
    }
}