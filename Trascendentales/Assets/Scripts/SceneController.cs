using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //[SerializeField] private Scenes scene;
    //[SerializeField] private AudioClip sceneMusic;
    // El singleton instance

    // Propiedad para acceder a la instancia
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Cursor.visible = true; //Desactivo el mouse

    }
    public void ChangeScene(Scenes scene)
    {
        //GameManager.Instance.SetGameMode(scene);
        string sceneName = scene.ToString(); // Convierte el valor del enum a una cadena (string).
        //AudioManager.Instance.PlayMusic(sceneMusic);
        SceneManager.LoadScene(sceneName);
    }
    public void SetScene(string scene)
    {
        Time.timeScale = 1; // Restaurar el tiempo
        string sceneName = scene.ToString(); // Convierte el valor del enum a una cadena (string).

        SceneManager.LoadScene(sceneName);
    }
    public void ResetScene()
    {
        // Recargar la escena actual
        Time.timeScale = 1; // Restaurar el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void DeleteLoadData()
    {
        SaveSystem.DeletePlayerData();
    }

}

public enum Scenes
{
    MainMenu = 0,
    TestScene = 1,
}
