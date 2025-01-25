using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBehaviour : MonoBehaviour
{
    [System.Serializable]
    private class ClockHandle
    {
        public GameObject prefab;
        public float angleStep = 0;
        public float timeToRotate = 0;
    }
    [SerializeField] private List<ClockHandle> handles;
    [SerializeField] private PaintableSolid rotor;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var handle in handles)
        {
            StartCoroutine(RotateHandle(handle));
        }
    }
    private IEnumerator RotateHandle(ClockHandle handle)
    {
        yield return new WaitForSeconds(handle.timeToRotate);
        if(!rotor.GetComponent<Collider>().isTrigger)
            handle.prefab.transform.Rotate(0,handle.angleStep,0);
        StartCoroutine(RotateHandle(handle));
    }

}
