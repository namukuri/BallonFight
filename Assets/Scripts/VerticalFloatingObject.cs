using DG.Tweening;
using UnityEngine;

public class VerticalFloatingObject : MonoBehaviour
{
    public float moveTime;
    public float moveRange;

    Tweener tweener;   // DOTween の処理の代入用

    // Start is called before the first frame update
    void Start()
    {
        // DOTween による命令を実行し、それを Tweener 型の tweener 変数に代入
        tweener = transform.DOMoveY(transform.position.y - moveRange, moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {

        // ゲームオブジェクトの破棄に合わせて DOTween の処理を破棄する(その結果、無限ループ処理を解消する)
        // DOTween を実行するタイミングで変数に代入しておくことで、あとで DOTween に命令を出すことができる
        tweener.Kill();
    }

    
}
