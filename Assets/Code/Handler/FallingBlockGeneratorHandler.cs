using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class FallingBlockGeneratorHandler : MonoBehaviour
  {
    public GameObject fallingBlockPrefab;

    private FallingBlockGenerator _generator;

    private void Start()
    {
      _generator = Game.FallingBlockGenerator;
      Game.BlockFallResolved += Create;
      Game.LevelEnd += () => Destroy(this);
      Game.InvokeBlockFallResolved();
    }

    private void Create()
    {
      var fallingBlock = Instantiate(fallingBlockPrefab);
      fallingBlock.GetComponent<FallingBlockHandler>().SetRequired(_generator.Next());
    }
  }
}