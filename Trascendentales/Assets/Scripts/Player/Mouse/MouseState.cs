using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState : MonoBehaviour
{
    private bool isLeftClickPress, isRightClickPress;
    private ToolTypes currentTool;

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

    public void SetCurrentToolType(ToolTypes toolType)
    {
        currentTool = toolType;
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
