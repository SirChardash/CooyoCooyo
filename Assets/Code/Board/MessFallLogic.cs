namespace Code.Board
{
  public class MessFallLogic
  {

    private readonly BoardState _board;
    private MessBlocks _messBlocks;
    
    public MessFallLogic(BoardState board)
    {
      _board = board;
    }

    public bool IsDone()
    {
      return _messBlocks.IsEmpty();
    }
    
    public void Accept(MessBlocks messBlocks)
    {
      _messBlocks = messBlocks;
    }

    public void Update(float deltaTime)
    {
      _messBlocks.Update(deltaTime);
      foreach (var block in _messBlocks.GetShouldDropBlocks())
      {
        _board.Set(block.ExpectedPosition.x, block.ExpectedPosition.y, block.Block);
        _messBlocks.Confirm(block);
      }
    }
  }
}