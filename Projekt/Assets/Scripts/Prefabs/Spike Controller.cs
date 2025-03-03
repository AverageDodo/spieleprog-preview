using UnityEngine;
using Random = UnityEngine.Random;

namespace Prefabs
{
    /// <summary>
    /// This class is responsible for controlling the behavior of the spikes in Level 2.
    /// </summary>
    public class SpikeController : MonoBehaviour
    {
        /// <summary>
        /// The frequency at which the spike move up and down.
        /// </summary>
        public float frequency;

        /// <summary>
        /// The amplitude of the spikes' up and down movement.
        /// </summary>
        public float amplitude;

        /// <summary>
        /// The starting position of the spike in world space.
        /// </summary>
        private Vector3 _startPosition;

        /// <summary>
        /// A random offset used to vary the movement of the spikes so that they don't move in parallel.
        /// </summary>
        private float _randomOffset;

        private void Start()
        {
            _startPosition = transform.position;
            _randomOffset = Random.value;
        }

        /// <summary>
        /// Updates the position of the spikes in each frame.
        /// The spikes are moved up and down based on a sine wave with the specified frequency and amplitude.
        /// </summary>
        private void Update()
        {
            transform.position = _startPosition
                                 + new Vector3(0, amplitude, 0)
                                 * Mathf.Sin(frequency * Time.time * _randomOffset);
        }
    }
}
