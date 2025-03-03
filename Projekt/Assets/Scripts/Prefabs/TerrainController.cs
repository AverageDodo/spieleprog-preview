using UnityEngine;
using World;

namespace Prefabs
{
    /// <summary>
    /// This class is responsible for controlling the randomized terrain in Level 3.
    /// </summary>
    public class TerrainController : MonoBehaviour
    {
        /// <summary>
        /// The size of the terrain in the x-axis.
        /// </summary>
        public int xSize;

        /// <summary>
        /// The maximum height of the terrain.
        /// </summary>
        public int ySize;

        /// <summary>
        /// The size of the terrain in the z-axis.
        /// </summary>
        public int zSize;

        /// <summary>
        /// Initializes the terrain and its collider at the start of the game.
        /// The terrain data is generated and set for both the terrain and its collider.
        /// </summary>
        private void Start()
        {
            var terrain = GetComponent<Terrain>();
            terrain.terrainData = GenerateTerrainData(terrain.terrainData);
            GetComponent<TerrainCollider>().terrainData = terrain.terrainData;
        }

        /// <summary>
        /// Generates the terrain data based on the specified sizes.
        /// The heights of the terrain are set using a noise map generated with the specified xSize and zSize.
        /// </summary>
        /// <param name="td">The TerrainData to modify.</param>
        /// <returns>The modified TerrainData.</returns>
        private TerrainData GenerateTerrainData(TerrainData td)
        {
            td.heightmapResolution = xSize + 1;
            td.size = new Vector3(xSize, ySize, zSize);
            td.SetHeights(0, 0, NoiseMapGenerator.Generate(xSize, zSize));

            return td;
        }
    }
}
