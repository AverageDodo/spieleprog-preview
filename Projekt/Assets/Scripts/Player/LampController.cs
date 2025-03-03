using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class is responsible for controlling the lamp's behavior in the game.
    /// It includes methods for updating its position, and reducing its light range over time.
    /// </summary>
    public class LampController : MonoBehaviour
    {
        public float distanceAbovePlayer;

        /// <summary>
        /// The amount by which the light range is reduced every second.
        /// </summary>
        public float lightChange;

        private GameManager _gameManager;
        private Light _lightSource;
        private GameObject _player;

        /// <summary>
        /// The offset of the lamp from the player.
        /// </summary>
        private Vector3 _offset;

        /// <summary>
        /// The time interval in seconds at which the <c>ReduceLightRange</c> method is called.
        /// </summary>
        private float _updateRate;

        private void Awake()
        {
            _player = GameObject.Find("Player");
            _lightSource = GetComponentInChildren<Light>();
            _offset = new Vector3(0, distanceAbovePlayer, 0);
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            _updateRate = 0.01f;
        }

        private void Start() => InvokeRepeating(nameof(ReduceLightRange), 0, _updateRate);

        /// <summary>
        /// Updates the position of the lamp in each frame.
        /// The lamp's position is set to the player's position plus the offset.
        /// </summary>
        private void Update() => transform.position = _player.transform.position + _offset;

        /// <summary>
        /// Reduces the light range of the lamp over time.
        /// If the light range is greater than the specified light change, it decreases the light range by the
        /// light change multiplied by the <c>_updateRate</c>.
        /// If the light range is less than or equal to the light change, it triggers a scene switch in the game manager.
        /// </summary>
        private void ReduceLightRange()
        {
            if (_lightSource.range > lightChange) _lightSource.range -= (lightChange * _updateRate);
            else _gameManager.SwitchScene(2);
        }
    }
}
