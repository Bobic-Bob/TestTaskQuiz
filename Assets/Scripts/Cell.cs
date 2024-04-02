using UnityEngine;

namespace FooGames
{
    [RequireComponent(typeof(SpriteRenderer), typeof(ParticleSystem))]
    public class Cell : MonoBehaviour
    {
        public Element Element { private set; get; }
        public ParticleSystem ParticleSystem { private set; get; }

        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            LevelCounter.LevelCompleted += ClearCellData;
        }

        private void OnDisable()
        {
            LevelCounter.LevelCompleted -= ClearCellData;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            ParticleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            transform.Rotate(new Vector3(0, 0, Element.RotationAngle));         
        }

        public void SetCellData(Element element)
        {
            if (Element == null)
            {
                Element = element;
                ShowSprite();
            }
            else
            {
                throw new System.ArgumentException($"This cell already has data {Element}");
            }
        }

        private void ShowSprite()
        {
            if (Element != null)
            {
                _spriteRenderer.sprite = Element.Sprite;
            }
            else
            {
                throw new System.NullReferenceException($"No data to show");
            }
        }

        private void ClearCellData()
        {
            Element = null;
            _spriteRenderer.sprite = null;
        }
    }
}