using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class FallingBlockGeneratorHandler : MonoBehaviour
  {
    public GameObject fallingBlockPrefab;

    private FallingBlockGenerator _generator;
    private BoardState _board;

    private void Start()
    {
      _generator = Game.FallingBlockGenerator;
      _board = Game.Board;
      Game.BlockFallResolved += Create;
      Game.LevelEnd += () => Game.BlockFallResolved -= Create;
      Game.InvokeBlockFallResolved();
    }

    private void Create()
    {
      if (!_board.IsEmpty(_generator.StartingX, 0)
          || !_board.IsEmpty(_generator.StartingX, 1))
      {
        Game.InvokeLevelEnd();
        return;
      }

      var fallingBlock = Instantiate(fallingBlockPrefab);
      fallingBlock.GetComponent<FallingBlockHandler>().SetRequired(_generator.Next());
    }
  }
}