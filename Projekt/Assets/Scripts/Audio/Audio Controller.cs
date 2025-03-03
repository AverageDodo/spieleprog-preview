using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        private AudioSource _audiosrc;

        private void Awake() => _audiosrc = gameObject.GetComponent<AudioSource>();

        private void Start() => _audiosrc.Play();

        /// <summary>
        /// Gradually reduces the volume of the audio source over a specified duration.
        /// The method uses a coroutine to gradually interpolate the volume of the audio source from its current
        /// volume to 0. The interpolation is done over the specified duration.
        /// The method also supports an optional delay before the volume reduction starts.
        /// </summary>
        /// <param name="duration">The duration over which the volume should be reduced.</param>
        /// <param name="delay">The delay before the volume reduction starts. Defaults to 0.</param>
        /// <returns>An IEnumerator that can be used to control the coroutine.</returns>
        public IEnumerator FadeVolumeOut(float duration, float delay = 0)
        {
            yield return new WaitForSeconds(delay);

            float startVolume = _audiosrc.volume;
            float time = 0;
            while (time < duration)
            {
                _audiosrc.volume = Mathf.Lerp(startVolume, 0, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Gradually increases the volume of the audio source over a specified duration.
        /// The method uses a coroutine to gradually interpolate the volume of the audio source from 0 to 0.5.
        /// The interpolation is done over the specified duration.
        /// The method also supports an optional delay before the volume increase starts.
        /// </summary>
        /// <param name="duration">The duration over which the volume should be increased.</param>
        /// <param name="delay">The delay before the volume increase starts. Defaults to 0.</param>
        /// <returns>An IEnumerator that can be used to control the coroutine.</returns>
        public IEnumerator FadeVolumeIn(float duration, float delay = 0)
        {
            yield return new WaitForSeconds(delay);

            float time = 0;
            while (time < duration)
            {
                _audiosrc.volume = Mathf.Lerp(0, 0.5F, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
