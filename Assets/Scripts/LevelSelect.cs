using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Обязательно для работы с кнопками

public class LevelSelectManager : MonoBehaviour
{
    [Tooltip("Кнопки уровней")]
    public Button[] levelButtons;

    void Start()
    {
        // Получаем номер самого высокого открытого уровня (по умолчанию 1)
        int reachedLevel = 1;

        if (GameManager.Instance != null)
            reachedLevel = GameManager.Instance.GetReachedLevel();

        // Проходимся по всем кнопкам
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Индекс уровня = индекс массива + 1
            int levelNum = i + 1;

            if (levelNum > reachedLevel)
            {
                levelButtons[i].interactable = false;

                // Опционально: можно затемнить цвет или повесить иконку замка
                // levelButtons[i].image.color = Color.gray;
            }
            else
                levelButtons[i].interactable = true;
        }
    }

    /// <summary>
    /// Метод для загрузки уровня по имени.
    /// </summary>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Метод для загрузки главного меню.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}