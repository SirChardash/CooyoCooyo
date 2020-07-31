using UnityEngine;

namespace Code.Common
{
  public static class Array2DUtils
  {
    private static void Print(int[,] rawNodes)
    {
      var rowLength = rawNodes.GetLength(0);
      var colLength = rawNodes.GetLength(1);
      var arrayString = "";
      for (var i = 0; i < rowLength; i++)
      {
        for (var j = 0; j < colLength; j++)
        {
          arrayString += $"{rawNodes[i, j]} ";
        }

        arrayString += System.Environment.NewLine + System.Environment.NewLine;
      }

      Debug.Log(arrayString);
    }
    
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