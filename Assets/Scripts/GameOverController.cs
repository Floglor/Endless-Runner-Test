using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class GameOverController : MonoBehaviour
{
    public static GameOverController Instance;
    [SerializeField] private ScoreCounter scoreCounter;
    [SerializeField] private TextMeshProUGUI centerTextUGUI;
    [SerializeField] private PlayerController playerController;
    private bool _gameStarted;
    private IInputController _inputController;
    private bool _isGameOver;
    private float _storedMoveSpeed;
    private bool _startedGameThisFrame;
    private void Start()
    {
        _inputController = GetComponent<IInputController>();
        Instance = this;
        playerController.IsControlsBlocked = true;
        centerTextUGUI.gameObject.SetActive(true);
        centerTextUGUI.text = "Press any key to start";
    }

    private void Update()
    {
        if (_gameStarted && !_isGameOver) return;
        if (!_inputController.GetInput()) return;

        if (!_isGameOver)
            StartGame();
        else
            RestartGame();
    }

    private void RestartGame()
    {
        Resources.UnloadUnusedAssets();
        SceneManager.UnloadSceneAsync("MainGame");
        SceneManager.LoadSceneAsync("MainGame");
    }

    private void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;
        scoreCounter.IsCountRunning = true;
        playerController.IsMoving = true;
        centerTextUGUI.gameObject.SetActive(false);
        _startedGameThisFrame = true;
    }

    private void LateUpdate()
    {
        if (!_startedGameThisFrame) return;
        playerController.IsControlsBlocked = false;
        _startedGameThisFrame = false;
    }

    public void GameOver()
    {
        _isGameOver = true;
        scoreCounter.IsCountRunning = false;
        scoreCounter.SetNewBestGameScore();
        playerController.gameObject.SetActive(false);
        SetCenterTextActiveAndGiveMessage("Game Over, press or tap to restart");
    }

    private void SetCenterTextActiveAndGiveMessage(string message)
    {
        centerTextUGUI.gameObject.SetActive(true);
        centerTextUGUI.text = message;
    }
}