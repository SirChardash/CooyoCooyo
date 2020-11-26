using System.Collections.Generic;
using Code.Board;
using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public class BoardHandler : MonoBehaviour, ICoordinates
  {
    private BoardState _board;
    private BoardCleaner _cleaner;
    private SpriteRenderer[,] _renderBoard;
    private Dictionary<Block, Sprite> _spriteMapping;

    public GameObject blockPrefab;
    public Transform playFieldTransform;
    public SpriteRenderer playFieldRenderer;
    public Transform boardTransform;
    public new Camera camera;
    public SpriteRenderer boardBackgroundRenderer;
    public CameraChangeListener cameraChangeListener;
    
    private int _boardHeight;
    private int _boardWidth;
    private float _blockOffsetX;
    private float _blockOffsetY;
    private Vector2 _startingPosition;

    private CleaningResult _cleaningResult;
    private List<Block[,]> _animationStates;

    void Start()
    {
      _boardHeight = Game.BoardHeight;
      _boardWidth = Game.BoardWidth;

      RefreshPosition();
      
      _board = Game.ActiveGame.Board;
      _cleaner = new BoardCleaner(_board.Height, _board.Width);
      _renderBoard = new SpriteRenderer[_boardHeight, _boardWidth];
      for (var x = 0; x < _boardWidth; x++)
      {
        for (var y = 0; y < _boardHeight; y++)
        {
          var block = Instantiate(blockPrefab, playFieldTransform, false);
          block.name = $"Block#{y}-{x}";
          block.transform.position = GetBoardCoordinates(x, y);
          _renderBoard[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      _spriteMapping = Game.ActiveGame.SpriteMapping;

      Game.ActiveGame.LevelEnd += () => Destroy(this);
      Game.ActiveGame.Poof += HandlePoof;
      Game.ActiveGame.BlockFall += HandleFallingBlockPlacement;
      cameraChangeListener.CameraChanged += RefreshPosition;
      cameraChangeListener.CameraChanged += () => gameObject.SetActive(true);
    }

    void Update()
    {
      if (_cleaningResult != null)
      {
        HandleCleaningAnimation(Time.deltaTime);
      }
    }

    private void RefreshPosition()
    {
      boardTransform.localScale = BoardScale();
      boardTransform.position = BoardPosition();
      _startingPosition = FallingBlockStartingPosition();
    }
    
    private void HandlePoof(CleaningResult cleaningResult)
    {
      Game.ActiveGame.State = Game.GameState.CleanResolution;
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
          Game.ActiveGame.State = _cleaningResult.Penalty > 0 ? Game.GameState.MessFalling : Game.GameState.BlockFalling;
          Game.ActiveGame.InvokeBlockClear(_cleaningResult);
          if (_cleaningResult.Penalty == 0) Game.ActiveGame.InvokeBlockFallResolved();
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

      _board.Set(staticBlock.x, staticBlockY, staticCode);
      _board.Set(rotatingBlock.x, rotatingBlockY, rotatingCode);

      var cleaningResult = _cleaner.TryClean(_board);
      if (cleaningResult.AnythingHappened())
      {
        Game.ActiveGame.InvokePoof(cleaningResult);
      }
      else
      {
        Game.ActiveGame.InvokeBlockFallResolved();
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

    public Vector2 GetBoardCoordinates(int x, int y)
    {
      var localScale = boardTransform.localScale;
      return new Vector2(
        (_startingPosition.x + 1.05f * _blockOffsetX * x) * localScale.x,
        (_startingPosition.y - 1.05f * _blockOffsetY * y) * localScale.y);
    }

    public Vector2 GetBoardCoordinates(Vector2Int pos)
    {
      return GetBoardCoordinates(pos.x, pos.y);
    }

    private Vector3 BoardPosition()
    {
      var screenAspect = Screen.width / (float) Screen.height;
      var cameraHeight = camera.orthographicSize;
      var boardHalfSize = boardTransform.localScale * boardBackgroundRenderer.size / 2;
      return (Vector2) camera.transform.position
             + new Vector2(-cameraHeight * screenAspect, cameraHeight) * 0.9f
             + new Vector2(boardHalfSize.x, -boardHalfSize.y);
    }

    private Vector3 BoardScale()
    {
      var displayWidth = 2 * camera.orthographicSize * Screen.width / Screen.height;
      var scale = 0.5f * displayWidth / boardBackgroundRenderer.size.x;
      return new Vector3(scale, scale, 1);
    }

    private Vector3 FallingBlockStartingPosition()
    {
      var size = playFieldRenderer.size;
      var position = playFieldTransform.position;
      var localScale = boardTransform.localScale;
      position.Scale(new Vector3(1 / localScale.x, 1 / localScale.y)); // I don't know why

      _blockOffsetX = size.x / (_boardWidth + (_boardWidth - 1) / 20f);
      _blockOffsetY = size.y / (_boardHeight + (_boardHeight - 1) / 20f);

      return position
             + new Vector3(-size.x / 2, size.y / 2, position.z)
             + new Vector3(_blockOffsetX / 2, -_blockOffsetY / 2, position.z);
    }
  }
}