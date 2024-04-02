using UnityEngine;
using DG.Tweening;

namespace FooGames
{
    public class DOTweenAnimator : MonoBehaviour
    {
        private TweenCallback _bounce;
        private Tween _leftRightTween;
        private Tween _fadeTween;

        public void FadeIn(CanvasGroup canvasGroup, float time)
        {
            if (canvasGroup != null)
            {
                Fade(canvasGroup, 1f, time, () =>
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                });
            }
        }

        public void FadeOut(CanvasGroup canvasGroup, float time)
        {
            if (canvasGroup != null)
            {
                Fade(canvasGroup, 0f, time, () =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                });
            }
        }

        private void Fade(CanvasGroup canvasGroup, float endValue, float time, TweenCallback onEnd)
        {
            if (canvasGroup != null)
            {
                if (_fadeTween != null)
                {
                    _fadeTween.Kill(true);
                }

                _fadeTween = canvasGroup.DOFade(endValue, time);
                _fadeTween.onKill += onEnd;
            }
        }

        public void Bounce(GameObject someObject, float time = 0.25f, TweenCallback onEnd = null)
        {
            if (onEnd != null)
            {
                DOTween.KillAll(true);
            }

            _bounce =
            DOTween.Sequence()
                .SetLink(someObject)
                .Append(someObject.transform.DOScale(0.5f, time))
                .AppendInterval(0.1f)
                .Append(someObject.transform.DOScale(1.5f, time))
                .AppendInterval(0.1f)
                .Append(someObject.transform.DOScale(1f, time))
                .AppendInterval(0.5f)
                .onComplete += onEnd;
            _bounce = null;
        }

        public void LeftRight(GameObject someObject, float time = 0.1f)
        {
            if (_bounce == null)
            {
                if (_leftRightTween != null)
                {
                    _leftRightTween.Kill(true);
                }

                Vector3 startPosition = someObject.transform.position;
                Vector3 leftPosition = new Vector3(startPosition.x - 0.25f, startPosition.y);
                Vector3 rightPosition = new Vector3(startPosition.x + 0.25f, startPosition.y);

                _leftRightTween =
                DOTween.Sequence()
                    .SetEase(Ease.InBounce)
                    .Append(someObject.transform.DOMove(leftPosition, time))
                    .AppendInterval(0.1f)
                    .Append(someObject.transform.DOMove(rightPosition, time))
                    .AppendInterval(0.1f)
                    .Append(someObject.transform.DOMove(startPosition, time))
                    .AppendInterval(0.5f);
            }
        }
    }
}