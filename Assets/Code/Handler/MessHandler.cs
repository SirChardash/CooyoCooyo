using Code.Board;
using Code.Common;
using UnityEngine;

namespace Code.Handler
{
  public class MessHandler : MonoBehaviour
  {
    private MessBlocks.MessBlock _messBlock;
    private Sprite _sprite;
    private Transform _transform;

    public void SetRequired(MessBlocks.MessBlock messBlock, Sprite sprite)
    {
      _messBlock = messBlock;
      _sprite = sprite;
    }
    
    private void Start()
    {
      GetComponent<SpriteRenderer>().sprite = _sprite;
      _transform = GetComponent<Transform>();
      _transform.position = BoardUtils.GetBoardCoordinates(_messBlock.ExpectedPosition.x, 1);
    }

    private void Update()
    {
      if (_messBlock.ShouldDrop()) Destroy(gameObject);
    }

    private void OnGUI()
    {
      _transform.position = Vector2.Lerp(BoardUtils.GetBoardCoordinates(
          _messBlock.ExpectedPosition.x, 1),
        BoardUtils.GetBoardCoordinates(_messBlock.ExpectedPosition.x, _messBlock.ExpectedPosition.y), t:
        _messBlock.GetBlockProgress() * _messBlock.GetBlockProgress()
      );
    }
    
  }
}