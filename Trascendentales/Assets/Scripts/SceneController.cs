using UnityEditor.SearchService;
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
        string sceneName = scene.ToString(); // Convierte el valor del enum a una cadena (string).
        SceneManager.UnloadSceneAsync(sceneName);

        //AudioManager.Instance.PlayMusic(sceneMusic);
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            return;
        }
        SceneManager.LoadScene(sceneName);

    }

}

public enum Scenes
{
    MainMenu = 0,
    TestScene = 1,
}
