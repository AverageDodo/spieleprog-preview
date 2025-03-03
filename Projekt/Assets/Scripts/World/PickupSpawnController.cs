using UnityEngine;
using static UnityEngine.Mathf;

namespace World
{
    /// <summary>
    /// The <c>PickupSpawnController</c> class is responsible for spawning, maintaining and disposing of the Pickups
    /// found in Level 3. It also extends the spawning logic to take the elapsed time into account, making the
    /// game harder as time goes on by placing the pickups further away from the player.
    /// </summary>
    internal class PickupSpawnController : SpawnController
    {
        private int _initialSpawnRadius;
        private float _initialSpaceBetweenPrefabs;
        private float _initialSpawnAngle;

        protected override void Awake()
        {
            base.Awake();

            _initialSpawnRadius = spawnRadius;
            _initialSpaceBetweenPrefabs = spaceBetweenPrefabs;
            _initialSpawnAngle = spawnAngle;
        }

        protected override void Start()
        {
            RandomPrefabRotation = new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
            VerticalSpawnOffset = Random.Range(0.4f, 0.8f);

            base.Start();

            InvokeRepeating(nameof(UpdateSpawnSettings), 10, 1);
        }

        /// <summary>
        /// Updates the spawn settings for the prefabs.
        /// The values of spawn radius, space between prefabs, and spawn angle are calculated based on the
        /// elapsed game time and the initial values of those attributes. Each attribute is adjusted by its own formula.
        /// </summary>
        private void UpdateSpawnSettings()
        {
            var time = GameManager.ElapsedTime;
            
            spawnRadius = (int)(Pow(Sqrt(0.02f * time), 5) + _initialSpawnRadius);
            spaceBetweenPrefabs = Exp(0.001f * time) - 1 + _initialSpaceBetweenPrefabs;
            spawnAngle = Exp(0.01f * time) - 1 + _initialSpawnAngle;
        }
    }
}
