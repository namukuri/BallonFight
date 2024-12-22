using DG.Tweening; //  <=  ☆　DOTween を利用するために宣言を追加します
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text TxtScore; // txtScore ゲームオブジェクトの持つ Text コンポーネントをインスペクターからアサインする

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

        btnInfo.onClick.AddListener(RestartGame);
    }

    /// <summary>
    /// ResultPopUpの生成
    /// </summary>
    public void GenerateResultPobUp(int score)
    {
        // ResultPopUp を生成
        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);
        //Instantiateの3番目の引数falseは、複製時にワールド座標を使うかどうかを指定します。
        //falseの場合：オブジェクトは親オブジェクト（canvasTran）のローカル座標を使用します。
        //trueの場合：オブジェクトはワールド座標をそのまま使用します。

        // ResultPopUp の設定を行う
        resultPopUp.SetUpResultPopUp(score);
    }

    // タイトルへ戻る
    public void RestartGame()
    {

        // ボタンからメソッドを削除(重複クリック防止)
        btnInfo.onClick.RemoveAllListeners();

        // 現在のシーンの名前を取得
        string sceneName = SceneManager.GetActiveScene().name;

        canvasGroupInfo.DOFade(0f, 1.0f) //1秒間かけてCanvas Groupの透明度が「現在の値」から「0」に変化
            .OnComplete(() => //OnComplete は、アニメーションが終了したときに呼び出されるコールバック（処理を追加するための仕組み）
            {             //{ ... } の中に記述された処理が実行されるラムダ式
                Debug.Log("Restart");
                SceneManager.LoadScene(sceneName);
            });
            
    }
}
