using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Balance_Atack_Method : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Balance_Atack_Method _secondBalance;

    [SerializeField] float _speedAt;

    [SerializeField] float _fallSpeed;

    [SerializeField] float _positionXMin;

    public delegate void miMetodo();
    private int _successCount = 0;

    miMetodo _atack;

    bool isFall = true;

    bool _canAt = true;

    private void Start()
    {
        _atack = ChangePosition;

    }

    private void Update()
    {
        if(_target.position.x >= _positionXMin)
            _atack();

    }

    void FallingRise()
    {

        if (isFall)
        {
            transform.position += Vector3.down * _fallSpeed * Time.deltaTime;
            if (transform.position.y <= -0.5) isFall = false;
        }
        else
        {
            transform.position += Vector3.up * _fallSpeed / 2 * Time.deltaTime;

            if (transform.position.y >= 5)
            {
                _atack = ChangePosition;
                print("cambio pos");
                _canAt = true;
            }
        }
    }

    void ChangePosition()
    {
        if (!_canAt) return;
        float x = Random.Range(-3f, 3f);
        float z = Random.Range(-3f, 3f);

        transform.position = new Vector3(_target.position.x + x, transform.position.y, _target.position.z + z);
        StartCoroutine(TimerRoutine());
        _canAt = false;
        isFall = true;
    }

    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(_speedAt);
        transform.position =new Vector3(transform.position.x, 5, transform.position.z);
        _atack = FallingRise;
    }
    public void IncreaseSpeed()
    {
        _successCount++;
        _speedAt = _speedAt - 0.3f;
        _fallSpeed = _fallSpeed + 1f;
        if(_successCount > 2)
        {
            if(_secondBalance !=null && !_secondBalance.gameObject.activeSelf)
                _secondBalance.gameObject.SetActive(true);
            _secondBalance.SetValues(_speedAt - 1f,_fallSpeed);
        }
    }
    public void SetValues(float speedAt,float fallSpeed)
    {
        _speedAt = speedAt;
        _fallSpeed = fallSpeed;
    }


}
