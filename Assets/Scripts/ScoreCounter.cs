using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private const string MAXScoreKey = "MaxScore";
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool isCountRunning;
    private int _maxScore;
    private int _score;
    private TextMeshProUGUI _textCounter;

    public bool IsCountRunning
    {
        set => isCountRunning = value;
    }

    private void Start()
    {
        _textCounter = GetComponent<TextMeshProUGUI>();
        _maxScore = PlayerPrefs.GetInt(MAXScoreKey);
    }

    private void Update()
    {
        if (!isCountRunning) return;
        _score = (int) playerTransform.transform.position.x;
        _textCounter.text = _score > -1 ? _score.ToString() : "0";
    }

    public void SetNewBestGameScore()
    {
        if (_score <= _maxScore) return;
        _maxScore = _score;
        _textCounter.text = $"New Best: {_maxScore}";
        PlayerPrefs.SetInt(MAXScoreKey, _maxScore);
    }
}