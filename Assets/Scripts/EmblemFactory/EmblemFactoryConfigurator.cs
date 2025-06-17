using UnityEngine;

[CreateAssetMenu(fileName = "ShapeConfigurator", menuName = "ShapeCascade/FactoryConfigurator")]
public class EmblemFactoryConfigurator : ScriptableObject
{
    [SerializeField] private ShapeConfigSO[] _shapeConfigs;
    [SerializeField] private ColorConfigSO[] _colorConfigs;
    [SerializeField] private AnimalConfigSO[] _animalConfigs;
    [SerializeField] private TripletCount _duplicateTripletCount;

    public ShapeConfigSO[] ShapeConfigs => _shapeConfigs;
    public ColorConfigSO[] ColorConfigs => _colorConfigs;
    public AnimalConfigSO[] AnimalConfigs => _animalConfigs;
    public int DuplicateTripletCount => (int)_duplicateTripletCount;

    private enum TripletCount
    {
        One = 3,
        Two = 6,
        Three = 9,
        Four = 12,
        Five = 15
    }
}