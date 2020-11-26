using UnityEngine;

namespace Code.Handler.LevelPlay
{
  public delegate void InputPressed();

  public class InputHandler : MonoBehaviour
  {
    public SingleInputHandler left;
    public SingleInputHandler right;
    public SingleInputHandler rotate;
    public SingleInputHandler fallFast;

    public event InputPressed LeftPressed;
    public event InputPressed RightPressed;
    public event InputPressed RotatePressed;
    public event InputPressed FallFastPressed;

    private void Start()
    {
      left.InputPressed += () => LeftPressed?.Invoke();
      right.InputPressed += () => RightPressed?.Invoke();
      rotate.InputPressed += () => RotatePressed?.Invoke();
      fallFast.InputPressed += () => FallFastPressed?.Invoke();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        LeftPressed?.Invoke();
      }
      else if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        RightPressed?.Invoke();
      }
      else if (Input.GetKeyDown(KeyCode.UpArrow))
      {
        RotatePressed?.Invoke();
      }
      else if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        FallFastPressed?.Invoke();
      }
    }
  }
}