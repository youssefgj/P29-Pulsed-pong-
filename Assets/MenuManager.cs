using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
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
