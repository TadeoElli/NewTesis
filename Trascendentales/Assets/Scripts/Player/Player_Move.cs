using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move
{
    public delegate void MovementByCamera(float x, float y);

    public MovementByCamera move;

    Rigidbody _rb;

    PlayerController _myControll;

    float _speed = 5f;

    public bool canJump = true;

    public Player_Move(Rigidbody rb, PlayerController controller, float speed)
    {
        _myControll = controller;
        _rb = rb;
        CheckPerspective();
        _speed = speed;
    }

    public void CheckPerspective()
    {
        if (Camera.main != Camera.main.orthographic)
        {
            move = PerspeView;
            return;
        }
        move = OrtoView;
    }

    public void PerspeView(float x, float y)
    {
        Vector3 _move = (new Vector3(x, 0, y).normalized) * _speed;
        _rb.velocity = new Vector3(_move.x, _rb.velocity.y, _move.z);
        //Debug.Log(_move);
    }

    public void OrtoView(float x, float y)
    {
        Vector3 _move = (new Vector3(x, 0, y).normalized) * _speed;
        _rb.velocity = new Vector3(_move.x, _rb.velocity.y, 0);
    }

    public void Jump()
    {
        if (canJump)
        {
            _rb.AddForce(new Vector3(0, 300, 0));
            canJump = false;
        }
    }



}
