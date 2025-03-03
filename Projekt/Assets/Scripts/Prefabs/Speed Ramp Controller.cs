using Player;
using UnityEngine;

namespace Prefabs
{
    /// <summary>
    /// This class is responsible for controlling the behavior of the speed ramp in the game.
    /// </summary>
    public class SpeedRampController : MonoBehaviour
    {
        public float speedModifier;
        public float lingerDuration = 0.05f;

        /// <summary>
        /// Handles the player's collision with the speed ramp trigger.
        /// If the player collides with the speed ramp, the player's speed modifier is increased by the speed ramp's
        /// speed modifier.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject;
            if (other.CompareTag("Player"))
            {
                player.GetComponent<PlayerController>().playerSpeedModifier += speedModifier;
            }
        }

        /// <summary>
        /// Handles the player's exit from the speed ramp trigger.
        /// If the player exits the speed ramp, the player's speed modifier is decreased by the speed ramp's speed modifier.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Invoke(nameof(ResetSpeedModifier), lingerDuration);
            }
        }

        private void ResetSpeedModifier()
        {
            GameObject.Find("Player").GetComponent<PlayerController>().playerSpeedModifier -= speedModifier;
        } 
    }
}
