using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    /// <summary>
    /// The SpawnController class is an abstract base class for Game Objects responsible for spawning and
    /// managing other prefabs in a set radius around the player.
    /// </summary>
    public abstract class SpawnController : MonoBehaviour
    {
        /// <summary>
        /// Represents the player GameObject. This is the GameObject that the SpawnController will use as a reference
        /// for spawning and managing other prefabs in a set radius around it.
        /// </summary>
        public GameObject player;

        /// <summary>
        /// An array of GameObjects that can be spawned by the SpawnController.
        /// </summary>
        public GameObject[] gameObjects;

        /// <summary>
        /// The number of prefabs that the SpawnController should maintain in the game world.
        /// </summary>
        public int prefabCount;

        /// <summary>
        /// The radius around the player within which the SpawnController should spawn prefabs. Marked as volatile
        /// because a child class continuously updates its value from a different thread.
        /// </summary>
        public volatile int spawnRadius;

        /// <summary>
        /// The minimum distance that should be maintained between spawned prefabs of this instance. Marked as volatile
        /// because a child class continuously updates its value from a different thread.
        /// </summary>
        public volatile float spaceBetweenPrefabs;

        /// <summary>
        /// The minimum scale that a spawned prefab can have.
        /// </summary>
        public float minScale;

        /// <summary>
        /// The maximum scale that a spawned prefab can have.
        /// </summary>
        public float maxScale;

        /// <summary>
        /// The radius of a spawned prefab. Needed for scaling operation.
        /// </summary>
        public float prefabRadius;

        /// <summary>
        /// The angle within prefabs should be spawned relative to the player's forward direction. Marked as volatile
        /// because a child class continuously updates its value from a different thread.
        /// </summary>
        public volatile float spawnAngle;

        /// <summary>
        /// Backing field for <see cref="spawnAngle"/>.
        /// </summary>
        private float _spawnAngle;

        /// <summary>
        /// The list of currently spawned prefabs by this instance.
        /// </summary>
        private List<GameObject> _prefabs;

        /// <summary>
        /// The position of the player during the previous frame.
        /// </summary>
        private Vector3 _previousPlayerPos;

        /// <summary>
        /// The current position of the player.
        /// </summary>
        private Vector3 _currentPlayerPos;

        /// <summary>
        /// The rotation to be applied by default to all prefabs spawned by this instance.
        /// </summary>
        protected Vector3 RandomPrefabRotation;

        /// <summary>
        /// The vertical offset for spawning prefabs. This value is used to adjust the height at which prefabs are spawned.
        /// </summary>
        protected float VerticalSpawnOffset;

        /// <summary>
        /// A static object used for locking to ensure thread safety during prefab destruction and creation in the
        /// CheckPrefabs method.
        /// </summary>
        private static readonly object LockObject = new();

        /// <summary>
        /// This method initializes the Prefabs list, sets the SpawnAngle, PreviousPlayerPos, CurrentPlayerPos,
        /// RandomPrefabRotation and VerticalSpawnOffset.
        /// </summary>
        protected virtual void Awake()
        {
            _prefabs = new List<GameObject>();
            _spawnAngle = spawnAngle;
            _previousPlayerPos = new Vector3();
            _currentPlayerPos = player.transform.position;
            RandomPrefabRotation = new Vector3();
            VerticalSpawnOffset = 0;
        }

        /// <summary>
        /// Initial spawning logic and input validation is called here.
        /// </summary>
        protected virtual void Start()
        {
            ValidateAttributes();
            InitialGeneration();
        }

        /// <summary>
        /// Validates the attributes of the SpawnController. If any of these attributes have invalid values,
        /// an ArgumentOutOfRangeException is thrown.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any value is invalid.</exception>
        private void ValidateAttributes()
        {
            if (!player)
                throw new ArgumentNullException(nameof(player), "Player GameObject cannot be null.");
            if (gameObjects.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(gameObjects),
                    "gameObjects must contain at least one prefab.");
            if (prefabCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(prefabCount), "Must be a positive number.");
            if (spawnRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(spawnRadius), "spawnRadius must be a positive number.");
            if (minScale == 0 || maxScale == 0)
                throw new ArgumentOutOfRangeException(nameof(minScale) + nameof(maxScale),
                    "Scale value cannot be zero.");
            if (minScale >= maxScale)
                throw new ArgumentOutOfRangeException(nameof(minScale), "minScale must be smaller than maxScale!");
            if (prefabRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(prefabRadius), "prefabRadius must be a positive number.");
            if (spawnAngle is < 0 or > 360)
                throw new ArgumentOutOfRangeException(nameof(spawnAngle), "spawnAngle must be between 0 and 360.");
        }

        protected virtual void Update() => CheckPrefabs();

        /// <summary>
        /// Checks the current prefabs in the instances' list. If a prefab is outside the spawn radius, it is destroyed.
        /// When a prefab is destroyed, a new prefab is generated and added to the list.
        /// </summary>
        private void CheckPrefabs()
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                if (!_prefabs[i])
                {
                    _prefabs.RemoveAt(i);
                    _prefabs.Insert(i, GenerateNewPrefab(Random.Range(0, gameObjects.Length)));
                }

                if (Vector3.Distance(_prefabs[i].transform.position, player.transform.position) > spawnRadius + 3)
                {
                    lock (LockObject)
                    {
                        var prefab = _prefabs[i];
                        _prefabs.RemoveAt(i);
                        _prefabs.Insert(i, GenerateNewPrefab(Random.Range(0, gameObjects.Length)));
                        Destroy(prefab);
                    }
                }
            }
        }

        /// <summary>
        /// This method is responsible for creating the initial set of prefabs up to the specified prefabCount.
        /// The spawn angle is temporarily set to 360 degrees to allow prefabs to spawn in any direction around the player.
        /// After the initial generation, the spawn angle is reset to its original value.
        /// </summary>
        private void InitialGeneration()
        {
            while (_prefabs.Count < prefabCount)
            {
                spawnAngle = 360.0f;
                var newPrefab = GenerateNewPrefab(Random.Range(0, gameObjects.Length), 1);

                _prefabs.Add(newPrefab);
            }

            spawnAngle = _spawnAngle;
        }

        /// <summary>
        /// Generates a new prefab and returns it as a child object. The prefab is instantiated at a valid spawn point
        /// calculated by the GetSpawnPoint method, with a random rotation and scale. 
        /// </summary>
        /// <param name="index">The index of the prefab to be generated from the gameObjects array.</param>
        /// <param name="mode">The mode of spawning. If mode is 0, the spawn point is calculated based on the
        /// player's movement direction. If mode is not 0, the spawn point is calculated based on the forward direction.
        /// </param>
        /// <returns>
        /// A GameObject representing the new prefab.
        /// </returns>
        private GameObject GenerateNewPrefab(int index, int mode = 0)
        {
            var scalingFactor = Random.Range(minScale, maxScale);
            var spawnPos = GetSpawnPoint(scalingFactor, mode);
            RandomPrefabRotation = new Vector3(0, Random.Range(0.0f, 90.0f), 0);
            var randomRotation = new Quaternion
            {
                eulerAngles = RandomPrefabRotation
            };

            var newPrefab = Instantiate(
                gameObjects[index],
                spawnPos,
                randomRotation
            );
            newPrefab.transform.localScale = Vector3.one * scalingFactor;

            newPrefab.transform.SetParent(gameObject.transform, true);

            return newPrefab;
        }

        /// <summary>
        /// Calculates and returns a spawn point for a new prefab. The spawn point is calculated such that it is within
        /// the spawn radius and at a random angle within the spawn angle from the player's position or forward
        /// direction, depending on the mode.
        /// </summary>
        /// <param name="scale">The scaling factor for the prefab to be spawned.</param>
        /// <param name="mode">The mode of spawning. If mode is 0, the spawn point is calculated based on the
        /// player's movement direction. If mode is not 0, the spawn point is calculated based on the forward direction.
        /// </param>
        /// <returns>
        /// A Vector3 representing the spawn point for the new prefab. If a valid spawn point cannot be found after
        /// 50 attempts, the method returns the last calculated spawn point regardless of its validity.
        /// </returns>
        private Vector3 GetSpawnPoint(float scale, int mode = 0)
        {
            var spawnPos = new Vector3();
            _currentPlayerPos = player.transform.position;
            var orientation = (mode == 0) ? (_currentPlayerPos - _previousPlayerPos).normalized : Vector3.forward;

            var loopIndex = 0;
            var isValid = false;
            while (!isValid)
            {
                var angle = Random.Range(-spawnAngle / 2.0f, spawnAngle / 2.0f);
                var rotation = Quaternion.Euler(0, angle, 0);
                var rotatedOrientation = rotation * orientation;
                var radius = (mode == 0) ? spawnRadius : Random.Range(1.0f, spawnRadius);

                spawnPos = new Vector3
                {
                    x = _currentPlayerPos.x + rotatedOrientation.x * radius,
                    y = 10,
                    z = _currentPlayerPos.z + rotatedOrientation.z * radius
                };

                Physics.Raycast(spawnPos, Vector3.down, out var hitInfo, 25.0f);
                spawnPos.y -= hitInfo.distance - VerticalSpawnOffset;

                isValid = ValidateSpawnPoint(scale, spawnPos);

                loopIndex++;
                if (loopIndex > 50)
                {
                    isValid = true;
                    spawnPos = new Vector3(0, 1000, 0);
                }
            }

            
            return spawnPos;
        }

        /// <summary>
        /// Validates the spawn point for a new prefab.
        /// A spawn point is considered valid if it is at a distance greater than the combined radius
        /// of the prefab and the space between prefabs from all other prefabs.
        /// </summary>
        /// <param name="scalingFactor">The scaling factor for the prefab to be spawned.</param>
        /// <param name="spawnPos">The position where the prefab is to be spawned.</param>
        /// <returns>
        /// a boolean value indicating whether the spawn point is valid or not.
        /// </returns>
        private bool ValidateSpawnPoint(float scalingFactor, Vector3 spawnPos)
        {
            var isValid = false;

            for (int i = 0; i < _prefabs.Count; i++)
            {
                var prefab = _prefabs[i];
                var combinedRadius =
                    (prefab.transform.localScale.x * prefabRadius) + (scalingFactor * prefabRadius);
                var distance = Vector3.Distance(prefab.transform.position, spawnPos);

                if (distance < combinedRadius + spaceBetweenPrefabs) break;

                if (_prefabs.Count == i + 1)
                {
                    isValid = true;
                    _previousPlayerPos = _currentPlayerPos;
                }
            }

            return isValid;
        }
    }
}
