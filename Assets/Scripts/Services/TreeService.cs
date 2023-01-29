using System.Collections.Generic;
using Data;

namespace Services
{
    public class TreeService
    {
        public HashSet<Point> TreePositions { get; private set; }

        public TreeService()
        {
            TreePositions = new HashSet<Point>();
        }

        public void GenerateTrees(IEnumerable<Point> mapLandTilePositions)
        {
            TreePositions.Clear();

            TreePositions.UnionWith(mapLandTilePositions);
        }
    }
}