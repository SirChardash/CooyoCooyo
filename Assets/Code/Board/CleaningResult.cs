using System.Collections.Generic;

namespace Code.Board
{
  public class CleaningResult
  {
    public readonly List<Block[,]> BoardStates;
    public readonly List<Poof> Poofs;
    public int Penalty;
    
    public CleaningResult(List<Block[,]> boardStates, List<Poof> poofs)
    {
      BoardStates = boardStates;
      Poofs = poofs;
    }

    public bool AnythingHappened()
    {
      return BoardStates.Count > 0;
    }
  }
}