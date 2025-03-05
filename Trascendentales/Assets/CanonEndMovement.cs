using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanonEndMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] RotationEnvironmentObjects checker1, checker2;
    public UnityEvent OnEnd, OnEnd2;
    bool once = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (once)
            return;
        if (checker1.isActiveAndEnabled && checker2.isActiveAndEnabled)
        {
            StartCoroutine(ShowEndClip());
            once = true;
        }
    }
    private IEnumerator ShowEndClip()
    {
        OnEnd.Invoke();
        yield return new WaitForSeconds(4);
        OnEnd2?.Invoke();
    }
}
