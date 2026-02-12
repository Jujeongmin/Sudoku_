using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    [SerializeField] Button easyButton;
    [SerializeField] Button mediumButton;
    [SerializeField] Button hardButton;

    int cellEmpty = 0;

    public int IsCellEmpty { get { return cellEmpty; } }

    public bool isSelcetFlag = false;

    void Start()
    {
        easyButton.onClick.AddListener(() =>
        {
            StartGame();
            cellEmpty = 30;
            isSelcetFlag = true;
        });

        mediumButton.onClick.AddListener(() =>
        {
            StartGame();
            cellEmpty = 40;
            isSelcetFlag = true;
        });

        hardButton.onClick.AddListener(() =>
        {
            StartGame();
            cellEmpty = 50;
            isSelcetFlag = true;
        });
    }

    private void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}