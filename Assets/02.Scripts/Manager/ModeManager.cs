using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeManager : MonoBehaviour
{
    public bool isSelcetFlag = false;

    private void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ClickEasy()
    {
        isSelcetFlag = true;
        PlayerPrefs.SetInt("SelectedMode", 30);
        StartGame();
    }

    public void ClickMedium()
    {
        isSelcetFlag = true;
        PlayerPrefs.SetInt("SelectedMode", 40);
        StartGame();
    }

    public void ClickHard()
    {
        isSelcetFlag = true;
        PlayerPrefs.SetInt("SelectedMode", 50);
        StartGame();
    }
}