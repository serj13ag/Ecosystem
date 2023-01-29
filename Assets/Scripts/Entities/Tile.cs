using DataTypes;

namespace Entities
{
    public class Tile
    {
        public Point Position { get; }
        public float Height { get; }
        public bool OnBorder { get; }

        public bool SuitableForPlants { get; }
        public bool Walkable { get; }
        public bool HasTree { get; }

        public bool IsShallow { get; }

        public Tile(Point position, float height, bool onBorder, bool suitableForPlants, bool isWalkable, bool hasTree)
        {
            Position = position;
            Height = height;
            OnBorder = onBorder;

            SuitableForPlants = suitableForPlants;
            Walkable = isWalkable;
            HasTree = hasTree;

            IsShallow = height > Constants.ShallowLowerHeight && height < Constants.ShallowHigherHeight;
        }
    }
}