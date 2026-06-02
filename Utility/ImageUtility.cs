using System.Drawing;

namespace Utility
{
    public class ImageUtility
    {
        public static void ResizeImage(string FileName, string NewFileName, int MaxWidth, int MaxHeight)
        {

            Bitmap ActualImage = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(FileName);
            Bitmap OrigImg = (System.Drawing.Bitmap)ActualImage.Clone();
            Size ResizedDimensions = GetDimensions(MaxWidth, MaxHeight, ref OrigImg);
            Bitmap NewImg = new Bitmap(OrigImg, ResizedDimensions);

            //saveFile(NewImg, NewFileName, NewFileExtension);
            NewImg.Save(NewFileName);

            ActualImage.Dispose();
            OrigImg.Dispose();
            NewImg.Dispose();
        }
        public static Size GetDimensions(int MaxWidth, int MaxHeight, ref Bitmap Img)
        {
            int Height; int Width; float Multiplier;
            Height = Img.Height; Width = Img.Width;

            if (Height <= MaxHeight && Width <= MaxWidth)
                return new Size(Width, Height);

            Multiplier = (float)((float)MaxWidth / (float)Width);

            if ((Height * Multiplier) <= MaxHeight)
            {
                Height = (int)(Height * Multiplier);
                return new Size(MaxWidth, Height);
            }

            Multiplier = (float)MaxHeight / (float)Height;
            Width = (int)(Width * Multiplier);

            return new Size(Width, MaxHeight);

        }
    }
}