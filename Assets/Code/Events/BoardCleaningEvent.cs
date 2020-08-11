using System;
using Code.Board;

namespace Code.Events
{
  //todo: see how event keyword works in C#
  public class BoardCleaningEvent : Exception
  {
    public CleaningResult CleaningResult;
  }
}