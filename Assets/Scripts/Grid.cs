using System.Collections.Generic;
using Helper;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Gem baseGem;
    [SerializeField] private Sprite[] gemSprites;
    public static readonly Vector2 gridDimensions = new Vector2(9, 9);
    private float _offset = 2.5f;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var i = 0; i < gridDimensions.x; i++)
        {
            for (var j = 0; j < gridDimensions.y; j++)
            {
                var gridPosition = new Vector2(i, j);
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
        Camera.main.SetCamera();
    }
}
