using Code.Board;
using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public class FallingBlockGeneratorHandler : MonoBehaviour
  {
    public GameObject fallingBlockPrefab;
    public BoardHandler boardHandler;
    public InputHandler inputHandler;
    public Transform boardTransform;

    private FallingBlockGenerator _generator;
    private BoardState _board;

    private void Start()
    {
      _generator = Game.ActiveGame.FallingBlockGenerator;
      _board = Game.ActiveGame.Board;
      Game.ActiveGame.BlockFallResolved += Create;
      Game.ActiveGame.InvokeBlockFallResolved();
    }

    private void Create()
    {
      if (!_board.IsEmpty(_generator.StartingX, 0)
          || !_board.IsEmpty(_generator.StartingX, 1))
      {
        Debug.Log("level failed");
        Game.ActiveGame.InvokeLevelEnd();
        return;
      }

      var fallingBlock = Instantiate(fallingBlockPrefab, boardTransform.transform, false);
      fallingBlock.name = "FallingBlock";
      fallingBlock.GetComponent<FallingBlockHandler>().SetRequired(_generator.Next(), boardHandler, inputHandler);
    }
  }
}