using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 gridPosition;
    public int id;

    public void Init(int id, Vector2 gridPosition)
    {
        this.id = id;
        this.gridPosition = gridPosition;
    }

    private void MoveGem(Gem newGem)
    {
        var tempPosition = transform.position;
        transform.position = newGem.transform.position;
        newGem.transform.position = tempPosition;
        var tempGem = gridPosition;
        gridPosition = newGem.gridPosition;
        newGem.gridPosition = tempGem;
    }

    private void OnMouseDown()
    {
        if (InputManager.Instance.firstSelectedGem == null)
        {
            InputManager.Instance.firstSelectedGem = this;
        }

        else if (InputManager.Instance.secondSelectedGem == null &&
                 Math.Abs(Vector2.Distance(gridPosition, InputManager.Instance.firstSelectedGem.gridPosition) - 1) < 0.05f)
        {
            {
                InputManager.Instance.secondSelectedGem = this;
                MoveGem(InputManager.Instance.firstSelectedGem);
                InputManager.Instance.ClearSelection();
            }
        }

        else
        {
            InputManager.Instance.ClearSelection();
        }
    }
}