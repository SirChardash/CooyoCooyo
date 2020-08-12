using System.Collections.Generic;

namespace Code.Board
{
  public class LevelObjective
  {
    public readonly Dictionary<Block, int> Objectives;

    public LevelObjective(Dictionary<Block, int> objectives)
    {
      Objectives = objectives;
    }
    
    public bool TryContribute(Block block)
    {
      if (!Objectives.ContainsKey(block)) return false;
      Objectives[block]--;
      if (Objectives[block] == 0) Objectives.Remove(block);
      return true;
    }

    public bool IsComplete()
    {
      return Objectives.Count == 0;
    }
  }
}