using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FooGames
{
    public class CellsDataRandomizer : MonoBehaviour
    {
        [SerializeField] private TaskRandomizer _task;       // задание, уровни, выбранный набор
        [SerializeField] private GridGameObjects _grid;      // сетка
        [SerializeField] private LevelCounter _levelCounter; // номер текущего уровн€

        [SerializeField] private List<Element> _usedElements;
        [SerializeField] private List<Element> _notUsedElements;

        private void OnEnable()
        {
            GridGameObjects.GridResized += SetRandomDataToAllCells;
        }

        private void OnDisable()
        {
            GridGameObjects.GridResized -= SetRandomDataToAllCells;
        }

        private void Start()
        {
            if (_task == null || _grid == null || _levelCounter == null)
            {
                throw new System.ArgumentException($"{_task}, {_grid} or {_levelCounter} fields can't be empty");
            }
        }

        private void SetRandomDataToAllCells()
        {
            if (_task.TasksByLevelNumber.Count > _levelCounter.CurrentLevelNumber)
            {
                PrepareDataToRandomize();

                // если €чеек сетки больше, чем элементов в наборе, то генераци€ с дубл€ми
                bool isEnoughElements = true;
                if (_grid.Cells.Count > _notUsedElements.Count + 1)
                {
                    isEnoughElements = false;
                }

                SetAnswerCell();

                // установка случайных данных в €чейки
                for (int i = 0; i < _grid.Cells.Count; i++)
                {
                    // генераци€ данных без повтора
                    if (isEnoughElements == true)
                    {
                        SetRandomDataToCell(i);
                    }
                    // генераци€ данных с повторами
                    else
                    {
                        // кешируем доступные элементы дл€ переиспользовани€
                        if (i == 0)
                        {
                            CopyElements(_notUsedElements, _usedElements);
                        }

                        // заполнение без повторов
                        if (_notUsedElements.Count > 0)
                        {
                            SetRandomDataToCell(i);
                        }
                        // заполнение с повторами
                        else
                        {
                            CopyElements(_usedElements, _notUsedElements);

                            SetRandomDataToCell(i);
                        }
                    }
                }
            }
        }

        // устанавливает все доступные дл€ рандома элементы, кроме ответа
        private void PrepareDataToRandomize()
        {
            ClearElementsCache();

            Kit kit = GetKitByElement(GetAnswer());
            CopyElements(kit.Elements.ToList(), _notUsedElements);
            _notUsedElements.Remove(GetAnswer());
        }

        private void ClearElementsCache()
        {
            _notUsedElements.Clear();
            _usedElements.Clear();
        }

        private void CopyElements(List<Element> from, List<Element> to)
        {
            for (int i = 0; i < from.Count; i++)
            {
                to.Add(from[i]);
            }
        }

        private void SetRandomDataToCell(int gridCellNumber)
        {
            // закешировать €чейки отдельно
            if (_grid.Cells[gridCellNumber].GetComponent<Cell>().Element == null)
            {
                int randomElementId = Random.Range(0, _notUsedElements.Count);

                Element cellData = _notUsedElements[randomElementId];
                // закешировать €чейки отдельно
                _grid.Cells[gridCellNumber].GetComponent<Cell>().SetCellData(cellData);

                _notUsedElements.RemoveAt(randomElementId);
            }
        }

        // надо знать текущий уровень, правильный ответ на нЄм
        private void SetAnswerCell()
        {
            // случайно задает правильную €чейку, относительно текущего уровн€
            Element answer = GetAnswer();
            int randomCellNumber = Random.Range(0, _grid.Cells.Count);
            // закешировать €чейки отдельно
            _grid.Cells[randomCellNumber].GetComponent<Cell>().SetCellData(answer);
        }


        private Kit GetKitByElement(Element elementToFindKit)
        {
            for (int i = 0; i < _task.Kits.Count; i++)
            {
                for (int j = 0; j < _task.Kits[i].Elements.Length; j++)
                {
                    if (_task.Kits[i].Elements[j] == elementToFindKit)
                    {
                        Kit kitWithElement = _task.Kits[i];
                        return kitWithElement;
                    }
                }
            }

            throw new System.ArgumentOutOfRangeException($"Can't find kit by {elementToFindKit} in {_task.Kits}");
        }

        private Element GetAnswer()
        {
            Element answer = _task.TasksByLevelNumber[_levelCounter.CurrentLevelNumber];
            return answer;
        }
    }
}