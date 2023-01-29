using Enums;
using UnityEngine;

namespace Entities
{
    public class Tree
    {
        public TreeType Type { get; }
        public Vector3 Position { get; }
        public int AngleRotation { get; }
        public Vector3 Scale { get; }

        public Tree(TreeType type, Vector3 position, int angleRotation, Vector3 scale)
        {
            Type = type;
            Position = position;
            AngleRotation = angleRotation;
            Scale = scale;
        }
    }
}