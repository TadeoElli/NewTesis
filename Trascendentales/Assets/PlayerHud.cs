using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static EraserWithSelectionTool;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private GameObject[] livesIcon;
    [SerializeField] private Image toolIcon, alternativeToolIcon;
    [Header("ToolsIcon")]
    [SerializeField] private Sprite brushIcon;
    [SerializeField] private Sprite rulerIcon, squadIcon, compassIcon, eraserIcon;
    [Header("BrushAlternativeIcon")]
    [SerializeField] private Sprite brushAlternativeBox;
    [SerializeField] private Sprite brushAlternativeSphere, brushAlternativeRectangle;
    [Header("CompassAlternativeIcon")]
    [SerializeField] private Sprite compassAlternativeXAxis;
    [SerializeField] private Sprite compassAlternativeYAxis, compassAlternativeZAxis;
    [Header("EraserAlternativeIcon")]
    [SerializeField] private Sprite eraserAlternativePosition;
    [SerializeField] private Sprite eraserAlternativeRotation, eraserAlternativeScale;

    private int lives;

    private void Awake()
    {
        lives = livesIcon.Count();
    }
    public void ReduceLive()
    {
        livesIcon[lives].SetActive(false);
    }
    public void UpdateToolIcon()
    {
        switch (MouseState.Instance.CurrentToolActive())
        {
            case ToolTypes.Brush:
                toolIcon.sprite = brushIcon;
                break;
            case ToolTypes.Ruler:
                toolIcon.sprite = rulerIcon;
                break;
            case ToolTypes.Squad:
                toolIcon.sprite = squadIcon;
                break;
            case ToolTypes.Compass:
                toolIcon.sprite = compassIcon;
                break;
            case ToolTypes.Eraser:
                toolIcon.sprite = eraserIcon;
                break;
            default: break;
        }
        switch (MouseState.Instance.CurrentAlternativeTool())
        {
            case AlternativeToolTypes.BrushBox:
                alternativeToolIcon.sprite = brushAlternativeBox;
                break;
            case AlternativeToolTypes.BrushSphere:
                alternativeToolIcon.sprite = brushAlternativeSphere;
                break;
            case AlternativeToolTypes.BrushRectangle:
                alternativeToolIcon.sprite = brushAlternativeRectangle;
                break;
            case AlternativeToolTypes.RulerLink:
                alternativeToolIcon.sprite = rulerIcon;
                break;
            case AlternativeToolTypes.SquadLink:
                alternativeToolIcon.sprite = squadIcon;
                break;
            case AlternativeToolTypes.CompassXAxis:
                alternativeToolIcon.sprite = compassAlternativeXAxis;
                break;
            case AlternativeToolTypes.CompassYAxis:
                alternativeToolIcon.sprite = compassAlternativeYAxis;
                break;
            case AlternativeToolTypes.CompassZAxis:
                alternativeToolIcon.sprite = compassAlternativeZAxis;
                break;
            case AlternativeToolTypes.EraserPosition:
                alternativeToolIcon.sprite = eraserAlternativePosition;
                break;
            case AlternativeToolTypes.EraserRotation:
                alternativeToolIcon.sprite = eraserAlternativeRotation;
                break;
            case AlternativeToolTypes.EraserScale:
                alternativeToolIcon.sprite = eraserAlternativeScale;
                break;
            default:
                break;
        }
    }
}