using UnityEngine;

/// <summary>
/// PlayerController: Отвечает за движение игрока (горизонтальное) и
/// ключевую механику игры — переключение гравитации.
/// Использует компоненты Rigidbody для физики.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // --- Настраиваемые параметры (Отображаются в инспекторе Unity) ---

    [Header("Настройки движения")]
    [Tooltip("Скорость горизонтального перемещения игрока.")]
    public float moveSpeed = 5f;

    [Header("Настройки гравитации")]
    [Tooltip("Сила гравитации, приложенная к игроку.")]
    public float gravityForce = 9.81f;

    // --- Приватные поля ---

    // Компонент Rigidbody игрока для управления физикой.
    private Rigidbody rb;

    // Текущее направление гравитации. По умолчанию -Y (вниз).
    private Vector3 currentGravityDirection = Vector3.down;

    // Флаг для предотвращения многократного переключения гравитации за короткое время.
    private bool canChangeGravity = true;
    private float gravityCooldown = 0.5f; // Задержка между переключениями.

    // --- Методы Unity ---

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleGravitySwitchInput();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        HandleMovement();
    }

    // --- Логика ввода и механик ---

    /// <summary>
    /// Применяет настроенную гравитацию к Rigidbody.
    /// </summary>
    private void ApplyGravity()
    {
        Vector3 gravityVector = currentGravityDirection * gravityForce;
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }

    /// <summary>
    /// Обрабатывает горизонтальное движение.
    /// </summary>
    private void HandleMovement()
    {
        // Получаем ввод по горизонтальной оси.
        float horizontalInput = Input.GetAxis("Horizontal");

        // Если нет ввода, выходим.
        if (Mathf.Approximately(horizontalInput, 0f))
        {
            return;
        }

        // --- ФИКС ИНВЕРСИИ ДВИЖЕНИЯ ---

        // 1. Определяем, какая ось является "вверх" (направление, противоположное гравитации).
        Vector3 upVector = -currentGravityDirection;

        // 2. Определяем направление "вправо" (перпендикулярно оси "вверх").
        // Мы используем Quaternion.FromToRotation для создания корректного горизонтального направления
        // независимо от текущего поворота куба, но с учетом новой плоскости гравитации.

        // В нашем случае (гравитация только вверх/вниз по оси Y), направление движения всегда
        // будет вдоль глобальной оси X (Vector3.right).

        // Использование Vector3.right для движения (даже после поворота)
        // гарантирует, что D всегда будет двигать "вправо", а A — "влево".
        Vector3 movementDirection = Vector3.right;

        // Мы должны гарантировать, что мы движемся по плоскости, перпендикулярной гравитации.
        // Vector3.ProjectOnPlane проецирует наш желаемый вектор (Vector3.right) на плоскость,
        // нормаль которой является нашей силой гравитации.
        movementDirection = Vector3.ProjectOnPlane(movementDirection, currentGravityDirection).normalized;

        // Создаем вектор горизонтальной скорости.
        Vector3 horizontalVelocity = movementDirection * (horizontalInput * moveSpeed);

        // Устанавливаем новую скорость Rigidbody:
        // Мы хотим сохранить только ВЕРТИКАЛЬНУЮ (гравитационную) скорость.
        Vector3 currentVerticalSpeed = Vector3.Project(rb.velocity, currentGravityDirection);

        // Новая скорость = Горизонтальная + Вертикальная
        rb.velocity = horizontalVelocity + currentVerticalSpeed;
    }

    /// <summary>
    /// Обрабатывает нажатие клавиши для переключения гравитации.
    /// Используется клавиша 'Space' или 'E'.
    /// </summary>
    private void HandleGravitySwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canChangeGravity)
        {
            SwitchGravity();
            canChangeGravity = false;
            Invoke(nameof(ResetGravityCooldown), gravityCooldown);
        }
    }

    /// <summary>
    /// Переключает гравитацию на противоположное направление.
    /// </summary>
    private void SwitchGravity()
    {
        // Инвертируем текущее направление гравитации.
        currentGravityDirection *= -1;

        // Наше новое "вверх" — это направление, противоположное текущей гравитации.
        Vector3 newUp = -currentGravityDirection;

        // Вычисляем вращение.
        // Quaternion.LookRotation — это надежный способ повернуть объект.
        // ВАЖНО: Мы сохраняем текущее направление transform.forward, чтобы не сбивать камеру.
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, newUp);
        transform.rotation = targetRotation;

        Debug.Log("Гравитация переключена! Новое направление: " + currentGravityDirection);

        // Сброс вертикальной скорости.
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, currentGravityDirection);
    }

    /// <summary>
    /// Сбрасывает флаг кулдауна, разрешая следующее переключение гравитации.
    /// </summary>
    private void ResetGravityCooldown()
    {
        canChangeGravity = true;
    }
}
