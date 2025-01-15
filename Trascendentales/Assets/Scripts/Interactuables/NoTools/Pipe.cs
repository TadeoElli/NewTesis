using UnityEngine;

[System.Serializable]
public class Pipe : MonoBehaviour
{
    public Steam pipeSteam; // Reference to the steam logic
    public bool isActive = false; // Indicates if this pipe is active
    public bool isBlocked = false; // Indicates if this pipe is blocked by another pipe
    public Pipe sourcePipe;
    private Pipes parentPipe; // Reference to the parent Pipes class

    public void Initialize(Pipes parent)
    {
        parentPipe = parent;
    }
    public void DrawRaycast(Pipes parentPipe)
    {
        RaycastHit hit;
        float rayDistance = Vector3.Distance(pipeSteam.transform.position, pipeSteam.pointDirection.position);
        if (Physics.Raycast(pipeSteam.transform.position, pipeSteam.pointDirection.position - pipeSteam.transform.position, out hit, rayDistance, parentPipe.layerMask))
        {
            // Check if the hit object is on the "Pipes" layer
            if (hit.collider.gameObject.TryGetComponent<Pipe>(out Pipe pipeComponent))
            {
                if (!pipeComponent.isActive)
                {
                    pipeComponent.EnableSteam(this);
                    isBlocked = true;
                }
                pipeSteam.gasParticles.gameObject.SetActive(false);
            }
        }
        else
        {
            pipeSteam.gasParticles.gameObject.SetActive(true);
            if (sourcePipe != null)
            {
                sourcePipe = null;
                if(!parentPipe.startActive)
                    parentPipe.CheckAllPipesSources();
            }
            isBlocked = false;
        }
    }

    public void EnableSteam(Pipe source)
    {
        isActive = true;
        sourcePipe = source;
        if(source != null)parentPipe.Activate(this); // Notify parent to activate other pipes
        if(!isBlocked)
            pipeSteam.ActivateParticles(); // Activate the particle system
        else
            pipeSteam.gasParticles.Stop();
    }

    public void DisableSteam()
    {
        isActive = false;
        pipeSteam.DesactivateParticles();
    }
}

