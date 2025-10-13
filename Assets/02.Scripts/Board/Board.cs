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
    public bool isMemoMode = false;
    public Button memoButton;
    public Color memoButtonColor = Color.yellow;
    public Color normalButtonColor = Color.white;

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

        int selectedNumber = 0;
        if (!string.IsNullOrEmpty(cell.numberText.text))
        {
            selectedNumber = int.Parse(cell.numberText.text);
        }

        foreach (var c in cells)
        {
            c.SetHighlightedNumber(selectedNumber);
        }

        if (cell.IsFixed && selectedNumber != 0)
        {
            HighlightNumberAndCross(selectedNumber, cell.Row, cell.Col);
        }

        UpdateNumberPanel();
    }

    public void OnNumberSelected(int number)
    {
        if (selectedCell != null && !selectedCell.IsFixed)
        {
            if (isMemoMode)
            {
                if (selectedCell.memoText[number - 1].text != "")
                {
                    selectedCell.RemoveMemo(number);
                }
                else
                {
                    selectedCell.AddMemo(number);
                }
            }
            else
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

                    RemoveMemo(number, r, c);
                }

                CheckGameClear();
            }
        }
    }

    private void RemoveMemo(int number, int row, int col)
    {
        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (r == row && c == col) continue;

                bool sameRow = r == row;
                bool sameCol = c == col;
                bool sameBlock = r >= startRow && r < startRow + 3 && c >= startCol && c < startCol + 3;

                if (sameRow || sameCol || sameBlock)
                {
                    cells[r, c].RemoveMemo(number);
                }
            }
        }
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

    public void UpdateNumberPanel()
    {
        if (selectedCell == null) return;

        int[] counts = new int[10];
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (int.TryParse(cells[r, c].numberText.text, out int num))
                {
                    counts[num]++;
                }
            }
        }

        for (int i = 0; i < numberButtons.Length; i++)
        {
            int num = i + 1;
            Button btn = numberButtons[i];
            TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();

            if (counts[num] >= 9)
            {
                btn.interactable = false;
                btnText.color = Color.gray;
            }
            else
            {
                btn.interactable = true;
                btnText.color = Color.black;
            }
        }
    }

    public void ToggleMemoMode()
    {
        isMemoMode = !isMemoMode;

        var colors = memoButton.colors;
        if (isMemoMode)
        {
            memoButton.GetComponentInChildren<TMP_Text>().text = "메모중";
            colors.normalColor = memoButtonColor;
            colors.highlightedColor = memoButtonColor;
            colors.pressedColor = memoButtonColor;
            colors.selectedColor = memoButtonColor;
        }
        else
        {
            memoButton.GetComponentInChildren<TMP_Text>().text = "메모";
            colors.normalColor = normalButtonColor;
            colors.highlightedColor = normalButtonColor;
            colors.pressedColor = normalButtonColor;
            colors.selectedColor = normalButtonColor;
        }
        memoButton.colors = colors;
    }
}