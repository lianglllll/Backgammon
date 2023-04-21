#if ENABLE_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM_PACKAGE
#define USE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Controls;
#endif

using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    #region ���״̬
    /// <summary>
    /// ���״̬
    /// </summary>
    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }
    #endregion

    CameraState m_TargetCameraState = new CameraState();
    CameraState m_InterpolatingCameraState = new CameraState();

    [Header("Movement Settings �ƶ�����")]
    [Tooltip("Exponential boost factor on translation, controllable by mouse wheel. ƽ�Ƶ�ָ����ǿ���ӣ���ͨ�������ֿ��ơ�")]
    public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target. �����λ�ò�ֵ��Ŀ��λ��99%�����ʱ�䡣"), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Header("Rotation Settings ��ת�趨")]
    [Tooltip("X = Change in mouse position. �ı����λ�á�\nY = Multiplicative factor for camera rotation. �����ת�ĳ������ӡ�")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target. ��ֵ�����ת99%��Ŀ�������ʱ�䡣"), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation. �Ƿ���������Y�ᷴתΪ��ת��")]
    public bool invertY = false;

    void OnEnable()
    {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return direction;
    }

    void Update()
    {
        Vector3 translation = Vector3.zero;

#if ENABLE_LEGACY_INPUT_MANAGER

        // Exit Sample ����Esc���˳���Ϸ
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        // Hide and lock cursor when right mouse button pressed ��������Ҽ�ʱ���ز��������
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Unlock and show cursor when right mouse button released �ɿ�����Ҽ�ʱ��������ʾ���
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Rotation ��ת
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
        }

        // Translation �ƶ�
        translation = GetInputTranslationDirection() * Time.deltaTime;

        // Speed up movement when shift key held ��סshift��ʱ�����ƶ�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //ԭ�ٶ�*10Ϊ����Shift����ٶ�
            translation *= 10.0f;
        }

        // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel) ͨ����ǿ�����޸��ƶ����ڼ�����ж��壬ͨ���������ڲ���ģʽ���޸ģ�
        boost += Input.mouseScrollDelta.y * 0.2f;
        translation *= Mathf.Pow(2.0f, boost);

#elif USE_INPUT_SYSTEM
            // TODO: make the new input system work ʹ�µ�����ϵͳ��������
#endif

        m_TargetCameraState.Translate(translation);

        // Framerate-independent interpolation ֡���޹ز�ֵ
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time ����lerp���������������ǾͿ�����ָ����ʱ���ڵ���Ŀ���99%
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

        m_InterpolatingCameraState.UpdateTransform(transform);
    }
}
