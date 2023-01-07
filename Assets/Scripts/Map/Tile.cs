using Enums;
using UnityEngine;

namespace Map
{
    public class Tile
    {
        public Vector2Int Position { get; }
        public float Height { get; }
        public TileType Type { get; }
        public bool OnBorder { get; }

        public Tile(int positionX, int positionY, float height, TileType type, bool onBorder)
        {
            Position = new Vector2Int(positionX, positionY);
            Height = height;
            Type = type;
            OnBorder = onBorder;
        }
    }
}