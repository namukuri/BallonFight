using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObj;

    [SerializeField]
    private GameDirector gameDirector;

    [SerializeField]
    private UIManager uiManager;

    IEnumerator Start() // <=  ���@�߂�l���Ⴄ�̂Œ��ӂ���
    {

        // �����Ŏw�肵�Ă�������𖞂����܂ŁA�����ŏ������ꎞ���f���đҋ@����
        yield return new WaitUntil(() => uiManager.isTitleClicked == true);

        // TitleObject ���ړ�
        MoveTitleObject();
    }

    // TitleObject ���ړ�
    private void MoveTitleObject()
    {
        // �Q�[���X�^�[�g�ɍ��킹�ăS�[������ʂ̉E�[���Ɉړ������āA��ʂ��猩���Ȃ��Ȃ��Ă����\���ɂ���
        titleObj.transform.DOMoveX(15, 2.0f).SetEase(Ease.Linear).OnComplete(() => { titleObj.SetActive(false); });
    }
    
}
