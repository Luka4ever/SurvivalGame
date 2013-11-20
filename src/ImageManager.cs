using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SurvivalGame.src
{
    static class ImageManager
    {
        private static List<Image> images = new List<Image>();
        private static Dictionary<string, int> lookup = new Dictionary<string, int>();

        /// <summary>
        /// Returns the image associated with a specific value
        /// </summary>
        /// <param name="index">The integer returned from the RegisterImage function</param>
        /// <returns>The Image</returns>
        public static Image GetImage(int index) {
            return images.ElementAt(index);
        }

        /// <summary>
        /// Registers and loads the image if it isn't registered at the time and returns the index to reference it.
        /// This function can only be called before the close function
        /// </summary>
        /// <param name="path">The relative path to the image</param>
        /// <returns>The index to refrence the image with</returns>
        public static int RegisterImage(string path) {
            int index;
            if (!lookup.TryGetValue(path, out index)) {
                images.Add(Image.FromFile(@path));
                index = images.Count() - 1;
                lookup.Add(path, index);
            }
            return index;
        }

        /// <summary>
        /// Closes the Manager for loading more images to free up memory
        /// </summary>
        public static void Close() {
            Console.WriteLine("Finished image loading");
            lookup = null;
        }
    }
}
