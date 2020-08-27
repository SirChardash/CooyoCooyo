namespace Code.Board
{
  public class BoardState
  {
    private Block[,] _board;

    public BoardState(int height, int width)
    {
      _board = new Block[height, width];
    }

    public BoardState(Block[,] board)
    {
      _board = board;
    }

    public bool IsEmpty(int x, int y)
    {
      return x >= 0
             && x < _board.GetLength(1)
             && y < _board.GetLength(0)
             && _board[y, x] == Block.Empty;
    }

    public Block Get(int x, int y)
    {
      return _board[y, x];
    }

    public void Set(int x, int y, Block block)
    {
      _board[y, x] = block;
    }

    public Block[,] Clone()
    {
      return (Block[,]) _board.Clone();
    }

    public void Set(Block[,] board)
    {
      _board = board;
    }
  }
}