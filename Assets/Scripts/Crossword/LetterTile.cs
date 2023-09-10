using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    [field: SerializeField]
    public Text LetterDisplay { get; private set; }

    [field: SerializeField]
    public Image BackgroundImage { get; private set; }

    public bool IsHinted { get; set; } = false;

    public bool IsFound { get; set; } = false;

    [SerializeField]
    private Color _textColor;

    [SerializeField]
    private Color _tileColor;

    [SerializeField]
    private Color _hintedTextColor;

    [SerializeField]
    private Color _hintedTileColor;

    [SerializeField]
    private Color _foundTextColor;

    [SerializeField]
    private Color _foundTileColor;


    public void SetColors(Color textColor, Color tileColor, Color hintTextColor, Color hintTileColor, Color foundTextColor, Color foundTileColor)
    {
        _textColor = textColor;
        _tileColor = tileColor;
        _hintedTextColor = hintTextColor;
        _hintedTileColor = hintTileColor;
        _foundTextColor = foundTextColor;
        _foundTileColor = foundTileColor;
    }

    public void UpdateColors()
    {
        (Color, Color) stateColors = GetStateColors();

        LetterDisplay.color = stateColors.Item1;
        BackgroundImage.color = stateColors.Item2;
    }

    private (Color, Color) GetStateColors()
    {
        if (IsFound)
            return (_foundTextColor, _foundTileColor);

        if (IsHinted)
            return (_hintedTextColor, _hintedTileColor);

        return (_textColor, _tileColor);
    }

    public void AnimateColors(float animationDuration)
    {
        Color startTextColor = LetterDisplay.color;
        Color startTileColor = BackgroundImage.color;

        // Get the colors of the tiles current state.
        (Color, Color) stateColors = GetStateColors();
        Color endTextColor = stateColors.Item1;
        Color endTileColor = stateColors.Item2;

        // The delegate to run each update of the animation.
        Action<float> updateColors = (value) =>
        {
            LetterDisplay.color = Color.Lerp(startTextColor, endTextColor, value);
            BackgroundImage.color = Color.Lerp(startTileColor, endTileColor, value);
        };

    }

}