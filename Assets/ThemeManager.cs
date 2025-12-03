using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [Header("Backgrounds")]
    public GameObject blueBackground;
    public GameObject redBackground;

    [Header("Theme Button Colors")]
    public Image themeButtonImage;
    public Color blueColor;
    public Color redColor;

    private bool isRedTheme = false;

    public void ToggleTheme()
    {
        isRedTheme = !isRedTheme;

        // Background toggle
        blueBackground.SetActive(!isRedTheme);
        redBackground.SetActive(isRedTheme);

        // Button color toggle
        themeButtonImage.color = isRedTheme ? redColor : blueColor;
    }
}
