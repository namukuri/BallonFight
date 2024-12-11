using DG.Tweening; //  <=  ☆　DOTween を利用するために宣言を追加します
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text TxtScore; // txtScore ゲームオブジェクトの持つ Text コンポーネントをインスペクターからアサインする

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;

    // スコア表示を更新
    public void UpdateDisplayScore(int score) //　<=　この引数でスコアの情報を受け取る
    {
        TxtScore.text = score.ToString();

    }

    // ゲームオーバー表示
    public void DisplayGameOverInfo()
    {
        // InfoBackGround ゲームオブジェクトの持つ CanvasGroup コンポーネントの Alpha の値を、1秒かけて 1 に変更して、背景と文字が画面に見えるようにする
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        // 文字列をアニメーションさせて表示
        txtInfo.DOText("Game Over...", 1.0f);
    }
}
