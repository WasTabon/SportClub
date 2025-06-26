using UnityEngine;
using DG.Tweening;

public class RotateUI : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 2f;

    private void Start()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.DORotate(new Vector3(0, 0, -360), rotationDuration, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1);
    }
}
