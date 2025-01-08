using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ballon : MonoBehaviour
{
    private PlayerController playerController;

    private Tweener tweener;

    private bool isDetached;  // バルーンがキャラから切り離されて浮遊しているか。true なら切り離されている。false は切り離されていない状態。
    private Rigidbody2D rb; // Rigidbody2D コンポーネントの代入用
    private Vector2 pos; // バルーンの位置情報の管理用

    // バルーンの初期設定
    public void SetUpBallon(PlayerController playerController)
    {
        this.playerController = playerController;

        // 本来のScaleを保持
        Vector3 scale = transform.localScale;

        // 現在のScaleを0にして画面から一時的に非表示にする
        transform.localScale = Vector3.zero;

        // だんだんバルーンが膨らむアニメ演出
        transform.DOScale(scale, 2.0f).SetEase(Ease.InBounce);

        // 左右にふわふわさせる
        tweener = transform.DOLocalMoveX(0.02f, 0.2f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {            // 左右にふわふわさせるループアニメを破棄する
            tweener?.Kill();

            // PlayerControllerのDestroyBallonメソッドを呼び出し、バルーンの破壊処理を行う
            playerController.DestroyBallon(this);
        }

    }

    // バルーンを上空へ飛ばす準備。PlayerController より呼び出される
    public void FloatingBallon()
    {
        // 左右にふわふわするループアニメを破棄する =>  破棄しない場合どうなるのか、コメントアウトして試してみましょう
        tweener.Kill();

        // Rigidbody2Dコンポーネントをバルーンに追加して代入
        rb = gameObject.AddComponent<Rigidbody2D>();

        // 重力は0にする
        rb.gravityScale = 0;

        // 回転も固定する
        rb.freezeRotation = true;

        // バルーンのコライダーを取得して、スイッチをオフにする
        GetComponent<CapsuleCollider2D>().enabled = false;

        // バルーンの位置情報を代入
        pos = transform.position;

        // 親子関係を解消する(特にPlayerが地面・床の子オブジェクトになっている場合に解消しておかないと不具合になる。試してみましょう)
        transform.SetParent(null);

        // バルーンとプレイヤーを切り離す状態にする
        isDetached = true;
    }

    private void FixedUpdate()
    {

        // バルーンが切り離されていなければ、処理をしない
        if (isDetached == false)
        {
            return;
        }

        //* バルーンが切り離されている場合、以下の処理を行う *//

        // バルーンの位置を上方向へ移動させる
        pos.y += 0.05f;

        // バルーンを左右に揺らす
        rb.MovePosition(new Vector2(pos.x + Mathf.PingPong(Time.time, 1.5f), pos.y));

        // 画面外にバルーンが出たら
        if (transform.position.y > 5.0f)
        {

            // 破壊する
            Destroy(gameObject);
        }
    }




}
