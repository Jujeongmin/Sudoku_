using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardParent;
    public GameObject numberPanel;
    public Button[] numberButtons;
    public Button clearButton;
    public Color highlightColor = Color.white;
    public Color sameNumberColor = Color.white;
    public Color selectedCellColor = Color.gray;

    private Cell[,] cells = new Cell[9, 9];
    private Cell selectedCell;
    private PuzzleGenerator generator;
    private int[,] solution;

    public event Action OnGameCleared;

    void Start()
    {
        InitializeCells();
        SetupNumberPanel();

        generator = new PuzzleGenerator();
        int[,] puzzle = generator.GeneratePuzzle(40, out solution);
        ApplyPuzzle(puzzle);
    }

    private void InitializeCells()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                GameObject cellObj = Instantiate(cellPrefab, boardParent);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Init(r, c, OnCellClicked);
                cells[r, c] = cell;
            }
        }
    }

    private void ApplyPuzzle(int[,] puzzle)
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (puzzle[r, c] != 0)
                    cells[r, c].SetFixedNumber(puzzle[r, c]);
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

        clearButton.onClick.AddListener(() => OnNumberSelected(0));
    }

    public void OnCellClicked(Cell cell)
    {
        selectedCell = cell;
        numberPanel.SetActive(true);
        ResetAllHighlights();

        cell.Highlight(selectedCellColor);

        if (cell.IsFixed)
        {
            int num = int.Parse(cell.numberText.text);
            HighlightNumberAndCross(num, cell.Row, cell.Col);
        }
    }

    public void OnNumberSelected(int number)
    {
        if (selectedCell != null && !selectedCell.IsFixed)
        {
            int r = selectedCell.Row;
            int c = selectedCell.Col;

            if (number == 0)
            {
                selectedCell.ClearNumber();
            }
            else if (number != solution[r, c])
            {
                selectedCell.ClearNumber();
                selectedCell.ShowError();
            }
            else
            {
                selectedCell.SetNumber(number);
            }

            CheckGameClear();
        }

        numberPanel.SetActive(false);
    }

    private void CheckGameClear()
    {
        if (IsBoardFull() && IsPuzzleSolved())
        {
            OnGameCleared?.Invoke();
        }
    }

    private bool IsPuzzleSolved()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (cells[r, c].numberText.text != solution[r, c].ToString()) return false;
            }
        }

        return true;
    }

    private bool IsBoardFull()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (string.IsNullOrEmpty(cells[r, c].numberText.text))
                    return false;
            }
        }
        return true;
    }

    private void HighlightNumberAndCross(int number, int clickedRow, int clickedCol)
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                cells[r, c].ResetColor();

                if (cells[r, c].numberText.text == number.ToString())
                    cells[r, c].Highlight(sameNumberColor);
            }
        }

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (cells[r, c].numberText.text == number.ToString())
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (cells[r, i].numberText.text != number.ToString())
                            cells[r, i].Highlight(highlightColor);

                        if (cells[i, c].numberText.text != number.ToString())
                            cells[i, c].Highlight(highlightColor);
                    }
                }
            }
        }
    }

    private void ResetAllHighlights()
    {
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                cells[r, c].ResetColor();
    }

    public void StartNewGame(int emptyCount)
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                cells[r, c].ClearNumber(true);
                cells[r, c].ResetColor();
            }
        }

        int[,] puzzle = generator.GeneratePuzzle(emptyCount, out solution);
        ApplyPuzzle(puzzle);
    }
}
