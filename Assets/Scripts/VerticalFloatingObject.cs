using DG.Tweening;
using UnityEngine;

public class VerticalFloatingObject : MonoBehaviour
{
    public float moveTime;
    public float moveRange;

    Tweener tweener;   // DOTween �̏����̑���p

    // Start is called before the first frame update
    void Start()
    {
        // DOTween �ɂ�閽�߂����s���A����� Tweener �^�� tweener �ϐ��ɑ��
        tweener = transform.DOMoveY(transform.position.y - moveRange, moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {

        // �Q�[���I�u�W�F�N�g�̔j���ɍ��킹�� DOTween �̏�����j������(���̌��ʁA�������[�v��������������)
        // DOTween �����s����^�C�~���O�ŕϐ��ɑ�����Ă������ƂŁA���Ƃ� DOTween �ɖ��߂��o�����Ƃ��ł���
        tweener.Kill();
    }

    
}
