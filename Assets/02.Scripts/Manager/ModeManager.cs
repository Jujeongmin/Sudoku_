using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    void Start()
    {
        easyButton.onClick.AddListener(() => StartGame(30));
        mediumButton.onClick.AddListener(() => StartGame(40));
        hardButton.onClick.AddListener(() => StartGame(50));
    }

    private void StartGame(int emptyCount)
    {
        PlayerPrefs.SetInt("SelectedMode", emptyCount);
        SceneManager.LoadScene("MainScene");
    }
}
