namespace Code.Common
{
  public static class Array2DUtils
  {
    public static void Or(bool[,] modified, bool[,] modifier)
    {
      for (var x = 0; x < modified.GetLength(1); x++)
      {
        for (var y = 0; y < modified.GetLength(0); y++)
        {
          modified[y, x] |= modifier[y, x];
        }
      }
    }
  }
}