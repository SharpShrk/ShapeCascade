using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _finalPanel;
    [SerializeField] private Button _startButtonEasy;
    [SerializeField] private Button _startButtonHard;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _finalText;

    [Header("Init")]
    [SerializeField] private Score _score;
    [SerializeField] private EmblemBar _bar;
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private EmblemFactory _factory;
    [SerializeField] private EmblemFactoryConfigurator[] _lvlConfigs;


    private void OnEnable()
    {
        _score.OnGameWin += GameWin;
        _bar.OnGameOver += GameOver;
        _startButtonEasy.onClick.AddListener(StartEasyGame);
        _startButtonHard.onClick.AddListener(StartHardGame);
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        _score.OnGameWin -= GameWin;
        _bar.OnGameOver -= GameOver;
        _startButtonEasy.onClick.RemoveListener(StartEasyGame);
        _startButtonHard.onClick.RemoveListener(StartHardGame);
        _restartButton.onClick.RemoveListener(RestartGame);
    }

    private void StartEasyGame()
    {
        StartGame(_lvlConfigs[0]);
    }

    private void StartHardGame()
    {
        StartGame(_lvlConfigs[1]);
    }

    private void StartGame(EmblemFactoryConfigurator config)
    {
        _factory.Init(config);
        _boardManager.gameObject.SetActive(true);
        _startPanel.SetActive(false);
    }

    private void GameOver()
    {
        _finalText.text = "Вы проиграли!";
        _finalPanel.SetActive(true);
    }

    private void GameWin()
    {
        _finalText.text = "Вы победили!";
        _finalPanel.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
