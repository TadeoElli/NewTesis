using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    public event Action<ToolTypes> OnToolSwitch;
    public event Action OnToolSwitchCheck;
    public event Action OnPerspectiveSwitch;
    public event Action OnLeftClickPress;
    public event Action OnLeftClickDrop;
    public event Action OnRightClickPress;
    public event Action OnRightClickDrop;
    public event Action OnChangeCameraToLeft;
    public event Action OnChangeCameraToRight;


    private Vector3 spawnPosition;
    Player_Move _myMove;

    private void Awake()
    {
        LoadPlayer();
        _myMove = GetComponent<Player_Move>();
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if(data == null )
            return;
        Vector3 position;
        position.x = data.lastPosition[0];
        position.y = data.lastPosition[1];
        position.z = data.lastPosition[2];
        transform.position = position;  
    }

    private void Update()
    {

        #region Tools
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnToolSwitch?.Invoke(ToolTypes.Brush);
            OnToolSwitchCheck?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnToolSwitch?.Invoke(ToolTypes.Ruler);
            OnToolSwitchCheck?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnToolSwitch?.Invoke(ToolTypes.Squad);
            OnToolSwitchCheck?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnToolSwitch?.Invoke(ToolTypes.Compass);
            OnToolSwitchCheck?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnToolSwitch?.Invoke(ToolTypes.Eraser);
            OnToolSwitchCheck?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
            OnLeftClickPress?.Invoke();

        if (Input.GetMouseButtonUp(0))
            OnLeftClickDrop?.Invoke();

        if (Input.GetMouseButtonDown(1))
            OnRightClickPress?.Invoke();

        if (Input.GetMouseButtonUp(1))
            OnRightClickDrop?.Invoke();
        #endregion

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _myMove.SwitchPerspective();
            OnPerspectiveSwitch?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if (!Input.GetKeyDown(KeyCode.E))
        {
            OnChangeCameraToRight?.Invoke();
        }
        if (!Input.GetKeyDown(KeyCode.Q))
        {
            OnChangeCameraToLeft?.Invoke();
        }


    }
    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
    }


    public void Takedmg(int dmg)
    {
        StartCoroutine(dmgVisual());
    }

    IEnumerator dmgVisual()
    {
            this.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
            this.GetComponent<Renderer>().material.color = Color.yellow;

        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            transform.SetParent(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.SetParent(null);
        }
    }
}