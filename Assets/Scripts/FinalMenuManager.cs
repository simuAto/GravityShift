using UnityEngine;
using TMPro; // Для работы с компонентами TextMeshPro
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет экраном победы, отображая финальное время и предоставляя опции.
/// </summary>
public class WinMenuManager : MonoBehaviour
{
    [Tooltip("Текстовый элемент для отображения финального времени.")]
    public TextMeshProUGUI timeTextDisplay;

    void Start()
    {
        DisplayFinalTime();
    }

    /// <summary>
    /// Извлекает время из GameManager и отображает его.
    /// </summary>
    private void DisplayFinalTime()
    {
        float finalTime = GameManager.FinalTime;

        if (timeTextDisplay != null)
        {
            // Проверяем, было ли время успешно сохранено (т.е. не равно 0.0f)
            if (finalTime > 0.0f)
                timeTextDisplay.text = $"ПОБЕДА!\nВремя прохождения: {finalTime:F2} с";
            else
                timeTextDisplay.text = "Ошибка: данные времени не найдены.";
        }
    }

    /// <summary>
    /// Вызывается кнопкой "Следующий уровень".
    /// </summary>
    public void OnNextLevelButtonClicked()
    {
        // Используем логику перехода в GameManager, который знает, какой уровень следующий
        GameManager.LoadNextLevel();
    }

    /// <summary>
    /// Вызывается кнопкой "Главное меню".
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}