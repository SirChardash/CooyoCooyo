using System.Collections.Generic;
using Code.Board;
using Code.Common;
using UnityEngine;

namespace Code.Handler
{
  public class BoardHandler : MonoBehaviour
  {
    private BoardState _board;
    private BoardCleaner _cleaner;
    private SpriteRenderer[,] _renderBoard;
    private Dictionary<Block, Sprite> _spriteMapping;

    public GameObject blockPrefab;

    private int _boardHeight;
    private int _boardWidth;

    private CleaningResult _cleaningResult;
    private List<Block[,]> _animationStates;

    void Start()
    {
      _boardHeight = Game.BoardHeight;
      _boardWidth = Game.BoardWidth;
      _board = Game.Board;
      _cleaner = new BoardCleaner(_board.Height, _board.Width);
      _renderBoard = new SpriteRenderer[_boardHeight, _boardWidth];
      for (var x = 0; x < _boardWidth; x++)
      {
        for (var y = 0; y < _boardHeight; y++)
        {
          var block = Instantiate(blockPrefab);
          block.transform.position = BoardUtils.GetBoardCoordinates(x, y);
          _renderBoard[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      _spriteMapping = Game.SpriteMapping;

      Game.LevelEnd += () => Destroy(this);
      Game.Poof += HandlePoof;
      Game.BlockFall += HandleFallingBlockPlacement;
    }

    void Update()
    {
      if (_cleaningResult != null)
      {
        HandleCleaningAnimation(Time.deltaTime);
      }
    }

    private void HandlePoof(CleaningResult cleaningResult)
    {
      Game.State = Game.GameState.CleanResolution;
      _cleaningResult = cleaningResult;
      _animationStates = new List<Block[,]>(_cleaningResult.BoardStates);
    }

    private const float SlideDuration = 0.45f;
    private float _slideProgress;

    private void HandleCleaningAnimation(float deltaTime)
    {
      _slideProgress += deltaTime;
      if (_slideProgress >= SlideDuration)
      {
        _board.Set(_animationStates[0]);
        _animationStates.RemoveAt(0);
        _slideProgress -= SlideDuration;
        if (_animationStates.Count == 0)
        {
          Game.State = _cleaningResult.Penalty > 0 ? Game.GameState.MessFalling : Game.GameState.BlockFalling;
          Game.InvokeBlockClear(_cleaningResult);
          if (_cleaningResult.Penalty == 0) Game.InvokeBlockFallResolved();
          _animationStates = null;
          _cleaningResult = null;
        }
      }
    }
    
    private void HandleFallingBlockPlacement(Vector2Int staticBlock, Vector2Int rotatingBlock, Block staticCode,
      Block rotatingCode)
    {
      var staticBlockY = staticBlock.y;
      var rotatingBlockY = rotatingBlock.y;
      while (_board.IsEmpty(staticBlock.x, staticBlockY + 1)) staticBlockY++;
      while (_board.IsEmpty(rotatingBlock.x, rotatingBlockY + 1)) rotatingBlockY++;

      if (staticBlock.y < rotatingBlock.y) staticBlockY--;
      else if (staticBlock.y > rotatingBlock.y) rotatingBlockY--;

      _board.Set(staticBlock.x, staticBlockY, staticCode);
      _board.Set(rotatingBlock.x, rotatingBlockY, rotatingCode);

      var cleaningResult = _cleaner.TryClean(_board);
      if (cleaningResult.AnythingHappened())
      {
        Game.InvokePoof(cleaningResult);
      }
      else
      {
        Game.InvokeBlockFallResolved();
      }
    }

    private void OnGUI()
    {
      for (var x = 0; x < _boardWidth; x++)
      {
        for (var y = 0; y < _boardHeight; y++)
        {
          _renderBoard[y, x].sprite = _board.Get(x, y) != 0 ? _spriteMapping[_board.Get(x, y)] : null;
        }
      }
    }
  }
}