using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public GameObject audioPlayer;
    
        private void Start()
        {
            StartCoroutine(audioPlayer.GetComponent<AudioController>().FadeVolumeOut(5));
            StartCoroutine(audioPlayer.GetComponent<AudioController>().FadeVolumeIn(5, 6));
        }
    }
}
