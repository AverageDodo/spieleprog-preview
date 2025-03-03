using System.Collections.Generic;
using UnityEngine;

namespace Prefabs
{
    /// <summary>
    /// This class is responsible for controlling the behavior of the arrow wall in Level 2.
    /// </summary>
    public class ArrowWallController : MonoBehaviour
    {
        /// <summary>
        /// The frequency at which the wall shoots arrows in seconds.
        /// </summary>
        public float shootingFrequency;
        public float arrowSpeedModifier;
        public float arrowDespawnTime;
        private Vector3 ShootingDirection { get; set; }
        public GameObject arrowPrefab;

        private List<Vector3> _arrowSpawns;

        /// <summary>
        /// Initializes the shooting direction and the arrow spawn points by finding all child objects' transforms
        /// representing the rings on the wall.
        /// </summary>
        private void Start()
        {
            ShootingDirection = transform.forward * -1;
            _arrowSpawns = new List<Vector3>();
            var leftRings = transform.Find("Left Rings");

            for (int i = 0; i < leftRings.transform.childCount; i++)
            {
                var position = leftRings.transform.GetChild(i).transform.position;
                _arrowSpawns.Add(position);
            }
            
            var middleRings = transform.Find("Middle Rings");

            for (int i = 0; i < middleRings.transform.childCount; i++)
            {
                var position = middleRings.transform.GetChild(i).transform.position;
                _arrowSpawns.Add(position);
            }

            var rightRings = transform.Find("Right Rings");

            for (int i = 0; i < rightRings.transform.childCount; i++)
            {
                var position = rightRings.transform.GetChild(i).transform.position;
                _arrowSpawns.Add(position);
            }
        }

        /// <summary>
        /// Starts shooting arrows when the player enters the trigger boundary.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) InvokeRepeating(nameof(FireArrows), 0.5f, shootingFrequency);
        }

        /// <summary>
        /// Stops shooting arrows when the player exits the trigger boundary.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) CancelInvoke();
        }

        /// <summary>
        /// Fires arrows from all the spawn points. Also corrects the orientation of the prefab.
        /// </summary>
        private void FireArrows()
        {
            foreach (var arrowSpawn in _arrowSpawns)
            {
                var arrow = Instantiate(
                    arrowPrefab, arrowSpawn, 
                    Quaternion.LookRotation(ShootingDirection)
                );
                arrow.transform.Rotate(new Vector3(1, 0, 0), 90);
                
                arrow.GetComponent<Rigidbody>().AddForce(ShootingDirection * arrowSpeedModifier, ForceMode.Impulse);
                Destroy(arrow, arrowDespawnTime);
            }
        }
    }
}
