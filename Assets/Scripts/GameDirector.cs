using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private GoalChecker goalHousePrefab; // �S�[���n�_�̃v���t�@�u���A�T�C��

    [SerializeField]
    private PlayerController playerController; // �q�G�����L�[�ɂ��� Yuko_Player �Q�[���I�u�W�F�N�g���A�T�C��

    [SerializeField]
    private FloorGenerator[] floorGenerators; // floorGenerator �X�N���v�g�̃A�^�b�`����Ă���Q�[���I�u�W�F�N�g���A�T�C��

    private bool isSetUp;  // �Q�[���̏�������p�Btrue �ɂȂ�ƃQ�[���J�n

    private bool isGameUp;

    private int generateCount; // �󒆏��̐�����

    // generateCount �ϐ��̃v���p�e�B
    public int GenerateCount
    {
        set
        {
            generateCount = value;

            Debug.Log("������/�N���A�ڕW�� : " + generateCount + "/" + clearCount);

            if (generateCount >= clearCount)
            {
                // �S�[���n�_�𐶐�
                GenerateGoal();

                // �Q�[���I��
                GameUp();
            }
        }
        get
        {
            return generateCount;
        }

    }

    public int clearCount;

    void Start()
    {
        isGameUp = false;
        isSetUp = false;

        // FloorGenerator�̏���
        SetUpFloorGenerators();

        // TODO �e�W�F�l���[�^���~
        Debug.Log("������~");
    }

    // FloorGenerator�̏���
    private void SetUpFloorGenerators()
    {
        for (int i = 0; i < floorGenerators.Length; i++)
        {
            // FloorGenerator�̏����E�����ݒ���s��
            floorGenerators[i].SetUpGenerator(this);
        }
    }
    void Update()
    {
        // �v���C���[���͂��߂ăo���[���𐶐�������
        if (playerController.isFirstGenerateBallon && isSetUp == false)
        {

            // ��������
            isSetUp = true;

            // TODO �e�W�F�l���[�^�𓮂����n�߂�
            Debug.Log("�����X�^�[�g");
        }
    }

    // �S�[���n�_�̐���
    private void GenerateGoal()
    {
        // �S�[���n�_�𐶐�
        GoalChecker goalHouse = Instantiate(goalHousePrefab);

        // TODO �S�[���n�_�̏����ݒ�
        Debug.Log("�S�[���n�_����");
    }

    // �Q�[���I��
    public void GameUp()
    {

        // �Q�[���I��
        isGameUp = true;

        // TODO �e�W�F�l���[�^���~
        Debug.Log("������~");
    }
}