using UnityEngine;

[CreateAssetMenu(fileName = "AnimalConfig", menuName = "ShapeCascade/AnimalConfig")]
public class AnimalConfigSO : ScriptableObject
{
    [SerializeField] private Animal _animal;
    [SerializeField] private Sprite _animalSprite;

    public Animal Animal => _animal;
    public Sprite AnimalSprite => _animalSprite;
}