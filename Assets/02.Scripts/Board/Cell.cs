using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public TMP_Text numberText;

    private Button button;
    private Image background;
    private int row;
    private int col;
    private bool isFixed = false;
    private Action<Cell> onClick;

    private Color defaultColor = Color.white;

    public int Row => row;
    public int Col => col;
    public bool IsFixed => isFixed;

    private void Awake()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
    }

    public void Init(int r, int c, Action<Cell> clickCallback, int initNumber = 0)
    {
        row = r;
        col = c;
        onClick = clickCallback;
        button = GetComponent<Button>();

        button.onClick.AddListener(() => onClick(this));

        if (initNumber != 0)
        {
            SetFixedNumber(initNumber);
        }
        else
        {
            numberText.text = "";
        }
    }

    public void SetNumber(int number)
    {
        if (!isFixed)
        {
            numberText.text = number == 0 ? "" : number.ToString();
            background.color = defaultColor;
            numberText.color = Color.black;
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
        }
    }

    public void SetFixedNumber(int number)
    {
        numberText.text = number.ToString();
        isFixed = true;
        numberText.color = Color.gray;
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
