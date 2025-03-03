using UnityEngine;

namespace World
{
    /// <summary>
    /// The RockSpawnController class inherits from the SpawnController class.
    /// This class is responsible for the spawning of rock prefabs in the game world.
    /// </summary>
    internal class RockSpawnController : SpawnController
    {
        protected override void Start()
        {
            RandomPrefabRotation = new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
            VerticalSpawnOffset = 0.01f;
            
            base.Start();
        }
    }
}
