using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState : MonoBehaviour
{
    private bool isLeftClickPress, isRightClickPress;
    private ToolTypes currentTool;
    private AlternativeToolTypes currentAlternativeTool;

    // El singleton instance
    private static MouseState _instance;

    // Propiedad para acceder a la instancia
    public static MouseState Instance { get; private set; }

    // Método Awake para inicializar el Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsLeftClickPress() => isLeftClickPress;
    public bool IsRightClickPress() => isRightClickPress;
    public ToolTypes CurrentToolActive() => currentTool;
    public AlternativeToolTypes CurrentAlternativeTool() => currentAlternativeTool;

    public void SetCurrentToolType(ToolTypes toolType)
    {
        currentTool = toolType;
    }
    public void SetCurrentAlternativeToolType(AlternativeToolTypes alternativeTool)
    {
        currentAlternativeTool = alternativeTool;
    }

    public void SetLeftclickPress()
    {
        isLeftClickPress = true;
    }
    public void DropLeftClick()
    {
        isLeftClickPress = false;
    }
    public void SetRightclickPress()
    {
        isRightClickPress = true;
    }
    public void DropRightClick()
    {
        isRightClickPress = false;
    }

}
public enum ToolTypes
{
    Brush = 0,
    Ruler = 1,
    Squad = 2,
    Compass = 3,
    Eraser = 4
}
public enum AlternativeToolTypes
{
    BrushBox = 0,
    BrushSphere = 1,
    BrushRectangle = 2,
    RulerLink = 3,
    SquadLink = 4,
    CompassXAxis = 5,
    CompassYAxis = 6,
    CompassZAxis = 7,
    EraserPosition = 8,
    EraserRotation = 9,
    EraserScale = 10
}