using System.Collections.Generic;
using UnityEngine;

namespace Code.Board
{
  public delegate void MessFallEvent(MessBlocks messBlocks);

  public delegate void BlockClearEvent(CleaningResult cleaningResult);

  public delegate void LevelEndEvent();

  public class Game
  {
    private const int BoardHeight = 10;
    private const int BoardWidth = 6;
    private const int BlockCount = 4;

    public static GameState State = GameState.BlockFalling;
    public static readonly BoardState Board = new BoardState(BoardHeight, BoardWidth);
    public static readonly FallingBlockGenerator FallingBlockGenerator = new FallingBlockGenerator(BlockCount, BoardWidth);

    public static readonly Dictionary<Block, Sprite> SpriteMapping = new Dictionary<Block, Sprite>
    {
      {Block.Block1, Resources.Load("Images/Apple_01", typeof(Sprite)) as Sprite},
      {Block.Block2, Resources.Load("Images/Cauliflower_01", typeof(Sprite)) as Sprite},
      {Block.Block3, Resources.Load("Images/Radish_01", typeof(Sprite)) as Sprite},
      {Block.Block4, Resources.Load("Images/Red_current_01", typeof(Sprite)) as Sprite},
      {Block.Poof1, Resources.Load("Images/SpellBook03_02", typeof(Sprite)) as Sprite},
      {Block.Mess, Resources.Load("Images/SpellBook03_103", typeof(Sprite)) as Sprite},
    };


    public static event MessFallEvent MessFallEvent;
    public static event BlockClearEvent BlockClearEvent;
    public static event LevelEndEvent LevelEndEvent;

    public static void InvokeMessFallEvent(MessBlocks messBlocks)
    {
      MessFallEvent?.Invoke(messBlocks);
    }

    public static void InvokeBlockClearEvent(CleaningResult cleaningResult)
    {
      BlockClearEvent?.Invoke(cleaningResult);
    }
    
    public static void InvokeLevelEndEvent()
    {
      LevelEndEvent?.Invoke();
    }

    public enum GameState
    {
      MessFalling,
      BlockFalling,
      CleanResolution
    }
  }
}