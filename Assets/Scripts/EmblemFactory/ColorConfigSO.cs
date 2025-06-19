using UnityEngine;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "ShapeCascade/ColorConfig")]
public class ColorConfigSO : ScriptableObject
{
    [SerializeField] private FrameColor _color;
    [SerializeField] private Sprite _gradientSprite;

    public FrameColor Color => _color;
    public Sprite GradientSprite => _gradientSprite;
}