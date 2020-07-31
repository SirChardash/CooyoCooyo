using System.Collections;
using System.Collections.Generic;

namespace Code.Board
{
  public class CleaningResult
  {
    public readonly List<Block[,]> BoardStates;

    public CleaningResult(List<Block[,]> boardStates)
    {
      BoardStates = boardStates;
    }

    public bool AnythingHappened()
    {
      return BoardStates.Count > 0;
    }
  }
}