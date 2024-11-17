using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFor2D : MonoBehaviour, IObjectAffectableByPerspective
{
    private Button button;
    private CameraManager cameraManager;

    public void OnPerspectiveChanged(bool is2D, bool isFrontView)
    {
        button.interactable = !is2D;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.RegisterObject(this);
    }
}
