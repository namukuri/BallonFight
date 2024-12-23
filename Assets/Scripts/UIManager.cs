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

    [SerializeField]
    private Button btnTitle;

    [SerializeField] 
    private Text lblStart;

    [SerializeField]
    private CanvasGroup canvasGroupTitle;

    private Tweener tweener;



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
    public void GenerateResultPopUp(int score)
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

    private void Start()
    {
        // �^�C�g���\��
        SwitchDisplayTitle(true, 1.0f);

        // �{�^����OnClick�C�x���g�Ƀ��\�b�h��o�^
        btnTitle.onClick.AddListener(OnClickTitle);

    }

    // �^�C�g���\��
    public void SwitchDisplayTitle(bool isSwich, float alpha)
    {
        if (isSwich) canvasGroupTitle.alpha = 0;

        canvasGroupTitle.DOFade(alpha, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            lblStart.gameObject.SetActive(isSwich);
        });

        if (tweener == null)
        {
            // Tap Start�̕������������_�ł�����
            tweener = lblStart.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        }
        else
        {
            tweener.Kill();
        }
    }

    // �^�C�g���\�����ɉ�ʂ��N���b�N�����ۂ̏���
    private void OnClickTitle() 
    {
        // �{�^���̃��\�b�h���폜���ďd���^�b�v�h�~
        btnTitle.onClick.RemoveAllListeners();

        // �^�C�g�������X�ɔ�\��
        SwitchDisplayTitle(false, 0.0f);

        // �^�C�g���\����������̂Ɠ���ւ��ŁA�Q�[���X�^�[�g�̕�����\������
        StartCoroutine(DisplayGameStartInfo());

    }

    // �Q�[���X�^�[�g�\��
    public IEnumerator DisplayGameStartInfo()
    {
        yield return new WaitForSeconds(0.5f); //0.5�b�҂�

        canvasGroupInfo.alpha = 0;
        canvasGroupInfo.DOFade(1.0f, 0.5f);�@//0.5�b�����ĕ\��
        txtInfo.text = "Game Start!";

        yield return new WaitForSeconds(1.0f);�@//1�b�҂�
        canvasGroupInfo.DOFade(0f, 0.5f);�@//0.5�b�����Ĕ�\��

        canvasGroupTitle.gameObject.SetActive(false);�@//�A�N�e�B�u��Ԃ�false�ɂ���

    }

}
