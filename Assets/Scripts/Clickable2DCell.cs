using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FooGames
{
    [RequireComponent(typeof(Collider2D), typeof(Cell))]
    public class Clickable2DCell : MonoBehaviour
    {
        public static Action<Cell> CellClicked;

        private Cell _cell;
        private Camera _camera;

        private void Start()
        {
            _cell = GetComponent<Cell>();
            _camera = Camera.main;
        }

        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CellClicked?.Invoke(_cell);
            }
        }
    }
}