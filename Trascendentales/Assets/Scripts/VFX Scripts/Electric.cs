﻿using UnityEngine;
using System.Collections;

public class Electric : MonoBehaviour
{
    /// <summary>
    /// Esta clase se encarga de simular el movimiento de rayos para el nexo
    /// </summary>
    private LineRenderer lRend;
    public Transform transformPointA;
    public Transform transformPointB;
    private readonly int pointsCount = 5;
    private readonly int half = 2;
    private float randomness;
    private Vector3[] points;

    private readonly int pointIndexA = 0;
    private readonly int pointIndexB = 1;
    private readonly int pointIndexC = 2;
    private readonly int pointIndexD = 3;
    private readonly int pointIndexE = 4;

    private readonly string mainTexture = "_MainTex";
    private Vector2 mainTextureScale = Vector2.one;
    private Vector2 mainTextureOffset = Vector2.zero;

    private float timer;
    private float timerTimeOut = 0.05f;

    public void Start ()
    {
        lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
        lRend.positionCount = pointsCount;
    }

    private void GenerateMeshCollider(){
        MeshCollider collider = GetComponent<MeshCollider>();
        if(collider == null){
            collider = gameObject.AddComponent<MeshCollider>();
        }
        Mesh mesh = new Mesh();
        lRend.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        CalculatePoints();
        if (timer > timerTimeOut)
        {
            timer = 0;


            lRend.SetPositions(points);
            GenerateMeshCollider();
        }
    }


    private void CalculatePoints()
    {
        points[pointIndexA] = transformPointA.position;
        points[pointIndexE] = transformPointB.position;
        points[pointIndexC] = GetCenter(points[pointIndexA], points[pointIndexE]);
        points[pointIndexB] = GetCenter(points[pointIndexA], points[pointIndexC]);
        points[pointIndexD] = GetCenter(points[pointIndexC], points[pointIndexE]);

        float distance = Vector3.Distance(transformPointA.position, transformPointB.position) / points.Length;
        mainTextureScale.x = distance;
        mainTextureOffset.x = Random.Range(-randomness, randomness);
        lRend.material.SetTextureScale(mainTexture, mainTextureScale);
        lRend.material.SetTextureOffset(mainTexture, mainTextureOffset);

        randomness = distance / (pointsCount * half);

        SetRandomness();
    }

    private void SetRandomness()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i != pointIndexA && i != pointIndexE)
            {
                points[i] += new Vector3(Random.Range(-randomness, randomness), 
                                         Random.Range(-randomness, randomness), 
                                         Random.Range(-randomness, randomness));
            }
        }
    }
    public void SetPointsPosition(Transform point1, Transform point2){
        transformPointA = point1;
        transformPointB = point2;
    }
    private Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / half;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
}