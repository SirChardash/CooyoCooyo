using System;
using UnityEngine;

namespace Code.Handler
{
  public delegate void CameraChangedEvent();

  public class CameraChangeListener : MonoBehaviour
  {
    public event CameraChangedEvent CameraChanged;
    public new Camera camera;

    private float _size;

    private void Start()
    {
      _size = camera.orthographicSize;
    }

    private void Update()
    {
      if (Math.Abs(_size - camera.orthographicSize) > 0.001f)
      {
        CameraChanged?.Invoke();
      }

      _size = camera.orthographicSize;
    }
  }
}