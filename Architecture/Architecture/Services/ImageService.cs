using System;

namespace Architecture
{
    public static class ImageService
    {
        public static string GetRandomImage()
        {
            Random random = new Random();
            
            //string baseUrl = "https://architectureappimages.blob.core.windows.net/imagecontainer/";

            string baseUrl = "https://architectureappimages.blob.core.windows.net/imagecontainer/";
            
            int picture = random.Next(1, 13);

            return baseUrl + picture.ToString() + ".jpg";
        }
    }
}
