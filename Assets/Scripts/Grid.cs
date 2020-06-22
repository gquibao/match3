using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [SerializeField] private Gem baseGem;
    [SerializeField] private Sprite[] gemSprites;
    public static readonly Vector2 GridDimensions = new Vector2(9, 9);
    private float _offset = 2.5f;

    private void Start()
    {
        GameManager.Instance.onMatchScored += ReadjustGrid;
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var i = 0; i < GridDimensions.x; i++)
        {
            for (var j = 0; j < GridDimensions.y; j++)
            {
                AddGemToGrid(new Vector2(i, j));
            }
        }

        Camera.main.SetCamera();
    }

    private void ReadjustGrid(Vector2 gemPosition)
    {
        StartCoroutine(ReadjustCoroutine(gemPosition));
    }

    private IEnumerator ReadjustCoroutine(Vector2 emptySpacePosition)
    {
        var xValue = emptySpacePosition.x;
        var yValue = emptySpacePosition.y;
        var gridObjects = GameManager.Instance.gridObjects;
        var emptySpaces = 0;
        for (var j = yValue; j < GridDimensions.y; j++)
        {
            var gridPosition = new Vector2(xValue, j);
            if (!gridObjects.ContainsKey(gridPosition))
            {
                emptySpaces++;
                continue;
            }
            var gem = gridObjects[gridPosition].GetComponent<Gem>();
            for (var k = gem.gridPosition.y - 1; k >= 0; k--)
            {
                if (gridObjects.ContainsKey(new Vector2(xValue, k)) ||
                    !gridObjects.ContainsValue(gem.gameObject)) break;
                gem.MoveGem(new Vector2(gem.gridPosition.x, k), _offset);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (emptySpaces > 0 && !gridObjects.ContainsKey(new Vector2(xValue, GridDimensions.y - 1)))
        {
            AddGemToGrid(new Vector2(xValue, GridDimensions.y - 1));
            StartCoroutine(ReadjustCoroutine(new Vector2(xValue, GridDimensions.y - emptySpaces)));
        }
        else
        {
            gridObjects.Values.ToList().ForEach(go =>
            {
                var gem = go.GetComponent<Gem>();
                GameManager.Instance.CheckMatches(gem);
            });
        }
    }

    private void AddGemToGrid(Vector2 gridPosition)
    {
        var random = Random.Range(0, gemSprites.Length);
        var go = Instantiate(baseGem.prefab, gridPosition * _offset, Quaternion.identity, transform);
        var gem = go.GetComponent<Gem>();
        while (!gem.Init(gridPosition, gemSprites[random], random))
        {
            random = Random.Range(0, gemSprites.Length);
        }

        GameManager.Instance.gridObjects.Add(gridPosition, go);
    }
}