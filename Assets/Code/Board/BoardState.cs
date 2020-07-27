using UnityEngine;

namespace Code.Board
{
  public class BoardState
  {
    private int[,] _board;

    public BoardState()
    {
      _board = new int[10, 8];
    }

    public bool IsEmpty(int x, int y)
    {
      return x >= 0 && x < 8 && y < 10 && _board[y, x] == 0;
    }

    public int Get(int x, int y)
    {
      return _board[y, x];
    }

    public void Set(int x, int y, int code)
    {
      _board[y, x] = code;
    }

    public int[,] Clone()
    {
      return (int[,]) _board.Clone();
    }

    public void Set(int[,] board)
    {
      _board = board;
    }
  }
}