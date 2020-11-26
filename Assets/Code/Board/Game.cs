using System.Collections.Generic;
using Code.LevelSelect;
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
    public static Game ActiveGame;

    public const int BoardHeight = 10;
    public const int BoardWidth = 6;

    public GameState State = GameState.BlockFalling;
    public readonly BoardState Board = new BoardState(BoardHeight, BoardWidth);
    public readonly FallingBlockGenerator FallingBlockGenerator;
    public readonly Dictionary<Block, Sprite> SpriteMapping;

    public event MessFallEvent MessFall;
    public event BlockClearEvent BlockClear;
    public event LevelEndEvent LevelEnd;
    public event BlockFallResolved BlockFallResolved;
    public event BlockFallEvent BlockFall;
    public event PoofEvent Poof;

    public Game(GameSetup gameSetup)
    {
      SpriteMapping = gameSetup.SpriteMapping;
      FallingBlockGenerator = new FallingBlockGenerator(gameSetup.BlockCount, BoardWidth);
    }

    public void InvokeMessFall(MessBlocks messBlocks)
    {
      MessFall?.Invoke(messBlocks);
    }

    public void InvokeBlockClear(CleaningResult cleaningResult)
    {
      BlockClear?.Invoke(cleaningResult);
    }

    public void InvokeLevelEnd()
    {
      LevelEnd?.Invoke();
    }

    public void InvokeBlockFallResolved()
    {
      BlockFallResolved?.Invoke();
    }

    public void InvokeBlockFall(Vector2Int staticBlock, Vector2Int rotatingBlock, Block staticCode,
      Block rotatingCode)
    {
      BlockFall?.Invoke(staticBlock, rotatingBlock, staticCode, rotatingCode);
    }

    public void InvokePoof(CleaningResult cleaningResult)
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