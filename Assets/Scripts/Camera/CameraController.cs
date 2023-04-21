using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; //要移动到的目标对象
    public float moveSpeed = 1.0f; //移动速度
    public float rotateSpeed = 1.0f; //旋转速度

    public void RotateCamera()
    {
        StartCoroutine(MoveToTargetCoroutine());
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        while (true)
        {
            //计算位置差和旋转差
            Vector3 positionDiff = target.position - transform.position;
            Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);

            //逐帧改变位置和旋转
            transform.position += positionDiff.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);

            //如果位置和旋转接近目标，退出协程
            if (positionDiff.magnitude < 0.01f && Quaternion.Angle(transform.rotation, target.rotation) < 1.0f)
            {
                yield break;
            }

            yield return null;
        }
    }
}
