using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public TMP_Text numberText;

    private Button button;
    private int row;
    private int col;
    private bool isFixed = false;
    private Action<Cell> onClick;

    public void Init(int r, int c, Action<Cell> clickCallback, int initNumber = 0)
    {
        row = r;
        col = c;
        onClick = clickCallback;
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            if (!isFixed) onClick(this);
        });

        if (initNumber != 0)
        {
            numberText.text = initNumber.ToString();
            isFixed = true;
            numberText.color = Color.gray;
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
            numberText.text = number.ToString();
        }
    }
}
