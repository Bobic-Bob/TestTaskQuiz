using System;
using UnityEngine;

namespace FooGames
{
    public class AnswerChecker : MonoBehaviour
    {
        public static Action lastCorrectAnswer;

        [SerializeField] private TaskRandomizer _taskRandomizer;
        [SerializeField] private LevelCounter _levelCounter;
        [Space]
        [SerializeField] private DOTweenAnimator _doTweenAnimator;

        private void OnEnable()
        {
            Clickable2DCell.CellClicked += CheckAnswer;
        }

        private void OnDisable()
        {
            Clickable2DCell.CellClicked -= CheckAnswer;
        }

        private void Start()
        {
            if (_taskRandomizer == null || _levelCounter == null)
            {
                throw new System.ArgumentNullException();
            }
        }

        private void CheckAnswer(Cell answerCell)
        {
            if (answerCell.Element != null)
            {
                if (answerCell.Element == _taskRandomizer.TasksByLevelNumber[_levelCounter.CurrentLevelNumber - 1])
                {
                    if (_doTweenAnimator != null)
                    {
                        answerCell.ParticleSystem.Play();

                        if (_levelCounter.CurrentLevelNumber < _levelCounter.Levels.Count)
                        {
                            _doTweenAnimator.Bounce(answerCell.gameObject, 0.25f, _levelCounter.NextLevel);
                        }
                        else
                        {
                            _doTweenAnimator.Bounce(answerCell.gameObject, 0.25f);
                            lastCorrectAnswer?.Invoke();
                        }
                    }
                }
                else
                {
                    if (_doTweenAnimator != null)
                    {
                        _doTweenAnimator.LeftRight(answerCell.gameObject);
                    }
                }
            }
        }
    }
}