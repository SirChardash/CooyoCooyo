using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class MessControllerHandler : MonoBehaviour
  {
    public GameObject messBlockPrefab;
    public BoardHandler boardHandler;
    public Transform boardTransform;

    private MessFallLogic _messFall;

    private void Start()
    {
      _messFall = new MessFallLogic(Game.Board);
      Game.MessFall += InstantiateMess;
      Game.MessFall += _messFall.Accept;
      Game.MessFall += Resume;
      enabled = false;
    }

    private void Update()
    {
      if (Game.State == Game.GameState.MessFalling)
      {
        _messFall.Update(Time.deltaTime);
        if (_messFall.IsDone())
        {
          Game.State = Game.GameState.BlockFalling;
          Game.InvokeBlockFallResolved();
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
        handler.SetRequired(messBlock, Game.SpriteMapping[messBlock.Block], boardHandler);
      }
    }

    private void Resume(MessBlocks messBlocks)
    {
      Game.State = Game.GameState.MessFalling;
      enabled = true;
    }
  }
}