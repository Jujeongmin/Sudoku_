using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    [SerializeField] Board board;

    [SerializeField] GameObject clearPanel;

    [SerializeField] ModeManager modeManager;

    public ModeManager IsModeManager { get { return modeManager; } }

    public static GManager Instance { get; private set; } = null;

    private void Awake()
    {
        switch (GManager.Instance)
        {
            case null:
                Instance = this;
                DontDestroyOnLoad(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Start()
    {
        if (!modeManager.isSelcetFlag) return;

        int emptyCount = PlayerPrefs.GetInt("SelectedMode", modeManager.IsCellEmpty);
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