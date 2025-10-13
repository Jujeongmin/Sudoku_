using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator
{
    private int[,] board = new int[9, 9];

    public int[,] GeneratePuzzle(int emptyCells, out int[,] solution)
    {
        Array.Clear(board, 0, board.Length);
        FillBoard();
        solution = (int[,])board.Clone();
        RemoveCells(emptyCells);
        return board;
    }

    private bool FillBoard()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (board[r, c] == 0)
                {
                    int[] numbers = GetShuffledNumbers();
                    foreach (int num in numbers)
                    {
                        if (IsValidMove(r, c, num))
                        {
                            board[r, c] = num;
                            if (FillBoard()) return true;
                            board[r, c] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    private void RemoveCells(int emptyCells)
    {
        List<(int, int)> positions = new List<(int, int)>();
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                positions.Add((r, c));

        for (int i = 0; i < positions.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, positions.Count);
            var temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }

        for (int i = 0; i < emptyCells; i++)
        {
            var (r, c) = positions[i];
            board[r, c] = 0;
        }
    }

    private int[] GetShuffledNumbers()
    {
        int[] nums = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = 0; i < 9; i++)
        {
            int j = UnityEngine.Random.Range(i, 9);
            int temp = nums[i];
            nums[i] = nums[j];
            nums[j] = temp;
        }
        return nums;
    }

    private bool IsValidMove(int row, int col, int num)
    {
        for (int c = 0; c < 9; c++)
            if (board[row, c] == num) return false;

        for (int r = 0; r < 9; r++)
            if (board[r, col] == num) return false;

        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int r = startRow; r < startRow + 3; r++)
            for (int c = startCol; c < startCol + 3; c++)
                if (board[r, c] == num) return false;

        return true;
    }
}