using UnityEngine;

public class Emblem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _colorSpriteRenderer;
    [SerializeField] private SpriteRenderer _animalSpriteRenderer;

    private ShapeType _shapeType;
    private FrameColor _color;
    private Animal _animal;
    private BoardManager _boardManager;

    public ShapeType ShapeType => _shapeType;
    public FrameColor Color => _color;
    public Animal Animal => _animal;

    public void Init(
        ShapeType shapeType,
        FrameColor color,
        Animal animal,
        Sprite colorSprite,
        Sprite animalSprite,
        BoardManager boardManager)
    {
        _shapeType = shapeType;
        _color = color;
        _animal = animal;
        _boardManager = boardManager;

        _colorSpriteRenderer.sprite = colorSprite;
        _animalSpriteRenderer.sprite = animalSprite;
    }

    private void OnMouseDown()
    {
        _boardManager.RemoveEmblem(this);
    }
}