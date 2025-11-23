using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Общий скрипт для порталов, отвечающих за переход между сценами.
/// Должен быть размещен на объекте с соответствующим Тегом ("Portal").
/// </summary>
public class SceneTransitionPortal : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [Tooltip("Имя сцены, которую нужно загрузить при входе в портал.")]
    public string sceneToLoad;
    [Tooltip("Номер текущго уровня")]
    public int currentLevelIndex = 1;

    /// <summary>
    /// Загружает указанную сцену.
    /// Этот метод вызывается из PlayerController при столкновении.
    /// </summary>
    public void LoadNextScene()
    {
        // ВАЖНО: На всякий случай всегда устанавливать Time.timeScale на 1 перед загрузкой сцены,
        // чтобы избежать проблем, если игра была на паузе.
        Time.timeScale = 1f;

        SaveTimeAndComplete();

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Имя сцены для загрузки не указано в SceneTransitionPortal!", this.gameObject);
            return;
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("PortalEnter");
        else
            Debug.Log("Объекта AudioManager не существует.");

        Debug.Log("Портал активирован! Загрузка сцены: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SaveTimeAndComplete()
    {
        // Попытка найти таймер на сцене автоматически
        LevelTimer timer = FindObjectOfType<LevelTimer>();

        if (timer != null)
        {
            // Останавливаем таймер и берем время
            float finalTime = timer.StopAndGetTime();

            // Сохраняем в GameManager
            if (GameManager.Instance != null)
            {
                GameManager.SetFinalTime(finalTime);
                GameManager.Instance.UnlockNextLevel(currentLevelIndex);
                Debug.Log($"Время сохранено, следуюший уровень разблокирован.");
            }
            else
                Debug.LogWarning("GameManager не найден! Время не будет передано на экран победы.");
        }
        else
            Debug.LogWarning("LevelTimer не найден на этой сцене. Время будет 0.");
    }
}