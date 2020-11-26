using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public interface ICoordinates
  {
    Vector2 GetBoardCoordinates(int x, int y);
    Vector2 GetBoardCoordinates(Vector2Int pos);
  }
}