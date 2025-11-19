using UnityEngine;

/// <summary>
/// Управляет камерой.
/// Плавно следует за игроком, сохраняя постоянное расстояние и угол,
/// и игнорирует вращение игрока.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Объект, за которым следит камера (наш Игрок)")]
    public Transform target;

    [Tooltip("Насколько плавно камера догоняет игрока (меньше = резче, больше = плавнее)")]
    public float smoothSpeed = 0.125f;

    // Приватное поле для хранения смещения камеры относительно игрока
    private Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraController: Цель (target) не назначена!");
            return;
        }

        // Вычисляем и запоминаем изначальное смещение камеры от игрока
        // (то, как выставили камеру в редакторе)
        offset = transform.position - target.position;
    }

    /// <summary>
    /// LateUpdate() вызывается ПОСЛЕ всех Update() и FixedUpdate().
    /// Это идеальное место для обновления камеры, т.к. игрок уже
    /// завершил свое движение в этом кадре.
    /// </summary>
    void LateUpdate()
    {
        if (target == null)
            return;

        // 1. Рассчитываем новую желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;

        // 2. Плавно интерполируем от текущей позиции к желаемой
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 3. Применяем новую позицию
        transform.position = smoothedPosition;

        // 4. (Опционально) Убеждаемся, что камера всегда смотрит на игрока
        // transform.LookAt(target); // <-- Раскомментировать, если нужно, чтобы камера всегда "смотрела" на центр куба
    }
}