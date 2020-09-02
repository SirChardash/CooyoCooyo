using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class MessHandler : MonoBehaviour
  {
    private const float Scale = 0.8f;
    
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
      _transform.position = GetBoardCoordinates(_messBlock.ExpectedPosition.x, 1);
    }

    private void Update()
    {
      if (_messBlock.ShouldDrop()) Destroy(gameObject);
    }

    private void OnGUI()
    {
      _transform.position = Vector2.Lerp(GetBoardCoordinates(
          _messBlock.ExpectedPosition.x, 1),
        GetBoardCoordinates(_messBlock.ExpectedPosition.x, _messBlock.ExpectedPosition.y), t:
        _messBlock.GetBlockProgress() * _messBlock.GetBlockProgress()
      );
    }
    
    private static Vector2 GetBoardCoordinates(int x, int y)
    {
      return new Vector2(Scale * (x - 4), Scale * (5 - y));
    }
  }
}