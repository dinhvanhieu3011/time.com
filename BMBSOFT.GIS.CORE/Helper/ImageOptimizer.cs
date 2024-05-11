using ImageMagick;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BMBSOFT.GIS.CORE.Helper
{
    public static class ImageOptimization
    {
        public static async Task<string> CompressImage(string fileName, string fileFullPath, string outputDirectory)
        {
            string outputFile = Path.Combine(outputDirectory, fileName);
            var snakewareLogo = new FileInfo(outputFile);
            File.Copy(fileFullPath, snakewareLogo.FullName, true);

            Console.WriteLine("Bytes before: " + snakewareLogo.Length);

            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(snakewareLogo);
            snakewareLogo.Refresh();
            Console.WriteLine("Bytes after:  " + snakewareLogo.Length);
            return fileFullPath;
        }

        public static async Task CompressImage(Stream file)
        {
            file.Position = 0;
            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(file);
        }

        //353:227; 576:324; 120:90;270:155; 376:226;376:426;376:400;351x285;776x496;185x119;776x479; 800x0

        public static async Task ResizeImage(string fileFullPath, string outputDirectory, bool ignoreAspectRatio, int width, int height)
        {
            // Read from file
            using (var image = new MagickImage(fileFullPath))
            {
                var size = new MagickGeometry(width,height);
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                // Normally an image will be resized to fit inside the specified size.
                size.IgnoreAspectRatio = ignoreAspectRatio;
                image.Resize(size);

                // Save the result
                image.Write(outputDirectory + $"Snakeware.{width}x{height}.png");
            }
        }

        public static async Task<string> ResizeImage(IHostEnvironment _env, string filePath, string outputDirectory, bool ignoreAspectRatio, int width, int height)
        {
            var fileFullPath = Path.Combine(_env.ContentRootPath, filePath);
            using (var image = new MagickImage(fileFullPath))
            {
                string fileName = Path.GetFileName(fileFullPath);
                string newFile = Path.Combine(_env.ContentRootPath, outputDirectory, fileName);
                var size = new MagickGeometry(width, height);
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                // Normally an image will be resized to fit inside the specified size.
                size.IgnoreAspectRatio = ignoreAspectRatio;
                image.Resize(size);

                // Save the result
                image.Write(newFile);
                return Path.Combine(outputDirectory, fileName);
            }
        }
    }
}
