namespace Code.Board
{
  public class MessFallLogic
  {

    private BoardState _board;
    public MessBlocks MessBlocks;
    
    public MessFallLogic(BoardState board)
    {
      _board = board;
    }
    
    public void Process(MessBlocks messBlocks)
    {
      MessBlocks = messBlocks;
    }

    public bool IsActive()
    {
      return MessBlocks != null;
    }
    
    public void Update(float deltaTime)
    {
      MessBlocks.Update(deltaTime);
      foreach (var block in MessBlocks.GetShouldDropBlocks())
      {
        _board.Set(block.ExpectedPosition.x, block.ExpectedPosition.y, block.Block);
        MessBlocks.Confirm(block);
      }

      if (MessBlocks.IsEmpty()) MessBlocks = null;
    }
  }
}