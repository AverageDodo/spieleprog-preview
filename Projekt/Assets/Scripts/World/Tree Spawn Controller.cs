using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    /// <summary>
    /// The TreeSpawnController class inherits from the SpawnController class.
    /// This class is responsible for the spawning of tree prefabs in the game world.
    /// </summary>
    internal class TreeSpawnController : SpawnController
    {
        protected override void Start()
        {
            RandomPrefabRotation = new Vector3(0, Random.Range(0, 90), 0);
            VerticalSpawnOffset = -1f;

            base.Start();
        }
    }
}
