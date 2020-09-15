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
          enabled = false;
        }
      }
    }

    private void InstantiateMess(MessBlocks messBlocks)
    {
      foreach (var messBlock in messBlocks.Blocks)
      {
        var instantiate = Instantiate(messBlockPrefab);
        var handler = instantiate.GetComponent<MessHandler>();
        handler.SetRequired(messBlock, Game.SpriteMapping[messBlock.Block]);
      }
    }
    
    private void Resume(MessBlocks messBlocks)
    {
      Game.State = Game.GameState.MessFalling;
      enabled = true;
    }
    
  }
}