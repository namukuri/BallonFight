using DG.Tweening; //  <=  ���@DOTween �𗘗p���邽�߂ɐ錾��ǉ����܂�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Button btnInfo;

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

        btnInfo.onClick.AddListener(RestartGame);
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

    // �^�C�g���֖߂�
    public void RestartGame()
    {

        // �{�^�����烁�\�b�h���폜(�d���N���b�N�h�~)
        btnInfo.onClick.RemoveAllListeners();

        // ���݂̃V�[���̖��O���擾
        string sceneName = SceneManager.GetActiveScene().name;

        canvasGroupInfo.DOFade(0f, 1.0f) //1�b�Ԃ�����Canvas Group�̓����x���u���݂̒l�v����u0�v�ɕω�
            .OnComplete(() => //OnComplete �́A�A�j���[�V�������I�������Ƃ��ɌĂяo�����R�[���o�b�N�i������ǉ����邽�߂̎d�g�݁j
            {             //{ ... } �̒��ɋL�q���ꂽ���������s����郉���_��
                Debug.Log("Restart");
                SceneManager.LoadScene(sceneName);
            });
            
    }
}
