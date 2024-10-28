using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorDictionary : MonoBehaviour
{
    [SerializeField] private List<string> colorNames;  // Lista para los nombres de los colores
    [SerializeField] private List<Color> colors;       // Lista para los colores

    private static Dictionary<string, Color> colorLookupTable;

    private void Awake()
    {
        if (colorLookupTable == null)
        {
            InitializeColors();
        }
    }

    private void InitializeColors()
    {
        // Verifica que ambas listas tengan la misma cantidad de elementos para evitar errores
        if (colorNames.Count != colors.Count)
        {
            Debug.LogError("Las listas 'colorNames' y 'colors' deben tener la misma cantidad de elementos.");
            return;
        }

        // Combina ambas listas en un diccionario usando LINQ
        colorLookupTable = colorNames
            .Zip(colors, (name, color) => new { name, color })  // Une ambos elementos por índice
            .ToDictionary(x => x.name, x => x.color);           // Crea el diccionario con nombres como llave y colores como valor
    }

    // Método estático para obtener un color
    public static Color GetColor(string colorName)
    {
        if (colorLookupTable.TryGetValue(colorName, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning($"Color '{colorName}' no encontrado en el diccionario. Se devolverá Color.white.");
            return Color.white;
        }
    }
}



