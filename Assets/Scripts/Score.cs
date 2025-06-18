using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreValue;

    public event Action OnGameWin;

    public void SetScore(int score)
    {
        _scoreValue.text = score.ToString();
        
        if(score <= 0)
        {
            OnGameWin?.Invoke();
        }
    }
}
