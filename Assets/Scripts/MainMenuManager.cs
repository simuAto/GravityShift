using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно для управления сценами

/// <summary>
/// Управляет логикой главного меню.
/// Отвечает за запуск игры и выход из приложения.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Настройки Сцен")]
    [Tooltip("Имя сцены, которую нужно загрузить при старте")]
    public string gameSceneName;

    /// <summary>
    /// Метод для кнопки "Начать игру".
    /// Загружает игровую сцену.
    /// </summary>
    public void StartGame()
    {
        // Проверяем, что имя сцены не пустое
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("Имя игровой сцены не указано в MainMenuManager!");
            return;
        }

        Debug.Log("Загрузка сцены: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Метод для кнопки "Выход".
    /// Закрывает приложение.
    /// </summary>
    public void ExitGame()
    {
        // Application.Quit() не работает в редакторе Unity,
        // поэтому мы выводим сообщение в консоль для теста.
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

    /// <summary>
    /// Метод для кнопки "Уровни".
    /// Открывает меню выбора уровней.
    /// </summary>
    public void OpenLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
