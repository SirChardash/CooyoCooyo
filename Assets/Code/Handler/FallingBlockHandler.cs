using System.Collections.Generic;
using Code.Board;
using Code.Common;
using UnityEngine;

namespace Code.Handler
{
  public class FallingBlockHandler : MonoBehaviour
  {
    private readonly Vector2Int _oneDown = new Vector2Int(0, 1);

    public Transform staticBlockTransform;
    public Transform rotatingBlockTransform;
    public SpriteRenderer staticBlockRenderer;
    public SpriteRenderer rotatingBlockRenderer;

    private FallingBlock _fallingBlock;
    private BoardState _board;
    private Dictionary<Block, Sprite> _spriteMapping;


    public void SetRequired(FallingBlock fallingBlock)
    {
      _fallingBlock = fallingBlock;
      _board = Game.Board;
      _spriteMapping = Game.SpriteMapping;
      staticBlockTransform.position = BoardUtils.GetBoardCoordinates(_fallingBlock.StaticBlock);
    }

    private void Update()
    {
      if ((!_board.IsEmpty(_fallingBlock.StaticBlock.x, _fallingBlock.StaticBlock.y + 1)
           || !_board.IsEmpty(_fallingBlock.RotatingBlock.x, _fallingBlock.RotatingBlock.y + 1))
          && _fallingBlock.ShouldDrop())
      {
        Game.InvokeBlockFall(
          _fallingBlock.StaticBlock,
          _fallingBlock.RotatingBlock,
          _fallingBlock.StaticCode,
          _fallingBlock.RotatingCode
        );
        Destroy(gameObject);
      }
      else HandleFastFall();

      _fallingBlock.Update(Time.deltaTime, _board);
    }

    private void OnGUI()
    {
      var staticBlock = _fallingBlock.StaticBlock;
      var rotatingBlock = _fallingBlock.RotatingBlock;

      if (_board.IsEmpty(staticBlock.x, staticBlock.y))
      {
        staticBlockRenderer.sprite = _spriteMapping[_fallingBlock.StaticCode];
        staticBlockTransform.position = FallingBlockProgress(
          staticBlock,
          _fallingBlock.GetBlockProgress(),
          _fallingBlock.Orientation == FallingBlock.BlockOrientation.Down
        );
      }

      if (rotatingBlock.y >= 0 && _board.IsEmpty(rotatingBlock.x, rotatingBlock.y))
      {
        rotatingBlockRenderer.sprite = _spriteMapping[_fallingBlock.RotatingCode];
        rotatingBlockTransform.position = FallingBlockProgress(
          rotatingBlock,
          _fallingBlock.GetBlockProgress(),
          _fallingBlock.Orientation == FallingBlock.BlockOrientation.Up
        );
      }
    }

    private Vector2 FallingBlockProgress(Vector2Int block, float fallingProgress, bool topFallingBlock)
    {
      var animationProgress = fallingProgress * fallingProgress;

      var startPos = BoardUtils.GetBoardCoordinates(block);
      var endPos = BoardUtils.GetBoardCoordinates(block + _oneDown);
      var trueProgress =
        _board.IsEmpty(block.x, block.y + 1) &&
        (!topFallingBlock || (_board.IsEmpty(block.x, block.y + 2)))
          ? animationProgress
          : 0;
      return Vector2.Lerp(startPos, endPos, t: trueProgress);
    }

    private void HandleFastFall()
    {
      if (Input.GetKeyDown(KeyCode.DownArrow)
          && _board.IsEmpty(_fallingBlock.StaticBlock.x, _fallingBlock.StaticBlock.y + 1)
          && _board.IsEmpty(_fallingBlock.RotatingBlock.x, _fallingBlock.RotatingBlock.y + 1))
      {
        _fallingBlock.FallFast();
      }

      if (_fallingBlock.ShouldDrop()) _fallingBlock.DropDown();
    }
  }
}