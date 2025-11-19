using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Глобальный менеджер игры. Отвечает за хранение текущего состояния,
/// таких как финальное время, и управляет переходами между уровнями.
/// Реализует паттерн Singleton + DontDestroyOnLoad для персистентности.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Синглтон ---
    public static GameManager Instance { get; private set; }

    // Данные о финальном времени теперь хранятся в нестатическом поле, 
    // но доступны через статический геттер/сеттер для удобства.
    private float _finalTime = 0.0f;
    public static float FinalTime => Instance != null ? Instance._finalTime : 0.0f;

    private void Awake()
    {
        // Проверка синглтона: если экземпляр уже существует, уничтожить этот дубликат.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager detected, destroying new instance.");
            Destroy(gameObject);
            return;
        }

        // Устанавливаем текущий экземпляр как основной
        Instance = this;

        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager инициализирован.");
    }

    /// <summary>
    /// Сохраняет время, затраченное игроком на прохождение уровня.
    /// </summary>
    /// <param name="time">Финальное время.</param>
    public static void SetFinalTime(float time)
    {
        if (Instance != null)
        {
            Instance._finalTime = time;
            Debug.Log($"GameManager: финальное время {Instance._finalTime:F2} секунд.");
        }
        else
            Debug.LogError("GameManager не существует.");
    }

    /// <summary>
    /// Загружает следующий уровень в последовательности.
    /// </summary>
    public static void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.Contains("Level_"))
        {
            string numberPart = currentSceneName.Replace("Level_", "");

            if (int.TryParse(numberPart, out int currentLevelNumber))
            {
                int nextLevelNumber = currentLevelNumber + 1;
                string nextLevelName = $"Level_{nextLevelNumber:D2}";

                int nextSceneBuildIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{nextLevelName}.unity");

                if (nextSceneBuildIndex != -1)
                {
                    Debug.Log($"Следующий уровень: {nextLevelName}");
                    SceneManager.LoadScene(nextLevelName);
                }
                else
                {
                    Debug.Log($"Следующий уровень '{nextLevelName}' не найден. Переход в главное меню.");
                    SceneManager.LoadScene("MainMenu");
                }
            }
            else
            {
                Debug.LogError("Имя нынешней сцены не соответвует шаблону. Переход в главное меню.");
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
            SceneManager.LoadScene("MainMenu");
    }
}