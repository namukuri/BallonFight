using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObj;

    [SerializeField]
    private GameDirector gameDirector;

    [SerializeField]
    private UIManager uiManager;

    IEnumerator Start() // <=  ☆　戻り値が違うので注意する
    {

        // 引数で指定している条件を満たすまで、ここで処理を一時中断して待機する
        yield return new WaitUntil(() => uiManager.isTitleClicked == true);

        // TitleObject を移動
        MoveTitleObject();
    }

    // TitleObject を移動
    private void MoveTitleObject()
    {
        // ゲームスタートに合わせてゴールを画面の右端側に移動させて、画面から見えなくなってから非表示にする
        titleObj.transform.DOMoveX(15, 2.0f).SetEase(Ease.Linear).OnComplete(() => { titleObj.SetActive(false); });
    }
    
}
