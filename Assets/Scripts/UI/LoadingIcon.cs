using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LoadingIcon : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
    public void StopRotation()
    {
        this.gameObject.GetComponent<RectTransform>().DOKill();
    }
}
