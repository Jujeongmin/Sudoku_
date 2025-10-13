using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public TMP_Text numberText;
    public TMP_Text[] memoText = new TMP_Text[9];

    private Button button;
    private Image background;
    private int row;
    private int col;
    private bool isFixed = false;
    private Action<Cell> onClick;

    private Color defaultColor = Color.white;
    private HashSet<int> memoNumbers = new();
    private int highlightedNumber = 0;

    public int Row => row;
    public int Col => col;
    public bool IsFixed => isFixed;

    private void Awake()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick?.Invoke(this));
    }

    public void Init(int r, int c, Action<Cell> clickCallback, int initNumber = 0)
    {
        row = r;
        col = c;
        onClick = clickCallback;

        if (initNumber != 0)
        {
            SetFixedNumber(initNumber);
        }
        else
        {
            ClearNumber();
        }
    }

    public void SetNumber(int number)
    {
        if (!isFixed)
        {
            numberText.text = number == 0 ? "" : number.ToString();
            background.color = defaultColor;
            numberText.color = Color.black;
            ClearMemo();
        }
    }

    public void ShowError()
    {
        if (!isFixed)
        {
            StartCoroutine(FlashError());
        }
    }

    private IEnumerator FlashError()
    {
        background.color = Color.red;
        yield return new WaitForSeconds(.5f);
        background.color = defaultColor;
    }

    public void ClearNumber(bool force = false)
    {
        if (!isFixed || force)
        {
            numberText.text = "";
            background.color = defaultColor;
            isFixed = false;
            numberText.color = Color.black;
            ClearMemo();
        }
    }

    public void SetFixedNumber(int number)
    {
        numberText.text = number.ToString();
        isFixed = true;
        numberText.color = Color.gray;
        ClearMemo();
    }

    public void AddMemo(int number)
    {
        if (numberText.text != "" || number < 1 || number > 9) return;
        memoNumbers.Add(number);
        UpdateMemoText();
    }

    public void RemoveMemo(int number)
    {
        memoNumbers.Remove(number);
        UpdateMemoText();
    }

    public void ClearMemo()
    {
        memoNumbers.Clear();
        UpdateMemoText();
    }

    private void UpdateMemoText()
    {
        for(int i = 0; i < 9; i++)
        {
            if(memoNumbers.Contains(i + 1))
            {
                if (i + 1 == highlightedNumber)
                {
                    memoText[i].color = Color.black;
                    memoText[i].fontStyle = FontStyles.Bold;
                }
                else
                {
                    memoText[i].color = Color.gray;
                    memoText[i].fontStyle = FontStyles.Normal;
                }

                memoText[i].text = (i + 1).ToString();
            }
            else
            {
                memoText[i].text = "";
            }
        }
    }

    public void SetHighlightedNumber(int number)
    {
        highlightedNumber = number;
        UpdateMemoText();
    }

    public void Highlight(Color color)
    {
        background.color = color;
    }

    public void ResetColor()
    {
        background.color = defaultColor;
    }
}