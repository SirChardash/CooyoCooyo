using Code.Board;
using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public class MessControllerHandler : MonoBehaviour
  {
    public GameObject messBlockPrefab;
    public BoardHandler boardHandler;
    public Transform boardTransform;

    private MessFallLogic _messFall;

    private void Start()
    {
      _messFall = new MessFallLogic(Game.ActiveGame.Board);
      Game.ActiveGame.MessFall += InstantiateMess;
      Game.ActiveGame.MessFall += _messFall.Accept;
      Game.ActiveGame.MessFall += Resume;
      enabled = false;
    }

    private void Update()
    {
      if (Game.ActiveGame.State == Game.GameState.MessFalling)
      {
        _messFall.Update(Time.deltaTime);
        if (_messFall.IsDone())
        {
          Game.ActiveGame.State = Game.GameState.BlockFalling;
          Game.ActiveGame.InvokeBlockFallResolved();
          enabled = false;
        }
      }
    }

    private void InstantiateMess(MessBlocks messBlocks)
    {
      var i = 0;
      foreach (var messBlock in messBlocks.Blocks)
      {
        var instantiate = Instantiate(messBlockPrefab, boardTransform.transform, false);
        instantiate.name = $"Mess#{++i}";
        var handler = instantiate.GetComponent<MessHandler>();
        handler.SetRequired(messBlock, Game.ActiveGame.SpriteMapping[messBlock.Block], boardHandler);
      }
    }

    private void Resume(MessBlocks messBlocks)
    {
      Game.ActiveGame.State = Game.GameState.MessFalling;
      enabled = true;
    }
  }
}