using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
    [SerializeField] private Button start;
    [SerializeField] private InputField seed;
    [SerializeField] private GameObject loadingScreen;

    private void Start() {
        seed.text = "42";
        start.onClick.AddListener(OnPress);
        loadingScreen.SetActive(false);
    }

    private void OnPress() {
        loadingScreen.SetActive(true);
        World.Seed = seed.text.GetHashCode();
        SceneManager.LoadScene("World");
    }
}
