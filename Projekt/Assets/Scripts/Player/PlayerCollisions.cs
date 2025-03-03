using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class is responsible for handling player collisions in the game.
    /// </summary>
    public class PlayerCollisions : MonoBehaviour
    {
        private GameManager _gm;

        /// <summary>
        /// The spawn position of the player. Used for resetting upon collision.
        /// </summary>
        private Vector3 _spawnPosition;
        
        private void Start()
        {
            _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            _spawnPosition = transform.position;
        }

        /// <summary>
        /// Handles the player's collision with triggers.
        /// If the player collides with a Finish trigger, the scene is switched.
        /// If the player collides with a Respawn trigger, the player's position is reset.
        /// /// If the player collides with an Enemy trigger, the player's position is reset.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Finish":
                    Debug.Log("Finish reached.");
                    _gm.SwitchScene(2);
                    break;

                case "Respawn":
                    ResetPosition();
                    break;
                
                case "Enemy":
                    ResetPosition();
                    break;
            }
        }

        /// <summary>
        /// Resets the player's position to the spawn position.
        /// </summary>
        private void ResetPosition() => transform.position = _spawnPosition;
    }
}
