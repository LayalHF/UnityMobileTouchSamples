using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
       
    private void Awake()
    {
        instance = this;
    }

    public void MoveCamera(Transform point)
    {
        transform.DOLocalMove(point.localPosition, 1).SetEase(Ease.OutSine);
        transform.DOLocalRotate(point.localEulerAngles, 1).SetEase(Ease.OutSine);
    }
}
