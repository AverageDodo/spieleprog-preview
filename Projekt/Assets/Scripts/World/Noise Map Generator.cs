using UnityEngine;

namespace World
{
    /// <summary>
    /// This class is responsible for generating a noise map for randomized terrain generation.
    /// </summary>
    public class NoiseMapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Generates a 2D noise map using Perlin noise.
        /// The noise map is a 2D array of floats, with dimensions specified by the width and depth parameters.
        /// The roughness parameter controls the scale of the noise.
        /// Each value in the noise map is determined by the Perlin noise at the corresponding x and y coordinates,
        /// scaled by the roughness.
        /// </summary>
        /// <param name="width">The width of the noise map.</param>
        /// <param name="depth">The depth of the noise map.</param>
        /// <param name="roughness">The roughness of the noise map. Defaults to 0.3f.</param>
        /// <returns>A 2D array of floats representing the noise map.</returns>
        public static float[,] Generate(int width, int depth, float roughness = 0.3f)
        {
            var noiseMap = new float[width, depth];

            for (int zIndex = 0; zIndex < depth; zIndex++)
            {
                for (int xIndex = 0; xIndex < width; xIndex++)
                {
                    var x = xIndex * roughness;
                    var y = zIndex * roughness;

                    noiseMap[zIndex, xIndex] = Mathf.PerlinNoise(x, y);
                }
            }

            return noiseMap;
        }
    }
}
