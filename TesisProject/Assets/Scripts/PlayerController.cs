using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<ToolTypes> OnToolSwitch;
    public event Action OnPerspectiveSwitch;
    public event Action OnToolInteract;
    public event Action OnToolDesinteract;

    public Rigidbody _rb;

    Player_Move _myMove;

    LayerMask layerMask;
    private void Awake()
    {
        _myMove = new Player_Move(_rb, this);
        layerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        
        #region Tools
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnToolSwitch?.Invoke(ToolTypes.Brush);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnToolSwitch?.Invoke(ToolTypes.Ruler);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnToolSwitch?.Invoke(ToolTypes.Squad);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            OnToolSwitch?.Invoke(ToolTypes.Compass);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            OnToolSwitch?.Invoke(ToolTypes.Eraser);

        if (Input.GetMouseButtonDown(0))
            OnToolInteract?.Invoke();

        if (Input.GetMouseButtonUp(0))
            OnToolDesinteract?.Invoke();
        #endregion

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnPerspectiveSwitch?.Invoke();
            _myMove.CheckPerspective();
        }

        MoveCheck();

        JumpCheck();
    }

    private void MoveCheck()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0)
        {
            _myMove.move?.Invoke(x, y);
        }
        else _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            _myMove.Jump();
    }

    void JumpCheck()
    {
    
        
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit info, 2, layerMask))
        {

            _myMove.canJump = true;
        }
    }
}
