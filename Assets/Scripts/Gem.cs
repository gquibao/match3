using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;
    public GameObject prefab;
    public Vector2 gridPosition;
    public int id;

    public bool Init(Vector2 gridPosition, Sprite sprite, int id)
    {
        this.gridPosition = gridPosition;
        spriteRenderer.sprite = sprite;
        this.id = id;
        return CheckForMatches(Vector2.left, gridPosition).Count <= 2 && CheckForMatches(Vector2.down, gridPosition).Count <= 2;
    }

    public void CheckMove(Gem newGem)
    {
        MoveGem(newGem);
    }
    
    private void MoveGem(Gem newGem)
    {
        var tempId = id;
        id = newGem.id;
        newGem.id = tempId;
        var tempSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = newGem.spriteRenderer.sprite;
        newGem.spriteRenderer.sprite = tempSprite;
    }
    
    public void MoveGem(Vector2 newPosition, float offset)
    {
        var gridObjects = GameManager.Instance.gridObjects;
        gridObjects.Remove(gridPosition);
        gridPosition = newPosition;
        transform.position = gridPosition * offset;
        gridObjects.Add(gridPosition, gameObject);
    }

    public List<Gem> CheckForMatches(Vector2 direction, Vector2 originPosition)
    {
        var directionsToCheck = new[] {direction, -direction};
        var checkPosition = originPosition;
        var foundMatches = new List<Gem> {this};
        var gridObjects = GameManager.Instance.gridObjects;
        for (var i = 0; i <= 1; i++)
        {
            while (gridObjects.ContainsKey(checkPosition + directionsToCheck[i]) &&
                   id == gridObjects[checkPosition + directionsToCheck[i]].GetComponent<Gem>().id)
            {
                var gem = gridObjects[checkPosition + directionsToCheck[i]].GetComponent<Gem>();
                foundMatches.Add(gem);
                checkPosition = gem.gridPosition;
            }
            checkPosition = originPosition;
        }
        return foundMatches;
    }

    private void OnMouseDown()
    {
        GameManager.Instance.SelectGems(this);
    }
}