using System;
using Code.Board;

namespace Code.Events
{
  public class BoardCleaningEvent : Exception
  {
    public CleaningResult CleaningResult;
  }
}