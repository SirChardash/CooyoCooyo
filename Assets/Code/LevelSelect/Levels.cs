using System.Collections.Generic;
using Code.Board;
using UnityEngine;

namespace Code.LevelSelect
{
  public static class Levels
  {
    public static readonly Dictionary<string, GameSetup> Predefined = new Dictionary<string, GameSetup>
    {
      {
        "testLevel", new GameSetup(4, new Dictionary<Block, Sprite>
        {
          {Block.Block1, Resources.Load("Images/Apple_01", typeof(Sprite)) as Sprite},
          {Block.Block2, Resources.Load("Images/Cauliflower_01", typeof(Sprite)) as Sprite},
          {Block.Block3, Resources.Load("Images/Radish_01", typeof(Sprite)) as Sprite},
          {Block.Block4, Resources.Load("Images/Red_current_01", typeof(Sprite)) as Sprite},
          {Block.Poof1, Resources.Load("Images/SpellBook03_02", typeof(Sprite)) as Sprite},
          {Block.Mess, Resources.Load("Images/SpellBook03_103", typeof(Sprite)) as Sprite}
        })
      }
    };
  }
}