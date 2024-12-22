using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CanvasGroup canvasGroupRestart;

    [SerializeField]
    private Button btnTitle;

    /// <summary>
    /// ResultPopUpの設定
    /// </summary>
    /// <param name="score"></param>
    public void SetUpResultPopUp(int score)
    {
        // 最初に透明にする
        canvasGroup.alpha = 0;

        // 徐々にResultPopUpを表示 1.0fが完全表示　１秒かけて表示
        canvasGroup.DOFade(1.0f, 1.0f);

        // スコアを表示
        txtScore.text = score.ToString();

        // リスタートのメッセージをゆっくりと点滅アニメさせる(学習済の命令です。復習しておきましょう)
        canvasGroupRestart.DOFade(0, 1.0f)
            .SetEase(Ease.InOutQuad)　//最初と最後の表示がゆっくりな滑らかな動き
            .SetLoops(-1, LoopType.Yoyo);　//-1は無限ループ　Yoｙoは行ったり来たり

        //ボタンにメソッドを登録
        btnTitle.onClick.AddListener(OnClickRestart);
    }
    
    /// <summary>
    /// ボタンを押した際の制御
    /// </summary>
    private void OnClickRestart()
    {
        // リザルト表示を徐々に非表示にする
        canvasGroup.DOFade(0, 1.0f)　//１秒かけて完全に非表示
            .SetEase(Ease.Linear);　//Linearは一定速度での意味　

        // 現在のシーンを再度読み込む
        StartCoroutine(Restart());
    }

    /// <summary>
    /// 現在のシーンを再度読み込む
    /// </summary>
    /// <returns></returns>
    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(1.0f);

        // 現在のシーンの名前を取得
        string sceneName = SceneManager.GetActiveScene().name;

        // 再度読み込み、ゲームを再スタート
        SceneManager.LoadScene(sceneName);
    }         
    
}
