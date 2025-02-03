using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private GameObject coinPrefab; // Prefab de la moneda
    [SerializeField] private List<CoinData> coins; // Lista de monedas
    [SerializeField] private TextMeshProUGUI coinCountText; // UI para mostrar el conteo de monedas
    private int coinCount = 0;

    private string coinPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // No destruir al cargar nuevas escenas
            coinPath = Application.persistentDataPath + "/coinData.json";
            //LoadCoinData();
        }
        else
        {
            Destroy(gameObject); // Asegurarse de que solo haya una instancia
        }
    }

    private void Start()
    {
        LoadCoinData();
        UpdateCoinCountText();
        Debug.Log("Start");
    }
    

    private void UpdateCoinCountText()
    {
        coinCountText.text = "Coins: " + coinCount.ToString();
    }

    private void LoadCoinData()
    {
        if (File.Exists(coinPath))
        {
            string json = File.ReadAllText(coinPath);
            coins = JsonUtility.FromJson<CoinList>(json).coins;

            foreach (var coin in coins)
            {
                if (!coin.collected && coin.levelName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    Instantiate(coinPrefab, coin.position, Quaternion.identity);
                    Debug.Log("Se instancio una moneda");
                }
                else if (coin.collected)
                {
                    coinCount++;
                }
            }
        }
        else
        {
            Debug.LogWarning("No coin data found. Creating new coins.");
            InitializeCoins(); // Método para inicializar las monedas si no hay datos
        }
    }

    private void InitializeCoins()
    {
        SaveCoinData(); // Guarda las monedas iniciales
        LoadCoinData();
    }

    public void CollectCoin(Vector3 position)
    {
        foreach (var coin in coins)
        {
            if (coin.position == position && !coin.collected &&
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == coin.levelName)
            {
                coin.collected = true;
                coinCount++;
                UpdateCoinCountText();
                SaveCoinData(); // Guarda el estado actualizado de las monedas
                break;
            }
        }
    }

    private void SaveCoinData()
    {
        CoinList data = new CoinList { coins = coins };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(coinPath, json);
        Debug.Log("Coin data saved to: " + coinPath);
    }
    public void ResetCoins()
    {
        // Borrar el archivo de datos de las monedas
        if (File.Exists(coinPath))
        {
            File.Delete(coinPath);
            Debug.Log("Coin data deleted.");
        }

        // Reiniciar la lista de monedas y contador
        coins.ForEach(coin => coin.collected = false);
        coinCount = 0;

    }
}

[System.Serializable]
public class CoinList
{
    public List<CoinData> coins;
}

