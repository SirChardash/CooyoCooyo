using Code.Board;
using UnityEngine;

namespace Code.Handler
{
  public class MessHandler : MonoBehaviour
  {
    private const float Scale = 0.8f;
    
    public MessBlocks.MessBlock messBlock;
    public Sprite sprite;
    private Transform _transform;

    private void Start()
    {
      GetComponent<SpriteRenderer>().sprite = sprite;
      _transform = GetComponent<Transform>();
      _transform.position = GetBoardCoordinates(messBlock.ExpectedPosition.x, 1);
    }

    private void Update()
    {
      if (messBlock.ShouldDrop()) Destroy(gameObject);
    }

    private void OnGUI()
    {
      _transform.position = Vector2.Lerp(GetBoardCoordinates(
          messBlock.ExpectedPosition.x, 1),
        GetBoardCoordinates(messBlock.ExpectedPosition.x, messBlock.ExpectedPosition.y), t:
        messBlock.GetBlockProgress() * messBlock.GetBlockProgress()
      );
    }
    
    private static Vector2 GetBoardCoordinates(int x, int y)
    {
      return new Vector2(Scale * (x - 4), Scale * (5 - y));
    }
  }
}