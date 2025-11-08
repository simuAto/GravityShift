using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Общий скрипт для порталов, отвечающих за переход между сценами.
/// Должен быть размещен на объекте с соответствующим Тегом (например, "Finish").
/// Теперь этот класс является универсальным для перехода на любой следующий уровень или экран.
/// </summary>
public class SceneTransitionPortal : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [Tooltip("Имя сцены, которую нужно загрузить при входе в портал (например, 'FinalScene')")]
    public string sceneToLoad;

    /// <summary>
    /// Загружает указанную сцену.
    /// Этот метод вызывается из PlayerController при столкновении.
    /// </summary>
    public void LoadNextScene()
    {
        // ВАЖНО: На всякий случай всегда устанавливаем Time.timeScale на 1 перед загрузкой сцены,
        // чтобы избежать проблем, если игра была на паузе.
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Имя сцены для загрузки не указано в SceneTransitionPortal!", this.gameObject);
            return;
        }

        Debug.Log("Портал активирован! Загрузка сцены: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}