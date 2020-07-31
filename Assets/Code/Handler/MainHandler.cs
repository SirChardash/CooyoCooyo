using System;
using System.Collections.Generic;
using Code.Board;
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

    void Start()
    {
      _game = new GameLogic();
      _board = _game.BoardState;
      _renderBoard = new SpriteRenderer[10, 8];
      for (var x = 0; x < 8; x++)
      {
        for (var y = 0; y < 10; y++)
        {
          var block = Instantiate(blockPrefab);
          block.transform.position = new Vector2(Scale * (x - 4), Scale * (5 - y));
          _renderBoard[y, x] = block.GetComponent<SpriteRenderer>();
        }
      }

      _spriteMapping = new Dictionary<Block, Sprite>()
      {
        {Block.Block1, Resources.Load("Images/Apple_01", typeof(Sprite)) as Sprite},
        {Block.Block2, Resources.Load("Images/Cauliflower_01", typeof(Sprite)) as Sprite},
        {Block.Block3, Resources.Load("Images/Radish_01", typeof(Sprite)) as Sprite},
        {Block.Block4, Resources.Load("Images/Red_current_01", typeof(Sprite)) as Sprite},
        {Block.Poof1, Resources.Load("Images/SpellBook03_02", typeof(Sprite)) as Sprite},
      };
    }

    void Update()
    {
      _game.Update(Time.deltaTime);
    }

    private void OnGUI()
    {
      for (var x = 0; x < 8; x++)
      {
        for (var y = 0; y < 10; y++)
        {
          if (_board.Get(x, y) != 0)
          {
            _renderBoard[y, x].sprite = _spriteMapping[_board.Get(x, y)];
          }
          else
          {
            _renderBoard[y, x].sprite = null;
          }
        }
      }

      var fallingBlock = _game.FallingBlock;
      _renderBoard[fallingBlock.StaticBlock.y, fallingBlock.StaticBlock.x].sprite =
        _spriteMapping[fallingBlock.StaticCode];
      if (fallingBlock.RotatingBlock.y >= 0)
      {
        _renderBoard[fallingBlock.RotatingBlock.y, fallingBlock.RotatingBlock.x].sprite =
          _spriteMapping[fallingBlock.RotatingCode];
      }
    }
  }
}