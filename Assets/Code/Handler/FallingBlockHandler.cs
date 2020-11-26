using System.Collections.Generic;
using Code.Board;
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
    private ICoordinates _coordinates;

    /// <summary>
    /// Means that one block is placed and the other one is falling down separately.
    /// </summary>
    private bool _freeFall;

    public void SetRequired(FallingBlock fallingBlock, ICoordinates coordinates, InputHandler input)
    {
      _fallingBlock = fallingBlock;
      _board = Game.ActiveGame.Board;
      _spriteMapping = Game.ActiveGame.SpriteMapping;
      staticBlockTransform.position = coordinates.GetBoardCoordinates(_fallingBlock.StaticBlock);
      _coordinates = coordinates;
      input.RotatePressed += TryRotate;
      input.LeftPressed += TryMoveLeft;
      input.RightPressed += TryMoveRight;
      input.FallFastPressed += StartFallFast;
    }

    private void TryRotate()
    {
      if (!_fallingBlock.FallFastMode)
      {
        _fallingBlock.TryRotate(_board);
      }
    }

    private void TryMoveLeft()
    {
      if (!_fallingBlock.FallFastMode
          && _board.IsEmpty(_fallingBlock.StaticBlock.x - 1, _fallingBlock.StaticBlock.y)
          && _board.IsEmpty(_fallingBlock.RotatingBlock.x - 1, _fallingBlock.RotatingBlock.y))
      {
        _fallingBlock.StaticBlock.x--;
        _fallingBlock.RotatingBlock.x--;
      }
    }

    private void TryMoveRight()
    {
      if (!_fallingBlock.FallFastMode
          && _board.IsEmpty(_fallingBlock.StaticBlock.x + 1, _fallingBlock.StaticBlock.y)
          && _board.IsEmpty(_fallingBlock.RotatingBlock.x + 1, _fallingBlock.RotatingBlock.y))
      {
        _fallingBlock.StaticBlock.x++;
        _fallingBlock.RotatingBlock.x++;
      }
    }

    private void StartFallFast()
    {
      if (!_fallingBlock.FallFastMode
          && _board.IsEmpty(_fallingBlock.StaticBlock.x, _fallingBlock.StaticBlock.y + 1)
          && _board.IsEmpty(_fallingBlock.RotatingBlock.x, _fallingBlock.RotatingBlock.y + 1))
      {
        _fallingBlock.FallFast();
      }
    }

    private void Update()
    {
      if (!_freeFall
          && _fallingBlock.StaticBlock.x != _fallingBlock.RotatingBlock.x
          && IsEmptyBelow(_fallingBlock.StaticBlock) != IsEmptyBelow(_fallingBlock.RotatingBlock))
      {
        _freeFall = true;
        _fallingBlock.FallFast();
      }

      if (_freeFall)
      {
        _fallingBlock.Update(Time.deltaTime, _board);
        if (_fallingBlock.ShouldDrop())
        {
          if (IsEmptyBelow(_fallingBlock.StaticBlock) && _fallingBlock.ShouldDrop())
          {
            _fallingBlock.DropDownStatic();
          }
          else
          {
            _fallingBlock.DropDownRotating();
          }
        }

        if (IsEmptyBelow(_fallingBlock.StaticBlock) || IsEmptyBelow(_fallingBlock.RotatingBlock))
        {
          return;
        }
      }

      if ((!IsEmptyBelow(_fallingBlock.StaticBlock)
           || !IsEmptyBelow(_fallingBlock.RotatingBlock))
          && (_fallingBlock.ShouldDrop() || _freeFall))
      {
        Game.ActiveGame.InvokeBlockFall(
          _fallingBlock.StaticBlock,
          _fallingBlock.RotatingBlock,
          _fallingBlock.StaticCode,
          _fallingBlock.RotatingCode
        );
        Destroy(gameObject);
      }
      else if (_fallingBlock.ShouldDrop())
      {
        _fallingBlock.DropDown();
      }

      _fallingBlock.Update(Time.deltaTime, _board);
    }

    private void OnGUI()
    {
      if (_coordinates == null)
      {
        return;
      }

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

      var startPos = _coordinates.GetBoardCoordinates(block);
      var endPos = _coordinates.GetBoardCoordinates(block + _oneDown);
      var trueProgress =
        _board.IsEmpty(block.x, block.y + 1) &&
        (!topFallingBlock || _board.IsEmpty(block.x, block.y + 2))
          ? animationProgress
          : 0;
      return Vector2.Lerp(startPos, endPos, t: trueProgress);
    }

    private bool IsEmptyBelow(Vector2Int position)
    {
      return _board.IsEmpty(position.x, position.y + 1);
    }
  }
}