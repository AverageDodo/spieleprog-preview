using UnityEngine;

namespace Prefabs
{
    /// <summary>
    /// This class is used by the Button Object in Level 2.
    /// </summary>
    public class ButtonController : MonoBehaviour
    {
        public GameObject ramp;
        public Material greenEmissive;
        
        /// <summary>
        /// Spawns the 'speed ramp' and changes the buttons' color to green when the player moves onto the button.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.GetComponent<MeshRenderer>().material = greenEmissive;
                ramp.SetActive(true);
            }
        }
    }
}
