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

    [SerializeField]
    private ResultPopUp resultPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;

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

    /// <summary>
    /// ResultPopUp�̐���
    /// </summary>
    public void GenerateResultPobUp(int score)
    {
        // ResultPopUp �𐶐�
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);
        //Instantiate��3�Ԗڂ̈���false�́A�������Ƀ��[���h���W���g�����ǂ������w�肵�܂��B
        //false�̏ꍇ�F�I�u�W�F�N�g�͐e�I�u�W�F�N�g�icanvasTran�j�̃��[�J�����W���g�p���܂��B
        //true�̏ꍇ�F�I�u�W�F�N�g�̓��[���h���W�����̂܂܎g�p���܂��B

        // ResultPopUp �̐ݒ���s��
        resultPopUp.SetUpResultPopUp(score);
    }
}
