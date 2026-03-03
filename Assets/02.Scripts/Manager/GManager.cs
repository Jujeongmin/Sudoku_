using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    BoardManager boardManager;

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

    public void GetBoardManager(BoardManager manager)
    {
        boardManager = manager;       
    }

    public void ShowClearPanel()
    {
        if (boardManager.IsClearPanel != null) boardManager.IsClearPanel.SetActive(true);
    }

    public void HideClearPanel()
    {
        if (boardManager.IsClearPanel != null) boardManager.IsClearPanel.SetActive(false);
    }    
}