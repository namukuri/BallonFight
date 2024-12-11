using DG.Tweening; //  <=  ���@DOTween �𗘗p���邽�߂ɐ錾��ǉ����܂�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text TxtScore; // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;

    // �X�R�A�\�����X�V
    public void UpdateDisplayScore(int score) //�@<=�@���̈����ŃX�R�A�̏����󂯎��
    {
        TxtScore.text = score.ToString();

    }

    // �Q�[���I�[�o�[�\��
    public void DisplayGameOverInfo()
    {
        // InfoBackGround �Q�[���I�u�W�F�N�g�̎��� CanvasGroup �R���|�[�l���g�� Alpha �̒l���A1�b������ 1 �ɕύX���āA�w�i�ƕ�������ʂɌ�����悤�ɂ���
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        // ��������A�j���[�V���������ĕ\��
        txtInfo.DOText("Game Over...", 1.0f);
    }
}
