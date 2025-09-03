using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Board board;
    public GameObject clearPanel;

    private void Start()
    {
        int emptyCount = PlayerPrefs.GetInt("SelectedMode", 30);
        board.StartNewGame(emptyCount);

        board.OnGameCleared += ShowClearPanel;

        clearPanel.SetActive(false);
    }

    private void ShowClearPanel()
    {
        clearPanel.SetActive(true);
    }

    public void HideClearPanel()
    {
        clearPanel.SetActive(false);
    }

    public void ClearBoard()
    {
        SceneManager.LoadScene("StartScene");
    }
}
