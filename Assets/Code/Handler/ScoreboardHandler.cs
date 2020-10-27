using System.Collections.Generic;
using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class ScoreboardHandler : MonoBehaviour
  {
    private Scoreboard _scoreboard;
    private readonly SpriteRenderer[,] _renderObjectives = new SpriteRenderer[5, 5];
    private Dictionary<Block, Sprite> _spriteMapping;
    private FallingBlockGenerator _fallingBlockGenerator;
    
    public GameObject blockPrefab;
    public GameObject controller;

    private void Start()
    {
      _spriteMapping = Game.SpriteMapping;
      _fallingBlockGenerator = Game.FallingBlockGenerator;
      var levelObjectives = new List<LevelObjective>
      {
        new LevelObjective(new Dictionary<Block, int> {{Block.Block1, 2}, {Block.Block2, 3}, {Block.Block3, 1}}),
        new LevelObjective(new Dictionary<Block, int> {{Block.Block1, 2}})
      };
      _scoreboard = new Scoreboard(levelObjectives);
      var basePosition = GetComponent<Transform>().position;
      for (var x = 0; x < 5; x++)
      {
        for (var y = 0; y < 5; y++)
        {
          var block = Instantiate(blockPrefab, transform, true);
          block.name = $"Block#{x}-{y}";
          block.transform.position = new Vector2(basePosition.x + Config.Scale * x, basePosition.y + Config.Scale * y);
          _renderObjectives[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      Game.BlockClear += UpdateScoreboard;
      DrawObjectives();
    }

    private void UpdateScoreboard(CleaningResult cleaningResult)
    {
      var penalty = _scoreboard.TryContribute(cleaningResult.Poofs);
      var messBlocks = _fallingBlockGenerator.Mess(penalty,
        new BoardState(cleaningResult.BoardStates[cleaningResult.BoardStates.Count - 1]));
      cleaningResult.Penalty = penalty;
      if (penalty > 0) Game.InvokeMessFall(messBlocks);
      DrawObjectives();
      
      if (_scoreboard.IsComplete()) End();
    }

    private void DrawObjectives()
    {
      for (var i = 0; i < 5; i++)
      {
        for (var j = 0; j < 5; j++)
        {
          _renderObjectives[i, j].sprite = null;
        }
      }

      var row = 0;
      if (_scoreboard.GetCurrentObjective() == null) return;
      foreach (var objective in _scoreboard.GetCurrentObjective().Objectives)
      {
        for (var i = 0; i < objective.Value; i++)
        {
          _renderObjectives[i, row].sprite = _spriteMapping[objective.Key];
        }

        row++;
      }
    }

    private void End()
    {
      Game.InvokeLevelEnd();
      Debug.Log("objectives complete");
      Destroy(controller);
    }
  }
}