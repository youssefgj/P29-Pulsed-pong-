using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");   // name of your game scene
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene"); // name of your settings scene
    }
}
