using Rage;
using System.CodeDom.Compiler;
using System.Drawing;
using System.IO;

namespace DLS.UI
{
    internal class Resources
    {
        public static Texture GetTextureFromResource(Bitmap resource)
        {
            using (var tempFiles = new TempFileCollection())
            {
                string filePath = tempFiles.AddExtension("png");

                resource.Save(filePath);
                if (File.Exists(filePath))
                {
                    return Game.CreateTextureFromFile(filePath);
                }
            }
            return null;
        }
    }
}
