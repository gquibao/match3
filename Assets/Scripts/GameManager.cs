using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Dictionary<Vector2, GameObject> gridObjects = new Dictionary<Vector2, GameObject>();
    private Gem _firstSelectedGem;
    private Gem _secondSelectedGem;

    public void SelectGems(Gem selectedGem)
    {
        if (_firstSelectedGem == null)
        {
            _firstSelectedGem = selectedGem;
        }

        else if (_secondSelectedGem == null &&
                 Math.Abs(Vector2.Distance(selectedGem.gridPosition, _firstSelectedGem.gridPosition) - 1) < 0.05f)
        {
            {
                _secondSelectedGem = selectedGem;
                _firstSelectedGem.CheckMove(_secondSelectedGem);

                //TODO A more efficient way to do this.
                ScoreMatch(_firstSelectedGem.CheckForMatches(Vector2.right, _firstSelectedGem.gridPosition));
                ScoreMatch(_secondSelectedGem.CheckForMatches(Vector2.right, _secondSelectedGem.gridPosition));
                ScoreMatch(_firstSelectedGem.CheckForMatches(Vector2.up, _firstSelectedGem.gridPosition));
                ScoreMatch(_secondSelectedGem.CheckForMatches(Vector2.up, _secondSelectedGem.gridPosition));
                //------------------------------------------

                ClearSelection();
            }
        }

        else
        {
            ClearSelection();
        }
    }

    private void ScoreMatch(List<Gem> foundMatches)
    {
        var matchCount = foundMatches.Count;
        var gridObjects = GameManager.Instance.gridObjects;
        if (matchCount < 3) return;
        Debug.Log($"Pontos: {10 * matchCount}");
        foundMatches.ForEach(gem =>
        {
            if (gridObjects.ContainsValue(gem.gameObject))
            {
                gridObjects.Remove(gem.gridPosition);
            }

            Destroy(gem.gameObject);
        });
    }

    private void ClearSelection()
    {
        _firstSelectedGem = null;
        _secondSelectedGem = null;
    }
}