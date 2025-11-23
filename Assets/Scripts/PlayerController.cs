using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет всеми механиками игрока:
/// 1. Горизонтальное движение (A/D)
/// 2. Переключение гравитации (Space)
/// 3. Обработка столкновений (Смерть, Победа)
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Настройки Движения")]
    [Tooltip("Скорость горизонтального движения")]
    public float moveSpeed = 5f;
    [Tooltip("Сила гравитации/прыжка")]
    public float gravityForce = 9.81f;
    [Tooltip("Направление движения (для отладки)")]
    public float moveInput;
    [Tooltip("Координата Y, ниже которой игрок считается 'упавшим'")]
    public float yBottomLimit = -10f;
    [Tooltip("Координата Y, выше которой игрок считается 'улетевшим'")]
    public float yUpperLimit = 10f;

    // Приватные переменные
    private Rigidbody rb;
    private Vector3 currentGravityDirection = Vector3.down;
    private bool isGrounded = false;
    private bool isGravitySwitched = false; // Отслеживаем текущее состояние гравитации

    // --- Константы для Тегов (безопасный способ) ---
    private const string DANGER_TAG = "Danger";
    private const string PORTAL_TAG = "Portal";
    private const string GROUND_TAG = "Ground";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("PlayerController требует компонент Rigidbody!");

        // Убедимся, что стандартная гравитация Unity отключена
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        HandleInput();
        CheckDeathZone();
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
        CheckGrounded();
    }

    /// <summary>
    /// Обрабатывает весь ввод с клавиатуры в Update()
    /// </summary>
    private void HandleInput()
    {
        // 1. Ввод Движения
        moveInput = Input.GetAxis("Horizontal"); // -1 (A) до 1 (D)

        // 2. Ввод Переключения Гравитации (Прыжок)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("GravitySwitch");
            else
                Debug.Log("Объекта AudioManager не существует.");

            SwitchGravity();
        }
    }

    /// <summary>
    /// Проверяет, не вышел ли игрок за пределы мира (не упал ли).
    /// Вызывается из Update().
    /// </summary>
    private void CheckDeathZone()
    {
        // Сравниваем текущую позицию Y игрока с порогом
        if (transform.position.y < yBottomLimit || transform.position.y > yUpperLimit)
        {
            Debug.Log("Игрок вышел за порог. Перезапуск уровня...");

            // Воспроизводим звук смерти (с проверкой на null)
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("PlayerDeath");

            // Вызываем метод перезагрузки
            RestartLevel();
        }
    }

    /// <summary>
    /// Применяет горизонтальное движение в FixedUpdate()
    /// </summary>
    private void HandleMovement()
    {
        // Рассчитываем вектор "вправо" перпендикулярно текущей гравитации
        Vector3 rightVector = Vector3.ProjectOnPlane(Vector3.right, currentGravityDirection).normalized;
        Vector3 moveVelocity = rightVector * moveInput * moveSpeed;

        // Применяем скорость, СОХРАНЯЯ текущую вертикальную (гравитационную) скорость
        // Это исправляет баг "медленного подъема"
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    /// <summary>
    /// Постоянно применяет нашу кастомную силу гравитации
    /// </summary>
    private void ApplyGravity()
    {
        rb.AddForce(currentGravityDirection * gravityForce, ForceMode.Acceleration);
    }

    /// <summary>
    /// Инвертирует направление гравитации
    /// </summary>
    private void SwitchGravity()
    {
        // Инвертируем направление
        currentGravityDirection *= -1;
        isGravitySwitched = !isGravitySwitched; // Переключаем флаг

        // Плавный поворот модели игрока, чтобы он "прилипал" ногами к потолку
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -currentGravityDirection) * transform.rotation;
        
        // Можно запустить Coroutine для плавного поворота, но пока сделаем мгновенно:
        transform.rotation = targetRotation;

        Debug.Log("Гравитация переключена. Новое направление: " + currentGravityDirection);
    }

    /// <summary>
    /// Проверяет, стоит ли игрок на земле (или потолке)
    /// </summary>
    private void CheckGrounded()
    {
        // Используем Raycast, чтобы "пощупать" землю под ногами (или над головой)
        float rayLength = 0.6f; // Чуть длиннее половины куба (0.5)
        Vector3 rayDirection = currentGravityDirection;

        isGrounded = Physics.Raycast(transform.position, rayDirection, rayLength);

        // Для отладки: рисуем луч в редакторе
        Debug.DrawRay(transform.position, rayDirection * rayLength, isGrounded ? Color.green : Color.red);
    }

    // ####################################################################
    // НОВАЯ ЛОГИКА: ОБРАБОТКА СТОЛКНОВЕНИЙ
    // ####################################################################

    /// <summary>
    /// Вызывается АВТОМАТИЧЕСКИ, когда Rigidbody игрока во что-то врезается
    /// </summary>
    /// <param name="collision">Информация о столкновении</param>
    private void OnCollisionEnter(Collision collision)
    {
        // 1. Проверяем, не столкнулись ли мы с "Опасностью"
        if (collision.gameObject.CompareTag(DANGER_TAG))
        {
            Debug.Log("Столкновение с ОПАСНОСТЬЮ! Перезапуск уровня...");
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("PlayerDeath");
            else
                Debug.Log("Объекта AudioManager не существует.");

            RestartLevel();
        }
        // 2. Проверяем, не столкнулись ли мы с "Порталом"
        else if (collision.gameObject.CompareTag(PORTAL_TAG))
        {
            Debug.Log("Столкновение с порталом! Запрос на загрузку следующего уровня...");

            // Получаем компонент портала с объекта, с которым столкнулись
            SceneTransitionPortal portal = collision.gameObject.GetComponent<SceneTransitionPortal>();
            if (portal != null)
            {
                // Вызываем метод на самом портале
                portal.LoadNextScene();
            }
            else
                Debug.LogWarning("Объект с тегом 'Portal' не имеет скрипта 'SceneTransitionPortal'!");
        }
    }

    /// <summary>
    /// Перезагружает текущую активную сцену
    /// </summary>
    private void RestartLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}