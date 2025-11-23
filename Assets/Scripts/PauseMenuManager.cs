using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет логикой Меню Паузы внутри игровой сцены.
/// Отвечает за остановку времени, показ/скрытие UI и обработку кнопок.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [Header("Настройки UI")]
    [Tooltip("Панель UI, которая содержит все элементы меню паузы")]
    public GameObject pauseMenuUI; // Ссылка на саму панель меню

    [Header("Настройки Сцен")]
    [Tooltip("Имя сцены Главного Меню для загрузки")]
    public string mainMenuSceneName = "MainMenu";

    // Приватная переменная для отслеживания состояния паузы
    private bool isPaused = false;

    void Start()
    {
        // Убедимся, что при старте уровня меню выключено и игра идет
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 1.0f - нормальная скорость времени
        isPaused = false;
    }

    void Update()
    {
        // Слушаем нажатие клавиши 'Escape'
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Если уже на паузе - возобновляем
                ResumeGame();
            }
            else
            {
                // Если не на паузе - ставим на паузу
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Метод для кнопки "Продолжить".
    /// </summary>
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false;
        Debug.Log("Игра возобновлена.");
    }

    /// <summary>
    /// Приватный метод для постановки на паузу.
    /// </summary>
    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ОСТАНАВЛИВАЕМ ВРЕМЯ
        isPaused = true;
        Debug.Log("Игра на паузе.");
    }

    /// <summary>
    /// Метод для кнопки "Начать Уровень Заново".
    /// </summary>
    public void RestartLevel()
    {
        // ВАЖНО: Перед загрузкой сцены всегда возвращайте Time.timeScale в 1!
        Time.timeScale = 1f;

        // Получаем имя текущей активной сцены и загружаем ее
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Метод для кнопки "Выйти в Главное Меню".
    /// </summary>
    public void LoadMainMenu()
    {
        // ВАЖНО: Перед загрузкой сцены всегда возвращайте Time.timeScale в 1!
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
