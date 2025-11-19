using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Общий скрипт для порталов, отвечающих за переход между сценами.
/// Должен быть размещен на объекте с соответствующим Тегом (например, "Portal").
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
        // ВАЖНО: На всякий случай всегда устанавливать Time.timeScale на 1 перед загрузкой сцены,
        // чтобы избежать проблем, если игра была на паузе.
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("PortalEnter");
            else
                Debug.Log("Объекта AudioManager не существует.");

            Debug.LogError("Имя сцены для загрузки не указано в SceneTransitionPortal!", this.gameObject);
            return;
        }

        Debug.Log("Портал активирован! Загрузка сцены: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}