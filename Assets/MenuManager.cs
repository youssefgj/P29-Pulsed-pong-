using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f; // Ensure game time is running
        AudioListener.pause = false; // Ensure audio is playing
        
        SceneManager.LoadScene("SampleScene");   // game scene
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // settings scene
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // main menu scene
    }
}
