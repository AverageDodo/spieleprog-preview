using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerController : MonoBehaviour
    {
        private GameManager _gameManager;
        private TextMeshProUGUI _tmp;

        private void Start()
        {
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            _tmp = gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Update() => _tmp.text = _gameManager.GetTimerString();
    }
}
