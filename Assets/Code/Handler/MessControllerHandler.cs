using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class MessControllerHandler : MonoBehaviour
  {
    
    public GameObject messBlockPrefab;
    private MessFallLogic _messFall;
    
    private void Start()
    {
      _messFall = new MessFallLogic(Game.Board);
      Game.MessFallEvent += InstantiateMess;
      Game.MessFallEvent += _messFall.Accept;
    }

    private void Update()
    {
      if (Game.State == Game.GameState.MessFalling)
      {
        _messFall.Update(Time.deltaTime);
        if (_messFall.IsDone()) Game.State = Game.GameState.BlockFalling;
      }
    }

    private void InstantiateMess(MessBlocks messBlocks)
    {
      foreach (var messBlock in messBlocks.Blocks)
      {
        var instantiate = Instantiate(messBlockPrefab);
        var handler = instantiate.GetComponent<MessHandler>();
        handler.sprite = Game.SpriteMapping[messBlock.Block];
        handler.messBlock = messBlock;
      }
    }
    
  }
}