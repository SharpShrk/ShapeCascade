using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class EmblemBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject[] _tokenSlots;

    private readonly List<Emblem> _tokens = new List<Emblem>();

    private int _currentSlotIndex = 0;

    public event Action<List<Emblem>> OnTokensMatched;
    public event Action OnGameOver;

    public int CountEmblem => _tokens.Count;

    public void AddToken(Emblem emblem)
    {
        if (emblem == null || _currentSlotIndex >= _tokenSlots.Length)
        {
            CheckForGameOver();
            return;
        }

        GameObject slot = _tokenSlots[_currentSlotIndex];

        if (slot == null)
            return;

        emblem.gameObject.SetActive(false);
        emblem.transform.rotation = Quaternion.identity;
        emblem.transform.position = slot.transform.position;
        emblem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        emblem.gameObject.SetActive(true);

        _tokens.Add(emblem);
        _currentSlotIndex++;

        var matchedTokens = CheckForMatches();

        if (matchedTokens != null)
        {
            HandleMatchedTokens(matchedTokens);
        }
        else if (_currentSlotIndex >= _tokenSlots.Length)
        {
            CheckForGameOver();
        }
    }

    private List<Emblem> CheckForMatches()
    {
        var groupedTokens = _tokens
            .GroupBy(t => (t.Color, t.ShapeType, t.Animal))
            .FirstOrDefault(g => g.Count() == 3);

        return groupedTokens?.ToList();
    }

    private void HandleMatchedTokens(List<Emblem> matchedTokens)
    {
        foreach (var token in matchedTokens)
        {
            _tokens.Remove(token);
            token.gameObject.SetActive(false);
        }

        _currentSlotIndex -= matchedTokens.Count;

        for (int i = 0; i < _tokens.Count; i++)
        {
            if (_tokenSlots[i] != null)
            {
                _tokens[i].transform.position = _tokenSlots[i].transform.position;
            }
        }

        OnTokensMatched?.Invoke(matchedTokens);
    }

    private void CheckForGameOver()
    {
        if (_currentSlotIndex >= _tokenSlots.Length && CheckForMatches() == null)
        {
            OnGameOver?.Invoke();
        }
    }
}