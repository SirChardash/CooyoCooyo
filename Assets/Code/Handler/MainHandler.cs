using System.Collections.Generic;
using Code.Board;
using Code.Events;
using UnityEngine;

namespace Code.Handler
{
  public class MainHandler : MonoBehaviour
  {
    private const float Scale = 0.8f;

    private GameLogic _game;
    private BoardState _board;
    private SpriteRenderer[,] _renderBoard;
    private Dictionary<Block, Sprite> _spriteMapping;

    public GameObject blockPrefab;
    private SpriteRenderer _fallingBlockStaticRenderer;
    private SpriteRenderer _fallingBlockRotatingRenderer;
    private Transform _fallingBlockStaticTransform;
    private Transform _fallingBlockRotatingTransform;


    private int _boardHeight;
    private int _boardWidth;

    private CleaningResult _cleaningResult;
    private List<Block[,]> _animationStates;

    void Start()
    {
      _boardHeight = Game.BoardHeight;
      _boardWidth = Game.BoardWidth;
      _board = Game.Board;
      _game = new GameLogic(_board);
      _renderBoard = new SpriteRenderer[_boardHeight, _boardWidth];
      for (var x = 0; x < _boardWidth; x++)
      {
        for (var y = 0; y < _boardHeight; y++)
        {
          var block = Instantiate(blockPrefab);
          block.transform.position = GetBoardCoordinates(x, y);
          _renderBoard[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      var staticBlock = Instantiate(blockPrefab);
      var rotatingBlock = Instantiate(blockPrefab);
      _fallingBlockStaticRenderer = staticBlock.GetComponent<SpriteRenderer>();
      _fallingBlockRotatingRenderer = rotatingBlock.GetComponent<SpriteRenderer>();
      _fallingBlockStaticTransform = staticBlock.transform;
      _fallingBlockRotatingTransform = rotatingBlock.transform;

      _spriteMapping = Game.SpriteMapping;

      Game.LevelEnd += () => Destroy(this);
    }

    void Update()
    {
      if (_cleaningResult != null)
      {
        HandleCleaningAnimation(Time.deltaTime);
        return;
      }

      try
      {
        if (Game.State != Game.GameState.MessFalling) _game.Update(Time.deltaTime);
      }
      catch (BoardCleaningEvent e)
      {
        Game.State = Game.GameState.CleanResolution;
        _cleaningResult = e.CleaningResult;
        _animationStates = new List<Block[,]>(_cleaningResult.BoardStates);
        _fallingBlockRotatingRenderer.sprite = null;
        _fallingBlockStaticRenderer.sprite = null;
      }
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
          _animationStates = null;
          _cleaningResult = null;
        }
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

      if (_cleaningResult != null) return;

      RenderFallingBlock();
    }


    private void RenderFallingBlock()
    {
      var fallingBlock = _game.FallingBlock;
      var staticBlock = _game.FallingBlock.StaticBlock;
      var rotatingBlock = fallingBlock.RotatingBlock;

      if (_board.IsEmpty(staticBlock.x, staticBlock.y))
      {
        _fallingBlockStaticRenderer.sprite = _spriteMapping[fallingBlock.StaticCode];
        _fallingBlockStaticTransform.position = FallingBlockProgress(
          staticBlock,
          fallingBlock.GetBlockProgress(),
          fallingBlock.Orientation == FallingBlock.BlockOrientation.Down
        );
      }

      if (rotatingBlock.y >= 0 && _board.IsEmpty(rotatingBlock.x, rotatingBlock.y))
      {
        _fallingBlockRotatingRenderer.sprite = _spriteMapping[fallingBlock.RotatingCode];
        _fallingBlockRotatingTransform.position = FallingBlockProgress(
          rotatingBlock,
          fallingBlock.GetBlockProgress(),
          fallingBlock.Orientation == FallingBlock.BlockOrientation.Up
        );
      }
    }

    private Vector2 FallingBlockProgress(Vector2Int block, float fallingProgress, bool topFallingBlock)
    {
      var animationProgress = fallingProgress * fallingProgress;

      var startPos = GetBoardCoordinates(block.x, block.y);
      var endPos = GetBoardCoordinates(block.x, block.y + 1);
      var trueProgress =
        _board.IsEmpty(block.x, block.y + 1) &&
        (!topFallingBlock || (_board.IsEmpty(block.x, block.y + 2)))
          ? animationProgress
          : 0;
      return Vector2.Lerp(startPos, endPos, t: trueProgress);
    }

    private static Vector2 GetBoardCoordinates(int x, int y)
    {
      return new Vector2(Scale * (x - 4), Scale * (5 - y));
    }
  }
}