using UnityEngine;
using TMPro;

/// <summary>
/// Отсчитывает время, прошедшее с момента загрузки уровня.
/// Устанавливается на любом активном объекте на уровне.
/// </summary>
public class LevelTimer : MonoBehaviour
{
    [Tooltip("Ссылка на текстовый элемент UI для отображения времени.")]
    public TextMeshProUGUI timeDisplay;

    private float _timeElapsed = 0f;
    private bool _isTimerRunning = true;

    /// <summary>
    /// Возвращает текущее прошедшее время.
    /// </summary>
    public float TimeElapsed => _timeElapsed;

    void Update()
    {
        if (_isTimerRunning)
        {
            // Увеличиваем время
            _timeElapsed += Time.deltaTime;

            // Отображаем время на экране, форматируем до двух знаков после запятой
            if (timeDisplay != null)
            {
                timeDisplay.text = $"ВРЕМЯ: {_timeElapsed:F2}";
            }
        }
    }

    /// <summary>
    /// Останавливает таймер и возвращает финальное время.
    /// </summary>
    public float StopAndGetTime()
    {
        _isTimerRunning = false;
        return _timeElapsed;
    }
}