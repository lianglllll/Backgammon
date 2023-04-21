using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; //Ҫ�ƶ�����Ŀ�����
    public float moveSpeed = 1.0f; //�ƶ��ٶ�
    public float rotateSpeed = 1.0f; //��ת�ٶ�

    public void RotateCamera()
    {
        StartCoroutine(MoveToTargetCoroutine());
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        while (true)
        {
            //����λ�ò����ת��
            Vector3 positionDiff = target.position - transform.position;
            Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);

            //��֡�ı�λ�ú���ת
            transform.position += positionDiff.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);

            //���λ�ú���ת�ӽ�Ŀ�꣬�˳�Э��
            if (positionDiff.magnitude < 0.01f && Quaternion.Angle(transform.rotation, target.rotation) < 1.0f)
            {
                yield break;
            }

            yield return null;
        }
    }
}
