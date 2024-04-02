using UnityEngine;

namespace FooGames
{
    [CreateAssetMenu(fileName = "New Kit", menuName = "New Kit")]
    public class Kit : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Element[] _elements;

        public string Id => _id;
        public Element[] Elements => _elements;
    }
}