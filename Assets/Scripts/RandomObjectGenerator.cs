using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objPrefab;

    [SerializeField]
    private Transform generateTran;

    [Header("�����܂ł̑ҋ@����")]
    public Vector2 waitTimeRange; // �P�񐶐�����܂ł̑ҋ@���ԁB�ǂ̈ʂ̊Ԋu�Ŏ����������s�����ݒ�

    private float waitTime;

    private float timer; // �ҋ@���Ԃ̌v���p

    // Start is called before the first frame update
    void Start()
    {
        SetGenerateTime();

    }

    // �����܂ł̎��Ԃ�ݒ�
    private void SetGenerateTime()
    {
        // �����܂ł̑ҋ@���Ԃ��A�ŏ��l�ƍő�l�̊Ԃ��烉���_���Őݒ�
        waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
    }


    // Update is called once per frame
    void Update()
    {
        // �v���p�^�C�}�[�����Z
        timer += Time.deltaTime;

        // �v���p�^�C�}�[���ҋ@���ԂƓ�������������
        if (timer >= waitTime)
        {
            // �^�C�}�[�����Z�b�g���āA�ēx�v���ł����Ԃɂ���
            timer = 0;

            // �����_���ȃI�u�W�F�N�g�𐶐�
            RandomGenerateObject();
        }
    }

    // �����_���ȃI�u�W�F�N�g�𐶐�
    private void RandomGenerateObject()
    {
        // ��������v���t�@�u�̔ԍ��������_���ɐݒ�
        int randomIndex = Random.Range(0, objPrefab.Length);

        // �v���t�@�u�����ɃN���[���̃Q�[���I�u�W�F�N�g�𐶐�
        GameObject obj = Instantiate(objPrefab[randomIndex], generateTran);

        // �����_���Ȓl���擾
        float randomPosY = Random.Range(-4.0f, 4.0f);

        // �������ꂽ�Q�[���I�u�W�F�N�g��Y���Ƀ����_���Ȓl�����Z���āA��������邽�тɍ����̈ʒu��ύX����
        obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + randomPosY);

        // ���̐����܂ł̎��Ԃ��Z�b�g����
        SetGenerateTime();
    }
}
