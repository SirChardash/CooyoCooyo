namespace Code.Board
{
  public class MessFallLogic
  {

    private readonly BoardState _board;
    public MessBlocks MessBlocks;
    
    public MessFallLogic(BoardState board)
    {
      _board = board;
    }

    public bool IsDone()
    {
      return MessBlocks.IsEmpty();
    }
    
    public void Accept(MessBlocks messBlocks)
    {
      MessBlocks = messBlocks;
    }

    public void Update(float deltaTime)
    {
      MessBlocks.Update(deltaTime);
      foreach (var block in MessBlocks.GetShouldDropBlocks())
      {
        _board.Set(block.ExpectedPosition.x, block.ExpectedPosition.y, block.Block);
        MessBlocks.Confirm(block);
      }
    }
  }
}