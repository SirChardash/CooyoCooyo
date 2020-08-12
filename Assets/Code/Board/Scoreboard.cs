using System.Collections.Generic;

namespace Code.Board
{
  public class Scoreboard
  {
    private List<LevelObjective> _objectives;
    public int Score;

    public Scoreboard(List<LevelObjective> objectives)
    {
      _objectives = objectives;
    }

    public bool IsComplete()
    {
      return _objectives.Count == 0;
    }

    public LevelObjective GetCurrentObjective()
    {
      return IsComplete() ?  null : _objectives[0];
    }

    public int TryContribute(List<Poof> poofs)
    {
      var penalty = 0;
      poofs.ForEach(poof =>
      {
        if (!_objectives[0].TryContribute(poof.Block))
        {
          penalty++;
        }
      });

      if (_objectives[0].IsComplete())
      {
        _objectives.RemoveAt(0);
      }

      return penalty;
    }
  }
}