using UnityEngine;
using DG.Tweening;

public class FloatingFloor : MonoBehaviour
{
    private Vector2 pos;

    private float randomValue;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        randomValue = Random.Range(0.01f, 0.3f);

        // �����_���ȍ����ɂӂ�ӂ�㉺�Ɉړ�������
        transform.DOMoveY(pos.y + randomValue, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
