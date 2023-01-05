using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class TreeService
    {
        public HashSet<Vector2Int> TreePositions { get; }

        public TreeService()
        {
            TreePositions = new HashSet<Vector2Int>();
        }

        public void GenerateTrees(float treesPercentage, Vector2Int[] mapLandTilePositions)
        {
            TreePositions.Clear();

            CreateTrees(treesPercentage, mapLandTilePositions);
        }

        private void CreateTrees(float treesPercentageValue, Vector2Int[] landTilesPositions)
        {
            foreach (Vector2Int landTilesPosition in landTilesPositions)
            {
                if (Random.Range(0, 101) > treesPercentageValue)
                {
                    continue;
                }

                TreePositions.Add(new Vector2Int(landTilesPosition.x, landTilesPosition.y));
            }
        }
    }
}