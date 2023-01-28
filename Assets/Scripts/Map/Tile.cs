using Data;

namespace Map
{
    public class Tile
    {
        public Point Position { get; }
        public float Height { get; }
        public bool OnBorder { get; }

        public bool SuitableForPlants { get; }
        public bool Walkable { get; }
        public bool IsShallow { get; }

        public Tile(Point position, float height, bool onBorder, bool suitableForPlants, bool isWalkable)
        {
            Position = position;
            Height = height;
            OnBorder = onBorder;

            SuitableForPlants = suitableForPlants;
            Walkable = isWalkable;
            IsShallow = height > Constants.ShallowLowerHeight && height < Constants.ShallowHigherHeight;
        }
    }
}