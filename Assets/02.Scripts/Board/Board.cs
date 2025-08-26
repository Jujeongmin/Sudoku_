using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardParent;
    public GameObject numberPanel;
    public Button[] numberButtons;

    private Cell[,] cells = new Cell[9, 9];
    private Cell selectedCell;

    void Start()
    {
        GenerateBoard();
        SetupNumberPanel();
    }

    public void GenerateBoard()
    {
        int[,] initialNumbers = new int[9, 9] {
            {5,0,0,0,0,0,0,1,0},
            {0,0,0,0,7,0,0,0,0},
            {0,0,0,0,0,0,6,0,0},
            {0,0,0,1,0,0,0,0,0},
            {0,0,0,0,0,5,0,0,0},
            {0,0,0,0,0,0,0,0,3},
            {0,0,0,0,4,0,0,0,0},
            {0,0,2,0,0,0,0,0,0},
            {0,8,0,0,0,0,0,0,0}
        };

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                GameObject cellObj = Instantiate(cellPrefab, boardParent);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Init(r, c, OnCellClicked, initialNumbers[r, c]);
                cells[r, c] = cell;
            }
        }
    }

    public void SetupNumberPanel()
    {
        numberPanel.SetActive(false);

        foreach (var btn in numberButtons)
        {
            int number = int.Parse(btn.GetComponentInChildren<TMP_Text>().text);
            btn.onClick.AddListener(() => OnNumberSelected(number));
        }
    }

    public void OnCellClicked(Cell cell)
    {
        selectedCell = cell;
        numberPanel.SetActive(true);
    }

    public void OnNumberSelected(int number)
    {
        if (selectedCell != null)
            selectedCell.SetNumber(number);
        numberPanel.SetActive(false);
    }
}
