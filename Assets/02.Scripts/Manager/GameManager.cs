using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Board board;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public GameObject clearPanel;

    private void Start()
    {
        board.OnGameCleared += ShowButtons;

        easyButton.gameObject.SetActive(false);
        mediumButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(false);
        clearPanel.SetActive(false);

        easyButton.onClick.AddListener(() => StartNewGame(30));
        mediumButton.onClick.AddListener(() => StartNewGame(40));
        hardButton.onClick.AddListener(() => StartNewGame(50));
    }

    private void StartNewGame(int emptyCount)
    {
        HideButtons();
        board.StartNewGame(emptyCount);
    }

    private void ShowButtons()
    {
        clearPanel.SetActive(true);
        easyButton.gameObject.SetActive(true);
        mediumButton.gameObject.SetActive(true);
        hardButton.gameObject.SetActive(true);
    }

    private void HideButtons()
    {
        clearPanel.SetActive(false);
        easyButton.gameObject.SetActive(false);
        mediumButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(false);
    }
}
