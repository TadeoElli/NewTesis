using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneUse : MonoBehaviour
{
    [SerializeField] GameObject StartCanvas;
    void Start()
    {
        StartCanvas.SetActive(true);

        StartCanvas.GetComponent<DialogueSysterm>().Start_Dialogues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
