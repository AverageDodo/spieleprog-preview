using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreTextController : MonoBehaviour
    {
        private void Start()
        {
            var comp = GetComponent<TextMeshProUGUI>();
            var gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            comp.text = gm.GetTimerString();
        }
    }
}
