using UnityEngine;

namespace Map
{
    public class Tile
    {
        public Vector2Int Position { get; }
        public float Height { get; }
        public bool OnBorder { get; }

        public bool SuitableForPlants { get; }
        public bool Walkable { get; }
        public bool IsShallow { get; }

        public Tile(int positionX, int positionY, float height, bool onBorder, bool suitableForPlants, bool isWalkable)
        {
            Position = new Vector2Int(positionX, positionY);
            Height = height;
            OnBorder = onBorder;

            SuitableForPlants = suitableForPlants;
            Walkable = isWalkable;
            IsShallow = height > Constants.ShallowLowerHeight && height < Constants.ShallowHigherHeight;
        }
    }
}