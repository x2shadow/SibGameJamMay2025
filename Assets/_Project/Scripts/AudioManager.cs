using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Настройки фоновой музыки")]
    public AudioSource bgMusic;

    private void Awake()
    {
        // Реализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // не уничтожается при смене сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
