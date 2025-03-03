using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main_Menu
{
    public class MenuButtonController : MonoBehaviour
    {
        public void StartGame() => SceneManager.LoadScene(3);

        public void StartDemoMode() => SceneManager.LoadScene(1);

        public void ToMainMenu() => SceneManager.LoadScene(0);

        public void ExitGame() {
            //UnityEditor.EditorApplication.isPlaying = false;
        } 
    }
}
