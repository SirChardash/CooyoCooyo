using System.Collections;
using System.Collections.Generic;

namespace Code.Board
{
  public class CleaningResult
  {
    public readonly List<int[,]> BoardStates;

    public CleaningResult(List<int[,]> boardStates)
    {
      BoardStates = boardStates;
    }

    public bool AnythingHappened()
    {
      return BoardStates.Count > 0;
    }
  }
}