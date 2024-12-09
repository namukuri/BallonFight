using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text TxtScore; // txtScore �Q�[���I�u�W�F�N�g�̎��� Text �R���|�[�l���g���C���X�y�N�^�[����A�T�C������

    // �X�R�A�\�����X�V
    public void UpdateDisplayScore(int score) //�@<=�@���̈����ŃX�R�A�̏����󂯎��
    {
        TxtScore.text = score.ToString();
        
    }
       
    
}
