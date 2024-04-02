using System;
using System.Collections.Generic;
using UnityEngine;

namespace FooGames
{
    public class TaskRandomizer : MonoBehaviour
    {
        public static Action TasksReady;

        [Min(1)]
        [SerializeField] private LevelCounter _levelCounter;
        [SerializeField] private List<Kit> _kits;

        [SerializeField] private List<Element> _notUsedTasks;

        private Dictionary<int, Element> _tasksByLevelNumber = new Dictionary<int, Element>();

        public IReadOnlyDictionary<int, Element> TasksByLevelNumber => _tasksByLevelNumber;

        public IReadOnlyList<Kit> Kits => _kits;

        private void OnEnable()
        {
            LevelCounter.GameRestart += SetAllPossibleTasks;
            LevelCounter.GameRestart += SetSessionTasks;
        }

        private void OnDisable()
        {
            LevelCounter.GameRestart -= SetAllPossibleTasks;
            LevelCounter.GameRestart -= SetSessionTasks;
        }

        private void Start()
        {
            SetAllPossibleTasks();

            SetSessionTasks();
        }

        private void SetAllPossibleTasks()
        {
            for (int i = 0; i < _kits.Count; i++)
            {
                for (int j = 0; j < _kits[i].Elements.Length; j++)
                {
                    _notUsedTasks.Add(_kits[i].Elements[j]);
                }
            }

            // если уровней больше, чем количество возможных заданий без повторов
            if (_levelCounter.Levels.Count > _notUsedTasks.Count)
            {
                throw new ArgumentException($"Too much levels {_levelCounter.Levels.Count} and not enough elements in kits {_notUsedTasks.Count}");
            }
        }

        private void SetSessionTasks()
        {
            _tasksByLevelNumber.Clear();

            for (int i = 0; i < _levelCounter.Levels.Count; i++)
            {
                Element element = GetRandomElementExceptUsed();

                if (_tasksByLevelNumber.ContainsValue(element) == false)
                {
                    _tasksByLevelNumber.Add(i, element);

                    _notUsedTasks.Remove(element);
                }
            }

            TasksReady?.Invoke();
        }

        private Element GetRandomElementExceptUsed()
        {
            int randomElement = UnityEngine.Random.Range(0, _notUsedTasks.Count);
            return _notUsedTasks[randomElement];
        }
    }
}