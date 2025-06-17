using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Borders")]
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _bottomWall;

    [Header("Settings")]
    [SerializeField] private EmblemFactory _emblemFactory;
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private LayerMask _emblemLayer;
    [SerializeField] private BoxCollider2D _checkArea;
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private float _velocityThreshold = 0.1f;

    private readonly List<Emblem> _availableEmblems = new List<Emblem>();
    private readonly List<Emblem> _spawnedEmblems = new List<Emblem>();
    private readonly List<GameObject> _availableSpawnPoints = new List<GameObject>();

    private GameObject _lastSpawnPoint;
    private UniTask _spawningTask;
    private bool _isSpawning = true;
    private bool _isInitialFill;
    private bool _isTaskRunning;

    private void Awake()
    {
        InitializeSpawnPoints();
    }

    private void OnEnable()
    {
        _emblemFactory.OnInitializationCompleted += InitializeEmblems;
    }

    private void OnDisable()
    {
        _emblemFactory.OnInitializationCompleted -= InitializeEmblems;
    }

    private async void InitializeEmblems(List<Emblem> emblems)
    {
        _availableEmblems.AddRange(emblems);
        _isInitialFill = true;
        await StartSpawningAsync();
    }

    private void InitializeSpawnPoints()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            return;
        }

        _availableSpawnPoints.Clear();
        _availableSpawnPoints.AddRange(_spawnPoints);
    }

    private async UniTask StartSpawningAsync()
    {
        if (_isTaskRunning)
        {
            return;
        }

        _isTaskRunning = true;
        try
        {
            while (_isSpawning && _availableEmblems.Count > 0)
            {
                if (!IsBoardFull())
                {
                    SpawnEmblem();
                }
                else
                {
                    _isSpawning = false;
                    break;
                }

                await UniTask.Delay(System.TimeSpan.FromSeconds(_spawnInterval));
            }
        }
        finally
        {
            _isTaskRunning = false;
            _spawningTask = UniTask.CompletedTask; // пригодится - но пока не нужна.
        }
    }

    private void SpawnEmblem()
    {
        if (_availableEmblems.Count == 0)
        {
            return;
        }

        Emblem emblem = _availableEmblems[0];
        _availableEmblems.RemoveAt(0);
        _spawnedEmblems.Add(emblem);

        GameObject spawnPoint = SelectSpawnPoint();

        emblem.transform.position = spawnPoint.transform.position;
        emblem.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        emblem.gameObject.SetActive(true);
        Rigidbody2D rb = emblem.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private GameObject SelectSpawnPoint()
    {
        if (_availableSpawnPoints.Count == 0)
        {
            return null;
        }

        if (_availableSpawnPoints.Count == 1)
        {
            _lastSpawnPoint = _availableSpawnPoints[0];
            return _lastSpawnPoint;
        }

        List<GameObject> tempSpawnPoints = new List<GameObject>(_availableSpawnPoints);
        if (_lastSpawnPoint != null)
        {
            tempSpawnPoints.Remove(_lastSpawnPoint);
        }

        GameObject selectedPoint = tempSpawnPoints[Random.Range(0, tempSpawnPoints.Count)];
        _lastSpawnPoint = selectedPoint;
        return selectedPoint;
    }

    private bool IsBoardFull()
    {
        if (_checkArea == null)
            return false;

        Vector2 checkPoint = _checkArea.transform.position;
        Vector2 checkSize = _checkArea.size;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(checkPoint, checkSize, 0f, Vector2.zero, 0f, _emblemLayer);

        if (!_isInitialFill)
            return hits.Length > 0;

        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.magnitude < _velocityThreshold)
            {
                _isInitialFill = false;
                return true;
            }
        }

        return false;
    }

    public void RemoveEmblem(Emblem emblem)
    {
        if (_spawnedEmblems.Contains(emblem))
        {
            _spawnedEmblems.Remove(emblem);
            _availableEmblems.Insert(0, emblem);
            emblem.gameObject.SetActive(false);

            if (!_isSpawning && !IsBoardFull() && _availableEmblems.Count > 0)
            {
                _isSpawning = true;
                StartSpawningAsync().Forget();
            }
        }
    }
}