using Code.Board;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Handler.LevelPlay
{
  public class LevelEndHandler : MonoBehaviour
  {
    private void Start()
    {
      Game.ActiveGame.LevelEnd += CloseActiveGame;
    }

    private void CloseActiveGame()
    {
      SceneManager.LoadScene("LevelSelect");
    }
  }
}