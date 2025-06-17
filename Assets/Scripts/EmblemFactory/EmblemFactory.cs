using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EmblemFactory : MonoBehaviour
{
    [SerializeField] private EmblemFactoryConfigurator _configurator;
    [SerializeField] private BoardManager _boardManager;

    private const int EmblemsPerFrame = 10;

    private readonly List<Emblem> _emblems = new List<Emblem>();

    public event Action<List<Emblem>> OnInitializationCompleted;

    private async void Awake()
    {
        await InitializeEmblemsAsync();
        ShuffleEmblems();
    }

    private async UniTask InitializeEmblemsAsync()
    {
        int createdCount = 0;

        foreach (var shapeConfig in _configurator.ShapeConfigs)
        {
            foreach (var colorConfig in _configurator.ColorConfigs)
            {
                foreach (var animalConfig in _configurator.AnimalConfigs)
                {
                    for (int i = 0; i < _configurator.DuplicateTripletCount; i++)
                    {
                        CreateEmblemInstance(shapeConfig, colorConfig, animalConfig);
                        createdCount++;

                        if (createdCount % EmblemsPerFrame == 0)
                        {
                            await UniTask.Yield();
                        }
                    }
                }
            }
        }
    }

    private void CreateEmblemInstance(ShapeConfigSO shapeConfig, ColorConfigSO colorConfig, AnimalConfigSO animalConfig)
    {
        GameObject emblemObject = Instantiate(shapeConfig.ShapePrefab, Vector3.zero, Quaternion.identity, transform);
        Emblem emblem = emblemObject.GetComponent<Emblem>();
        emblemObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        emblem.Init(
            shapeType: shapeConfig.ShapeType,
            color: colorConfig.Color,
            animal: animalConfig.Animal,
            colorSprite: colorConfig.GradientSprite,
            animalSprite: animalConfig.AnimalSprite,
            _boardManager
        );

        emblemObject.SetActive(false);
        _emblems.Add(emblem);
    }

    private void ShuffleEmblems()
    {
        int n = _emblems.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (_emblems[i], _emblems[j]) = (_emblems[j], _emblems[i]);
        }

        OnInitializationCompleted.Invoke(_emblems);
    }
}