using Rage;
using System.Drawing;

namespace DLS.UI
{
    internal class Sprite
    {
        internal Sprite(Bitmap resource, Point position, Size size)
        {
            Position = position;
            Size = size;
            Texture = Resources.GetTextureFromResource(resource);
        }

        internal Sprite(Texture texture, Point position, Size size)
        {
            Position = position;
            Size = size;
            Texture = texture;
        }

        internal Point Position { get; set; }
        internal Size Size { get; set; }
        internal Texture Texture { get; set; }
    }
}
