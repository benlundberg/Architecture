using System;

namespace Architecture
{
    public static class ImageService
    {
        public static string GetRandomImage()
        {
            Random random = new Random();

            string baseUrl = "http://clarityapplication.com/dev/images/";

            int picture = random.Next(1, 13);

            return baseUrl + picture.ToString() + ".jpg";
        }
    }
}
