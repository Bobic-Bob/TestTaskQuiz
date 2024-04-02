using TMPro;
using UnityEngine;

namespace FooGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITask : MonoBehaviour
    {
        [SerializeField] private TaskRandomizer _taskRandomizer;
        [SerializeField] private LevelCounter _levelCounter;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private DOTweenAnimator _animator;

        private bool _firstShowTask = true;

        private void OnEnable()
        {
            LevelCounter.LevelCompleted += ShowTask;
        }

        private void OnDisable()
        {
            LevelCounter.LevelCompleted -= ShowTask;
        }

        private void Start()
        {
            if (_taskRandomizer == null || _levelCounter == null || _textMeshPro == null)
            {
                throw new System.ArgumentNullException();
            }
        }

        private void ShowTask()
        {
            if (_firstShowTask == true || _animator != null)
            {
                _firstShowTask = false;
                _animator.FadeIn(gameObject.transform.parent.GetComponent<CanvasGroup>(), 3f);
            }

            _textMeshPro.text = _taskRandomizer.TasksByLevelNumber[_levelCounter.CurrentLevelNumber].Name;
        }
    }
}