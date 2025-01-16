using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] private List<Pipe> pipes; // List of all pipes
    public bool startActive = false; // Initial state of pipes
    public LayerMask layerMask; // Layer For the colliders of the steam

    void Start()
    {
        foreach (var pipe in pipes)
        {
            if (startActive)
                pipe.EnableSteam(null);
            else
                pipe.DisableSteam();
            pipe.Initialize(this); // Initialize each pipe with a reference to this Pipes instance
        }
    }

    void Update()
    {
        foreach (var pipe in pipes)
        {
            if (pipe.isActive)
                pipe.DrawRaycast(this); // Pass reference to this Pipes instance for communication
        }
    }
    public void CheckAllPipesSources()
    {
        if (pipes.Where(x => x.isActive).All(x => x.sourcePipe == null))
            DeactivateAllPipes();
    }
    public void Activate(Pipe source)
    {
        foreach (var pipe in pipes)
        {
            if (pipe != source && !pipe.isActive)
                pipe.EnableSteam(null);
        }
    }

    public void DeactivateAllPipes()
    {
        foreach (var pipe in pipes)
        {
            pipe.DisableSteam(); // Deactivate all pipes when needed
        }
    }
}

