using UnityEngine;
using System;

/// <summary>
/// Универсальный класс для хранения пары Имя-АудиоКлип.
/// [System.Serializable] позволяет редактировать это в инспекторе.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name; // Имя, по которому будет вызываться звук
    public AudioClip clip; // Сам аудиофайл
}

/// <summary>
/// Управляет всей фоновой музыкой (BGM) и звуковыми эффектами (SFX).
/// Использует паттерн Синглтон (один объект, досутпен для всей программы) 
/// и DontDestroyOnLoad, чтобы существовать между сценами.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // --- Синглтон ---
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Источник для фоновой музыки (должен быть в режиме Loop)")]
    public AudioSource bgmSource;
    [Tooltip("Источник для звуковых эффектов (не в режиме Loop)")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    [Tooltip("Музыка, играющая на фоне (BGM)")]
    public AudioClip backgroundMusic;
    [Tooltip("Список всех звуковых эффектов (SFX)")]
    public Sound[] sfxList;


    private void Awake()
    {
        // --- Настройка Синглтона ---
        if (Instance == null)
        {
            Instance = this;
            // Объект перестает уничтожаться при смене сцен
            DontDestroyOnLoad(gameObject);

            // Подписка на событие
            GameObject playerObjSubscribe = GameObject.FindWithTag("Player");
            if (playerObjSubscribe != null)
            {
                PlayerController player = playerObjSubscribe.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.OnGravitySwitched += HandleGravitySwitch;
                }
            }
        }
        else
        {
            // Если менеджер уже существует,
            // то этот дубликат уничтожается
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            // Отписка от события
            GameObject playerObjUnsubscribe = GameObject.FindWithTag("Player");
            if (playerObjUnsubscribe != null)
            {
                PlayerController player = playerObjUnsubscribe.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.OnGravitySwitched -= HandleGravitySwitch;
                }
            }
        }
    }

    private void Start()
    {
        // Назначение и запуск фоновой музыки
        if (backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// Проигрывает звуковой эффект (SFX) по его имени.
    /// </summary>
    /// <param name="name">Имя звука из списка sfxList</param>
    public void PlaySFX(string name)
    {
        // Поиск звука в массиве sfxList по имени
        Sound s = Array.Find(sfxList, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("AudioManager: Звук с именем '" + name + "' не найден!");
            return;
        }

        // Используется PlayOneShot, чтобы звуки могли накладываться
        sfxSource.PlayOneShot(s.clip);
    }

    /// <summary>
    /// Обработчик события, проигрывает звук при смене гравитации
    /// </summary>
    /// <param name="isSwitched"></param>
    private void HandleGravitySwitch(bool isSwitched)
    {
        PlaySFX("GravitySwitch");
    }
}