using Code.Board;
using UnityEngine;

namespace Code.Common
{
  public abstract class BoardUtils
  {
    public static Vector2 GetBoardCoordinates(int x, int y)
    {
      return new Vector2(Config.Scale * (x - 4), Config.Scale * (5 - y));
    }
    
    public static Vector2 GetBoardCoordinates(Vector2Int pos)
    {
      return new Vector2(Config.Scale * (pos.x - 4), Config.Scale * (5 - pos.y));
    }
  }
}