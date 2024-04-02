using System;
using UnityEngine;

namespace FooGames
{
    [Serializable]
    public class Element
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [Range(0f, 360f)]
        [SerializeField] private float _rotationAngle = 0f;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public float RotationAngle => _rotationAngle;
    }
}