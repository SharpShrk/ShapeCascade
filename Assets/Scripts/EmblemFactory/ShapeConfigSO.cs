using UnityEngine;

[CreateAssetMenu(fileName = "ShapeConfig", menuName = "ShapeCascade/ShapeConfig")]
public class ShapeConfigSO : ScriptableObject
{
    [SerializeField] private ShapeType _shapeType;
    [SerializeField] private GameObject _shapePrefab;

    public ShapeType ShapeType => _shapeType;
    public GameObject ShapePrefab => _shapePrefab;
}