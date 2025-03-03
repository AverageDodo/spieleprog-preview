using UnityEngine;

namespace Prefabs
{
    /// <summary>
    /// This class is responsible for the individual pickups in Level 3.
    /// </summary>
    public class PickUpController : MonoBehaviour
    {
        /// <summary>
        /// If the player collides with a pickup, it extends the lights range preventing the game over condition.
        /// Also destroys the pickup upon collision. 
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.Find("Laterne").GetComponentInChildren<Light>().range += 1.0f;
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
