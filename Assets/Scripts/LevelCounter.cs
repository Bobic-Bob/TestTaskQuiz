using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FooGames
{
    public class LevelCounter : MonoBehaviour
    {
        public static Action<Level> LevelChanged;
        public static Action LevelCompleted;
        public static Action<Level> GameCompleted;
        public static Action GameRestart;

        [SerializeField] private List<Level> _levels;

        [SerializeField] private DOTweenAnimator _animator;
        [SerializeField] private CanvasGroup _endGameMenu;
        [SerializeField] private CanvasGroup _loadScreen;

        public IReadOnlyList<Level> Levels => _levels;

        private int _currentLevelNumber = 0;

        public int CurrentLevelNumber => _currentLevelNumber;

        private void OnEnable()
        {
            TaskRandomizer.TasksReady += NextLevel;
            AnswerChecker.lastCorrectAnswer += ShowEndGameMenu;
        }

        private void OnDisable()
        {
            TaskRandomizer.TasksReady -= NextLevel;
            AnswerChecker.lastCorrectAnswer -= ShowEndGameMenu;
        }

        private void Start()
        {
            if (Levels.Count == 0)
            {
                throw new ArgumentException($"Levels count can't be 0");
            }
        }

        public void NextLevel()
        {
            if (_currentLevelNumber < _levels.Count)
            {
                LevelCompleted?.Invoke();
                LevelChanged?.Invoke(_levels[_currentLevelNumber]);
                _currentLevelNumber++;
            }
            else
            {
                Restart();
            }
        }

        public void ShowLoadScreen()
        {
            if (_animator != null && _loadScreen != null)
            {
                _animator.FadeIn(_loadScreen, 3f);
                _animator.FadeOut(_loadScreen, 3f);
            }
        }

        private void ShowEndGameMenu()
        {
            if (_animator != null && _endGameMenu != null)
            {
                _animator.FadeIn(_endGameMenu, 1.5f);
            }
        }

        private void Restart()
        {
            _animator.DOKill();
            _currentLevelNumber = 0;
            GameCompleted?.Invoke(_levels[0]);
            GameRestart?.Invoke();
        }
    }
}