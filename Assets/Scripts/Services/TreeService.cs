using System.Collections.Generic;
using DataTypes;
using Entities;
using Enums;
using UnityEngine;
using Tree = Entities.Tree;

namespace Services
{
    public class TreeService
    {
        private readonly RandomService _randomService;

        public List<Tree> Trees { get; }

        public TreeService(RandomService randomService)
        {
            _randomService = randomService;

            Trees = new List<Tree>();
        }

        public void GenerateTrees(IEnumerable<Tile> mapTiles)
        {
            Trees.Clear();

            foreach (Tile mapTile in mapTiles)
            {
                if (!mapTile.HasTree)
                {
                    continue;
                }

                Point treePosition = mapTile.Position;

                TreeType type = GetTreeType(treePosition);
                Vector3 position = new Vector3(treePosition.X, Constants.TerrainPositionY, treePosition.Y);
                int angleRotation = GetRandomRotation(treePosition);
                Vector3 scale = GetRandomScale(treePosition);

                Tree tree = new Tree(type, position, angleRotation, scale);

                Trees.Add(tree);
            }
        }

        private TreeType GetTreeType(Point treePosition)
        {
            return (TreeType)_randomService.RandomFunction(treePosition, Constants.TreeTypes.Length);
        }

        private int GetRandomRotation(Point treePosition)
        {
            return _randomService.RandomFunction(treePosition, Constants.TreeMeshMaxRotationAngle);
        }

        private Vector3 GetRandomScale(Point treePosition)
        {
            float randomScale = _randomService.RandomFunction(treePosition, Constants.TreeMeshScaleMinPercentage,
                Constants.TreeMeshScaleMaxPercentage) / 100f;

            return new Vector3(randomScale, randomScale, randomScale);
        }
    }
}