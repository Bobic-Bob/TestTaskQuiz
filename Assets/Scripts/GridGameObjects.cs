using System;
using System.Collections.Generic;
using UnityEngine;

namespace FooGames
{
    public class GridGameObjects : MonoBehaviour
    {
        public static Action GridResized;

        [SerializeField] private GameObject _cellPrefab;

        [Min(1)]
        [SerializeField] private int _sizeX = 1;
        [Min(1)]
        [SerializeField] private int _sizeY = 1;
        [SerializeField] private List<GameObject> _cells;

        [Space]
        [SerializeField] private DOTweenAnimator _doTweenAnimator;

        private bool _firstCellsSpawn = true;

        public IReadOnlyList<GameObject> Cells => _cells;

        private void OnEnable()
        {
            LevelCounter.LevelChanged += ResizeGribByLevel;
        }

        private void OnDisable()
        {
            LevelCounter.LevelChanged -= ResizeGribByLevel;
        }

        private void Start()
        {
            ClearGrid();
        }

        private void ResizeGribByLevel(Level level)
        {
            ClearGrid();
            _sizeX = level.SizeX;
            _sizeY = level.SizeY;
            SpawnCells();
            GridResized?.Invoke();
        }

        private void SpawnCells()
        {
            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    Vector3 cellPoint = new Vector3(x, y, 0);
                    GameObject cell = Instantiate(_cellPrefab, cellPoint, Quaternion.identity);
                    cell.transform.parent = transform;

                    _cells.Add(cell);
                }
            }

            if (_firstCellsSpawn == true && _doTweenAnimator != null)
            {
                for (int i = 0; i < _cells.Count; i++)
                {
                    _doTweenAnimator.Bounce(_cells[i].gameObject, 1f);
                }

                _firstCellsSpawn = false;
            }
        }

        private void ClearGrid()
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

            }

            _cells.Clear();
        }
    }
}