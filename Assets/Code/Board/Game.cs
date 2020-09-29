using System.Collections.Generic;
using UnityEngine;

namespace Code.Board
{
  public delegate void MessFallEvent(MessBlocks messBlocks);

  public delegate void BlockClearEvent(CleaningResult cleaningResult);

  public delegate void LevelEndEvent();

  public delegate void BlockFallResolved();

  public delegate void BlockFallEvent(Vector2Int staticBlock, Vector2Int rotatingBlock, Block staticCode,
    Block rotatingCode);

  public delegate void PoofEvent(CleaningResult cleaningResult);

  public class Game
  {
    public const int BoardHeight = 10;
    public const int BoardWidth = 6;
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


    public static event MessFallEvent MessFall;
    public static event BlockClearEvent BlockClear;
    public static event LevelEndEvent LevelEnd;
    public static event BlockFallResolved BlockFallResolved;
    public static event BlockFallEvent BlockFall;
    public static event PoofEvent Poof;

    public static void InvokeMessFall(MessBlocks messBlocks)
    {
      MessFall?.Invoke(messBlocks);
    }

    public static void InvokeBlockClear(CleaningResult cleaningResult)
    {
      BlockClear?.Invoke(cleaningResult);
    }

    public static void InvokeLevelEnd()
    {
      LevelEnd?.Invoke();
    }

    public static void InvokeBlockFallResolved()
    {
      BlockFallResolved?.Invoke();
    }

    public static void InvokeBlockFall(Vector2Int staticBlock, Vector2Int rotatingBlock, Block staticCode,
      Block rotatingCode)
    {
      BlockFall?.Invoke(staticBlock, rotatingBlock, staticCode, rotatingCode);
    }

    public static void InvokePoof(CleaningResult cleaningResult)
    {
      Poof?.Invoke(cleaningResult);
    }

    public enum GameState
    {
      MessFalling,
      BlockFalling,
      CleanResolution
    }
  }
}