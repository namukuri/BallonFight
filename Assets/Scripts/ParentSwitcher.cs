using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParentSwitcher : MonoBehaviour
{
    private string player = "Player"; //Tag �ɐݒ肵�Ă��镶�������

    //���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�̃R���C�_�[�ƁA���̃Q�[���I�u�W�F�N�g�̃R���C�_�[���ڐG���Ă���Ԃ����ƐڐG������s�����\�b�h
    private void OnCollisionStay2D(Collision2D col)
    {
        //�ڐG���肪��������Ɓ@col�ϐ��ɃR���C�_�[�̏�񂪑�������B���̃R���C�_�[�����Q�[���I�u�W�F�N�g��Tag��player�ϐ��̒l�i"Player")�Ƃ��Ȃ�������Ȃ�
        if (col.gameObject.tag == player)
        {
            //�ڐG���Ă���Q�[���I�u�W�F�N�g�i�L�����j���A���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�i���j�̎q�I�u�W�F�N�g�ɂ���
            col.transform.SetParent(transform);
        }

    }


//���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�̃R���C�_�[�ƁA���̃Q�[���I�u�W�F�N�g�̃R���C�_�[�����ꂽ�ۂɔ�����s�����\�b�h
    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == player && gameObject.activeInHierarchy) //�V�����ǉ�)
        {

        //�ڐG��ԂŖS���Ȃ����i���ꂽ�j�Q�[���I�u�W�F�N�g�i�L�����j�ƁA���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g�i���j�̐e�q�֌W����������
            col.transform.SetParent(null);
        }
     }
}