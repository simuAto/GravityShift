using UnityEngine;
using UnityEngine.SceneManagement; // ОБЯЗАТЕЛЬНО для управления сценами

/// <summary>
/// Управляет логикой Главного Меню.
/// Отвечает за запуск игры и выход из приложения.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Настройки Сцен")]
    [Tooltip("Имя сцены, которую нужно загрузить при старте (например, 'SampleScene')")]
    public string gameSceneName = "SampleScene";

    /// <summary>
    /// Метод для кнопки "Начать Игру".
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
}
