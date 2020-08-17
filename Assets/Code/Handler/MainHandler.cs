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
    private Scoreboard _scoreboard;
    private readonly SpriteRenderer[,] _renderObjectives = new SpriteRenderer[5, 5];

    public GameObject blockPrefab;
    private SpriteRenderer _fallingBlockStaticRenderer;
    private SpriteRenderer _fallingBlockRotatingRenderer;
    private Transform _fallingBlockStaticTransform;
    private Transform _fallingBlockRotatingTransform;


    private const int BoardHeight = 10;
    private const int BoardWidth = 6;
    private const int BlockCount = 4;

    private CleaningResult _cleaningResult;

    void Start()
    {
      var levelObjectives = new List<LevelObjective>
      {
        new LevelObjective(new Dictionary<Block, int> {{Block.Block1, 2}, {Block.Block2, 3}, {Block.Block3, 1}}),
        new LevelObjective(new Dictionary<Block, int> {{Block.Block1, 2}})
      };

      _scoreboard = new Scoreboard(levelObjectives);
      _game = new GameLogic(BoardHeight, BoardWidth, BlockCount, _scoreboard);
      _board = _game.BoardState;
      _renderBoard = new SpriteRenderer[BoardHeight, BoardWidth];
      for (var x = 0; x < BoardWidth; x++)
      {
        for (var y = 0; y < BoardHeight; y++)
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

      var scoreboardPosition = new Vector2(Scale * 4, Scale * 2);
      for (var x = 0; x < 5; x++)
      {
        for (var y = 0; y < 5; y++)
        {
          var block = Instantiate(blockPrefab);
          block.transform.position = new Vector2(scoreboardPosition.x + Scale * x, scoreboardPosition.y + Scale * y);
          _renderObjectives[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      _spriteMapping = new Dictionary<Block, Sprite>
      {
        {Block.Block1, Resources.Load("Images/Apple_01", typeof(Sprite)) as Sprite},
        {Block.Block2, Resources.Load("Images/Cauliflower_01", typeof(Sprite)) as Sprite},
        {Block.Block3, Resources.Load("Images/Radish_01", typeof(Sprite)) as Sprite},
        {Block.Block4, Resources.Load("Images/Red_current_01", typeof(Sprite)) as Sprite},
        {Block.Poof1, Resources.Load("Images/SpellBook03_02", typeof(Sprite)) as Sprite},
        {Block.Mess, Resources.Load("Images/SpellBook03_103", typeof(Sprite)) as Sprite},
      };
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
        _game.Update(Time.deltaTime);
      }
      catch (BoardCleaningEvent e)
      {
        _cleaningResult = e.CleaningResult;
        _fallingBlockRotatingRenderer.sprite = null;
        _fallingBlockStaticRenderer.sprite = null;
      }
      catch (GameEndEvent)
      {
        Destroy(this);
      }
    }

    private const float SlideDuration = 0.45f;
    private float _slideProgress;

    private void HandleCleaningAnimation(float deltaTime)
    {
      _slideProgress += deltaTime;
      if (_slideProgress >= SlideDuration)
      {
        _board.Set(_cleaningResult.BoardStates[0]);
        _cleaningResult.BoardStates.RemoveAt(0);
        _cleaningResult = _cleaningResult.BoardStates.Count > 0 ? _cleaningResult : null;
        _slideProgress -= SlideDuration;
      }
    }

    private void OnGUI()
    {
      RenderScoreboard();

      for (var x = 0; x < BoardWidth; x++)
      {
        for (var y = 0; y < BoardHeight; y++)
        {
          _renderBoard[y, x].sprite = _board.Get(x, y) != 0 ? _spriteMapping[_board.Get(x, y)] : null;
        }
      }

      if (_cleaningResult != null) return;

      RenderFallingBlock();

      _game.MessBlocks?.Blocks.ForEach(block =>
      {
        _renderBoard[block.Position.y, block.Position.x].sprite = _spriteMapping[block.Block];
      });
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

    private void RenderScoreboard()
    {
      for (var i = 0; i < 5; i++)
      {
        for (var j = 0; j < 5; j++)
        {
          _renderObjectives[i, j].sprite = null;
        }
      }

      var row = 0;
      if (_scoreboard.GetCurrentObjective() == null) return;
      foreach (var objective in _scoreboard.GetCurrentObjective().Objectives)
      {
        for (var i = 0; i < objective.Value; i++)
        {
          _renderObjectives[i, row].sprite = _spriteMapping[objective.Key];
        }

        row++;
      }
    }

    private static Vector2 GetBoardCoordinates(int x, int y)
    {
      return new Vector2(Scale * (x - 4), Scale * (5 - y));
    }
  }
}