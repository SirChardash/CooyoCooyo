using UnityEngine;

namespace Code.Handler
{
  public delegate void Pressed();

  public class SingleInputHandler : MonoBehaviour
  {
    public event Pressed InputPressed;

    public Collider2D collider;
    public Camera camera;

    public void Update()
    {
      if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      {
        var worldPoint = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
        if (collider.OverlapPoint(worldPoint))
        {
          InputPressed?.Invoke();
        }
      }
    }
  }
}