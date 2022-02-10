using UnityEngine;

namespace Graph.Structs
{
    [SerializeField]
    public struct TextInfo
    {
        public TextInfo(string text, float xPos, float yPos, float textSize, int rotation,  Vector2 size, Color color)
        {
            Text = text;
            XPos = xPos;
            YPos = yPos;
            TextSize = textSize;
            Rotation = rotation;
            Size = size;
            Color = color;
        }

        public string Text;
        public float XPos;
        public float YPos; // y position on the graph
        public float TextSize;
        public int Rotation;
        public Vector2 Size;
        public Color Color;
    }
}
