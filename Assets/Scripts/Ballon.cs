using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ballon : MonoBehaviour
{
    private PlayerController playerController;

    private Tweener tweener;

    private bool isDetached;  // �o���[�����L��������؂藣����ĕ��V���Ă��邩�Btrue �Ȃ�؂藣����Ă���Bfalse �͐؂藣����Ă��Ȃ���ԁB
    private Rigidbody2D rb; // Rigidbody2D �R���|�[�l���g�̑���p
    private Vector2 pos; // �o���[���̈ʒu���̊Ǘ��p

    // �o���[���̏����ݒ�
    public void SetUpBallon(PlayerController playerController)
    {
        this.playerController = playerController;

        // �{����Scale��ێ�
        Vector3 scale = transform.localScale;

        // ���݂�Scale��0�ɂ��ĉ�ʂ���ꎞ�I�ɔ�\���ɂ���
        transform.localScale = Vector3.zero;

        // ���񂾂�o���[�����c��ރA�j�����o
        transform.DOScale(scale, 2.0f).SetEase(Ease.InBounce);

        // ���E�ɂӂ�ӂ킳����
        tweener = transform.DOLocalMoveX(0.02f, 0.2f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {            // ���E�ɂӂ�ӂ킳���郋�[�v�A�j����j������
            tweener?.Kill();

            // PlayerController��DestroyBallon���\�b�h���Ăяo���A�o���[���̔j�󏈗����s��
            playerController.DestroyBallon(this);
        }

    }

    // �o���[�������֔�΂������BPlayerController ���Ăяo�����
    public void FloatingBallon()
    {
        // ���E�ɂӂ�ӂ킷�郋�[�v�A�j����j������ =>  �j�����Ȃ��ꍇ�ǂ��Ȃ�̂��A�R�����g�A�E�g���Ď����Ă݂܂��傤
        tweener.Kill();

        // Rigidbody2D�R���|�[�l���g���o���[���ɒǉ����đ��
        rb = gameObject.AddComponent<Rigidbody2D>();

        // �d�͂�0�ɂ���
        rb.gravityScale = 0;

        // ��]���Œ肷��
        rb.freezeRotation = true;

        // �o���[���̃R���C�_�[���擾���āA�X�C�b�`���I�t�ɂ���
        GetComponent<CapsuleCollider2D>().enabled = false;

        // �o���[���̈ʒu������
        pos = transform.position;

        // �e�q�֌W����������(����Player���n�ʁE���̎q�I�u�W�F�N�g�ɂȂ��Ă���ꍇ�ɉ������Ă����Ȃ��ƕs��ɂȂ�B�����Ă݂܂��傤)
        transform.SetParent(null);

        // �o���[���ƃv���C���[��؂藣����Ԃɂ���
        isDetached = true;
    }

    private void FixedUpdate()
    {

        // �o���[�����؂藣����Ă��Ȃ���΁A���������Ȃ�
        if (isDetached == false)
        {
            return;
        }

        //* �o���[�����؂藣����Ă���ꍇ�A�ȉ��̏������s�� *//

        // �o���[���̈ʒu��������ֈړ�������
        pos.y += 0.05f;

        // �o���[�������E�ɗh�炷
        rb.MovePosition(new Vector2(pos.x + Mathf.PingPong(Time.time, 1.5f), pos.y));

        // ��ʊO�Ƀo���[�����o����
        if (transform.position.y > 5.0f)
        {

            // �j�󂷂�
            Destroy(gameObject);
        }
    }




}
