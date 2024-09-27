using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCubeTool : MonoBehaviour
{

    public GameObject cubePrefab; // Prefab del cubo a spawnear
    public float lineLifetime = 1.0f; // Tiempo que dura cada posición antes de eliminarse
    public int minPoints = 20; // Cantidad mínima de puntos para validar la forma
    public float squareThreshold = 0.1f; // Umbral de tolerancia para la forma de cuadrado
    public float minSize = 0.5f; // Tamaño mínimo de la caja

    private List<Vector2> points = new List<Vector2>();
    private LineRenderer lineRenderer;
    private bool isDrawing = false;

    void Start()
    {
        // Crear un LineRenderer para visualizar el dibujo
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Inicia el dibujo con click derecho
        {
            isDrawing = true;
            points.Clear();
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButtonUp(1)) // Finaliza el dibujo
        {
            isDrawing = false;
            TrySpawnCube();
        }

        if (isDrawing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AddPoint(mousePosition);
        }

        // Actualizar el dibujo en pantalla
        UpdateLineRenderer();
    }

    void AddPoint(Vector2 point)
    {
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], point) > 0.1f)
        {
            points.Add(point);
        }

        // Limitar el tamaño de la lista para eliminar puntos antiguos
        if (points.Count > minPoints)
        {
            points.RemoveAt(0);
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    void TrySpawnCube()
    {
        if (points.Count < minPoints) return;

        // Obtener el bounding box del dibujo
        Vector2 min = points[0];
        Vector2 max = points[0];
        foreach (Vector2 point in points)
        {
            min = Vector2.Min(min, point);
            max = Vector2.Max(max, point);
        }

        Vector2 size = max - min;
        if (size.x >= minSize && size.y >= minSize)
        {
            float aspectRatio = Mathf.Abs(size.x - size.y) / Mathf.Max(size.x, size.y);

            // Si la forma es lo suficientemente cuadrada
            if (aspectRatio < squareThreshold)
            {
                Vector2 spawnPosition = (min + max) / 2;
                SpawnCube(spawnPosition, size);
            }
        }
    }

    void SpawnCube(Vector2 position, Vector2 size)
    {
        Vector3 spawnPos = new Vector3(position.x, position.y, 0);
        GameObject newCube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
        newCube.transform.localScale = new Vector3(size.x, size.y, 1);
    }

}
