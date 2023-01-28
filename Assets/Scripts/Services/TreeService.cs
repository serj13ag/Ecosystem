using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Services
{
    public class TreeService
    {
        public HashSet<Point> TreePositions { get; }

        public TreeService()
        {
            TreePositions = new HashSet<Point>();
        }

        public void GenerateTrees(float treesPercentage, IEnumerable<Point> mapLandTilePositions)
        {
            TreePositions.Clear();

            CreateTrees(treesPercentage, mapLandTilePositions);
        }

        private void CreateTrees(float treesPercentageValue, IEnumerable<Point> landTilesPositions)
        {
            foreach (Point landTilesPosition in landTilesPositions)
            {
                if (Random.Range(0, 101) >= treesPercentageValue)
                {
                    continue;
                }

                TreePositions.Add(landTilesPosition);
            }
        }
    }
}