using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static field that tracks the elapsed time for all levels.
    /// </summary>
    public static float ElapsedTime { get; private set; } = 0;

    private void Update() => ElapsedTime = Time.fixedTime;

    public void SwitchScene(int delayInSeconds) => Invoke(nameof(SwitchScene), delayInSeconds);

    private void SwitchScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    /// <summary>
    /// Returns a string representation the elapsed time divided into minutes, seconds, and milliseconds.
    /// Each unit of time is calculated by using the <c>Mathf.FloorToInt</c> method on the appropriate
    /// division of the elapsed time.
    /// </summary>
    /// <returns>a string representation of the elapsed time in the format "mm:ss.fff".</returns>
    public string GetTimerString()
    {
        var minutes = Mathf.FloorToInt(ElapsedTime / 60);
        var seconds = Mathf.FloorToInt(ElapsedTime % 60);
        var milliseconds = Mathf.FloorToInt(ElapsedTime * 1000 % 1000);

        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}
