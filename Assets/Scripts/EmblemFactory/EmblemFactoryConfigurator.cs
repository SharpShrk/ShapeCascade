using UnityEngine;

[CreateAssetMenu(fileName = "ShapeConfigurator", menuName = "ShapeCascade/FactoryConfigurator")]
public class EmblemFactoryConfigurator : ScriptableObject
{
    [SerializeField] private ShapeConfigSO[] _shapeConfigs;
    [SerializeField] private ColorConfigSO[] _colorConfigs;
    [SerializeField] private AnimalConfigSO[] _animalConfigs;

    public ShapeConfigSO[] ShapeConfigs => _shapeConfigs;
    public ColorConfigSO[] ColorConfigs => _colorConfigs;
    public AnimalConfigSO[] AnimalConfigs => _animalConfigs;
}