using System.Collections.Generic;
using Helper;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public static Dictionary<Vector2, Gem> gemsInTheGrid = new Dictionary<Vector2, Gem>();
    [SerializeField] private Gem[] gems;
    private readonly Vector2 _gridDimensions = new Vector2(9, 9);
    private float _offset = 2.5f;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (var i = 0; i < _gridDimensions.x; i++)
        {
            for (var j = 0; j < _gridDimensions.y; j++)
            {
                var scenePosition = new Vector2(i, j);
                var random = Random.Range(0, gems.Length);
                var go = Instantiate(gems[random].prefab, scenePosition * _offset, Quaternion.identity, transform);
                var gem = go.GetComponent<Gem>();
                gem.Init(random, scenePosition);
                gemsInTheGrid.Add(scenePosition, gem);
            }
        }
        Camera.main.SetCamera();
    }
}
