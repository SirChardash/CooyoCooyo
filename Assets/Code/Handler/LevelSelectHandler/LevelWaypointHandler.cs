using Code.Board;
using Code.LevelSelect;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Handler.LevelSelectHandler
{
  public class LevelWaypointHandler : MonoBehaviour
  {
    public Collider2D collider;
    public Camera camera;

    public void Update()
    {
      if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      {
        var worldPoint = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
        if (collider.OverlapPoint(worldPoint))
        {
          LoadLevel();
        }
      }
    }

    private void OnMouseDown()
    {
      LoadLevel();
    }

    private void LoadLevel()
    {
      Game.ActiveGame = new Game(Levels.Predefined["testLevel"]);
      SceneManager.LoadScene("LevelPlay");
    }
  }
}