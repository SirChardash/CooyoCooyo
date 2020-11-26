using Code.Board;
using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public class MessHandler : MonoBehaviour
  {
    private MessBlocks.MessBlock _messBlock;
    private Sprite _sprite;
    private ICoordinates _coordinates;
    private Transform _transform;

    public void SetRequired(MessBlocks.MessBlock messBlock, Sprite sprite, ICoordinates coordinates)
    {
      _messBlock = messBlock;
      _sprite = sprite;
      _coordinates = coordinates;
    }
    
    private void Start()
    {
      GetComponent<SpriteRenderer>().sprite = _sprite;
      _transform = GetComponent<Transform>();
      _transform.position = _coordinates.GetBoardCoordinates(_messBlock.ExpectedPosition.x, 1);
    }

    private void Update()
    {
      if (_messBlock.ShouldDrop()) Destroy(gameObject);
    }

    private void OnGUI()
    {
      _transform.position = Vector2.Lerp(_coordinates.GetBoardCoordinates(
          _messBlock.ExpectedPosition.x, 1),
        _coordinates.GetBoardCoordinates(_messBlock.ExpectedPosition.x, _messBlock.ExpectedPosition.y), t:
        _messBlock.GetBlockProgress() * _messBlock.GetBlockProgress()
      );
    }
    
  }
}