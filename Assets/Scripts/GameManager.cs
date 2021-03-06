﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private const int LevelTime = 120;
    public Dictionary<Vector2, GameObject> gridObjects = new Dictionary<Vector2, GameObject>();
    private Gem _firstSelectedGem;
    private Gem _secondSelectedGem;
    private int _score;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text targetScoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioSelected, audioSwap, audioClear;
    public Action<Vector2> onMatchScored;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        scoreText.text = "0";
        StartCoroutine(Timer(100));
    }

    public void SelectGems(Gem selectedGem)
    {
        if (_firstSelectedGem == null)
        {
            _firstSelectedGem = selectedGem;
            PrepareAndPlayAudio(audioSelected);
            _firstSelectedGem.spriteRenderer.color = new Color(1, 1, 1, 0.8f);
        }

        else if (_secondSelectedGem == null &&
                 Math.Abs(Vector2.Distance(selectedGem.gridPosition, _firstSelectedGem.gridPosition) - 1) < 0.05f)
        {
            {
                _secondSelectedGem = selectedGem;
                PrepareAndPlayAudio(audioSwap);
                _firstSelectedGem.CheckMove(_secondSelectedGem);
                CheckMatches(_firstSelectedGem);
                CheckMatches(_secondSelectedGem);
                ClearSelection();
            }
        }

        else
        {
            ClearSelection();
        }
    }

    private void PrepareAndPlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void CheckMatches(Gem gem)
    {
        ScoreMatch(gem.CheckForMatches(Vector2.right, gem.gridPosition));
        ScoreMatch(gem.CheckForMatches(Vector2.up, gem.gridPosition));
    }

    private void ScoreMatch(List<Gem> foundMatches)
    {
        var matchCount = foundMatches.Count;
        if (matchCount < 3) return;
        _score += 10 * matchCount;
        scoreText.text = _score.ToString();
        PrepareAndPlayAudio(audioClear);
        foundMatches.ForEach(gem =>
        {
            if (gridObjects.ContainsValue(gem.gameObject))
            {
                gridObjects.Remove(gem.gridPosition);
            }
            onMatchScored.Invoke(gem.gridPosition);
            Destroy(gem.gameObject);
        });
    }

    private void ClearSelection()
    {
        _firstSelectedGem.spriteRenderer.color = Color.white;
        _firstSelectedGem = null;
        _secondSelectedGem = null;
    }

    private void RestartGame()
    {
        var grid = FindObjectOfType<Grid>();
        grid.gameObject.ClearGemContainer();
        gameOverPanel.SetActive(false);
        gridObjects.Clear();
        _score = 0;
        scoreText.text = "0";
        grid.CreateGrid();
        StartCoroutine(Timer(100));
    }
    
    private IEnumerator Timer(int levelGoal)
    {
        var time = LevelTime;
        targetScoreText.text = levelGoal.ToString();
        while (time > 0)
        {
            time--;
            timerText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
            yield return new WaitForSeconds(1);
        }

        if (_score >= levelGoal)
        {
            StartCoroutine(Timer(levelGoal + 200));
        }

        else
        {
            gameOverPanel.SetActive(true);
        }
    }
}