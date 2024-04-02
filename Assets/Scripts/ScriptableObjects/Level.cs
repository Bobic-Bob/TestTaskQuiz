using UnityEngine;

namespace FooGames
{
    [CreateAssetMenu(fileName = "New Level", menuName = "New Level")]
    public class Level : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int _sizeX = 1;
        [Min(1)]
        [SerializeField] private int _sizeY = 1;

        public int SizeX => _sizeX;
        public int SizeY => _sizeY;
    }
}