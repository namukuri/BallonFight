using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal"; //キー入力用の文字列指定（InputManagerのHorizontalの入力を判定するための文字列）

    private string jump = "Jump"; //キー入力用の文字列指定

    private Rigidbody2D rb; //コンポーネントの取得用

    private Animator anim; //Animetorを取得

    private float scale;  //向きの設定に利用する

    private float limitPosX = 9.0f; //横方向の制限値
    private float limitPosY = 5.5f; //縦方向の制限値

    private bool isGameOver = false;  // GameOver状態の判定用。true ならゲームオーバー。

    public bool isFirstGenerateBallon;  // 初めてバルーンを生成したかを判定するための変数(後程外部スクリプトでも利用するためpublicで宣言する)

    public float moveSpeed;　//移動速度

    public float jumpPower; //ジャンプ・浮遊力

    [SerializeField, Header("Linecast用　地面判定レイヤー")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;

    public bool isGrounded;

    //public GameObject[] ballons;  // GameObject型の配列。インスペクターからヒエラルキーにある Ballon ゲームオブジェクトを２つアサインする
    public List<Ballon> ballonList = new List<Ballon>(); // Ballon 型のリストを宣言して初期化する

    public int maxBallonCount; //バルーンを生成する最大数

    public Transform[] ballonTrans; //バルーンの生成位置の配列

    //public GameObject ballonPrefab; //バルーンのプレハブ
    public Ballon ballonPrefab; // バルーンのプレファブ。GameObject 型ではなく、Ballon 型で宣言する

    public float generateTime; //バルーンを生成する時間

    public bool isGenerating; //バルーンを生成中かどうかを判定する。faleなら生成していない状態。trueは生成中の状態

    public float knockbackPower; // 敵と接触した際に吹き飛ばされる力

    public int coinPoint; // コインを獲得すると増えるポイントの総数

    public UIManager uiManager;

    [SerializeField]
    private AudioClip knockbackSE; // 敵と接触した際に鳴らすSE用のオーディオファイルをアサインする

    [SerializeField]
    private GameObject knockbackEffectPrefab; // 敵と接触した際に生成するエフェクト用のプレファブのゲームオブジェクトをアサインする

    [SerializeField]
    private AudioClip coinSE;

    [SerializeField]
    private GameObject coinEffectPrefab;




    // Start is called before the first frame update
    void Start()
    {
        //Walk();
        //必要なコンポーネントを取得して用意した変数に代入
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        scale = transform.localScale.x; //追加分

        //配列の初期化（バルーンの最大生成数だけ配列の要素数を用意する）
        //ballons = new GameObject[maxBallonCount];

        //Walk(); 
    }

    void Update()
    {
        //地面接地　Phsixs2D.Linecastメソッドを実行して、Ground Layerとキャラのコライダーとが接地している距離かどうかを確認し、接地しているならtrue、接地していないならfalseを戻す
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        //SceneビューにPhysics2D.LinecastメソッドのLineを表示する
        //Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);

        // ballonList 変数の最大値が 1 以上なら = バルーンが１つ以上あるなら
        if (ballonList.Count > 0)
        {

            //Ballons配列変数の最大要素数が 0 以上なら = インスペクターでBallons変数に情報が登録されているなら
            //if (ballons.Length > 0)
            //{

            //ジャンプ
            if (Input.GetButtonDown(jump)) //InputManagerのJumpの項目に登録されているキー入力を判定する
            {
                Jump();
                //Walk();

            }

            // 空中にいる間にRボタンを押すと
            if (Input.GetKeyDown(KeyCode.R) && isGrounded == false)
            {
                // すべてのバルーンを切り離す(地面や床にいる間は不可)
                DetachBallons();
            }

            //接地していない（空中にいる）間で、落下中の場合
            if (isGrounded == false && rb.velocity.y < 0.15f)
            {
                //落下アニメを繰り返す
                anim.SetTrigger("Fall");
            }
        }
        else
        {
            Debug.Log("バルーンがない。ジャンプ不可");
        }

        //Velocity.yの値が5.0fを超える場合(ジャンプを連続で押した場合）
        if (rb.velocity.y > 5.0f)
        {
            //Velocity.yの値に制限をかける（落下せずに上空で待機できてしまう現象を防ぐため）
            rb.velocity = new Vector2(rb.velocity.x, 5.0f);
        }

        // 地面に接地していて、バルーンが生成中ではなく、かつ、バルーンが２個以下の場合
        if (isGrounded == true && isGenerating == false && ballonList.Count < maxBallonCount)
        {
            //Qボタンを押したら
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //バルーンを一つ作成する
                StartCoroutine(GenerateBallon(1, generateTime)); //引数を追加
            }
        }
    }

    private void Walk()
    {
        Debug.Log("Walk");
        Debug.Log("scale/ " + scale);

    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpPower); //キャラの位置を上方向に移動させる（ジャンプ・浮遊)
        anim.SetTrigger("Jump"); //jump(UP+Mid)アニメーションを再生する
    }
    void FixedUpdate()  //一定間隔で呼び出されるメソッド
    {
        if (isGameOver == true) //ここでゲームオーバーの際の移動を制限する
        {
            return; //「ここで処理を終わらせる」という命令。
        }
        //移動
        Move();
    }
    private void Move()
    {
        //水平（横）方向への入力受付
        float x = Input.GetAxis(horizontal); //InputManagerのHorizontalに登録されているキーの入力があるかどうか確認を行う

        //xの値が０でない場合＝キー入力がある場合
        if (x != 0)
        {
            //velocity（速度）に新しい値を代入して移動
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

            //temp変数にlocalScale値を代入
            Vector3 temp = transform.localScale;

            //現在のキー入力値xをtemp.xに代入
            temp.x = x;

            //向きが変わる時に小数になるとキャラが縮んで見えるので整数値にする
            if (temp.x > 0)
            {
                //数字が０よりも大きければすべて１にする
                temp.x = scale;
            }
            else
            {
                //数字が０より小さければすべて‐１にする
                temp.x = -scale;
            }

            //キャラの向きを移動方向に合わせる
            transform.localScale = temp;

            //待機状態のアニメ再生を止めて、走るアニメの再生への遷移を行う
            anim.SetBool("Idle", false);
            anim.SetFloat("Run", 0.5f); //Runアニメーションに対して、０．５ｆの値を情報として渡す。繊維条件がgreater０．１なので、０．１以上の値を渡すと条件が成立してＲｕｎアニメーションが再生される
        }
        else
        {
            //左右の入力がなかったら横移動の速度を０にしてすぐ停止させる
            rb.velocity = new Vector2(0, rb.velocity.y);
            //走るアニメ再生を止めて、待機状態のアニメの再生への遷移を行う
            anim.SetFloat("Run", 0.0f);
            anim.SetBool("Idle", true);
        }

        //現在の位置情報が移動範囲の制限範囲を超えていないか確認する。超えていたら制限範囲内に収める
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        //現在の位置情報が移動範囲の制限範囲を超えていないか確認する。超えていたら、制限範囲に収める
        transform.position = new Vector2(posX, posY);
    }

    //バルーン作成

    private IEnumerator GenerateBallon(int ballonCount, float waitTime)
    {
        //全ての配列の要素にバルーンが存在している場合には、バルーンを生成しない
        if (ballonList.Count >= maxBallonCount)
        {
            yield break;
        }

        //生成中状態にする
        isGenerating = true;

        // isFirstGenerateBallon 変数の値が false、つまり、ゲームを開始してから、まだバルーンを１回も生成していないなら
        if (isFirstGenerateBallon == false)
        {
            // 初回バルーン生成を行ったと判断し、true に変更する = 次回以降はバルーンを生成しても、if 文の条件を満たさなくなり、この処理には入らない
            isFirstGenerateBallon = true;

            Debug.Log("初回のバルーン生成");

            // startChecker 変数に代入されている StartChecker スクリプトにアクセスして、SetInitialSpeed メソッドを実行する
            startChecker.SetInitialSpeed();
        }


        //１つめの配列の要素が空なら
        //if (ballons[0] == null)
        //{
        //１つめのバルーンを生成して、１番目の配列へ代入
        //ballons[0] = Instantiate(ballonPrefab, ballonTrans[0]);

        //ballons[0].GetComponent<Ballon>().SetUpBallon(this);
        //}
        //else
        //{
        //２つ目のバルーンを生成して、２番目の配列へ代入
        //ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);

        //ballons[1].GetComponent<Ballon>().SetUpBallon(this);
        //}
        // バルーンの数を増やす
        //ballonCount++;

        //生成時間分待機
        //yield return new WaitForSeconds(generateTime);

        // ここから List 用の処理を追加

        // 引数で指定された数だけバルーンを生成
        for (int i = 0; i < ballonCount; i++)
        {
            // 生成されたバルーンのクローン(Ballon 型)を代入するための変数を宣言
            Ballon ballon;

            // 1つ目のバルーンの生成位置にバルーンがない場合
            if (ballonTrans[0].childCount == 0)
            {
                // 1つ目のバルーンの生成位置にバルーン生成
                ballon = Instantiate(ballonPrefab, ballonTrans[0]);
            }
            // 1つ目のバルーンの生成位置にバルーンがある場合
            else
            {
                // 2つ目のバルーンの生成位置にバルーン生成
                ballon = Instantiate(ballonPrefab, ballonTrans[1]);
            }

            // バルーンの設定(Ballon 型で生成して変数に代入しているので、GetComponent メソッドを行うことなく、すぐにメソッドの呼び出し命令を実行できる)
            ballon.SetUpBallon(this);

            // List に追加
            ballonList.Add(ballon);

            // バルーン生成時間分だけ待機
            yield return new WaitForSeconds(waitTime);
        }


        //生成中状態終了。再度生成できるようにする
        isGenerating = false;
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        // 接触したコライダーを持つゲームオブジェクトのTagがEnemyなら
        if (col.gameObject.tag == "Enemy")
        {
            // キャラと敵の位置から距離と方向を計算
            Vector3 direction = (transform.position - col.transform.position).normalized;
            // キャラと敵の位置から距離と方向を計算
            transform.position += direction * knockbackPower;

            // 敵との接触用のSE(AudioClip)を再生する
            AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            // 接触した際のエフェクトを、敵の位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
            GameObject knockbackEffect = Instantiate(knockbackEffectPrefab, col.transform.position, Quaternion.identity);

            // エフェクトを 0.5 秒後に破棄。生成したタイミングで変数に代入しているので、削除の命令が出せる
            Destroy(knockbackEffect, 0.5f);
        }
    }




    /// バルーン破壊

    public void DestroyBallon(Ballon ballon)
    {
        // List から削除
        ballonList.Remove(ballon);
        Destroy(ballon.gameObject);
        // TODO 後程、バルーンが破壊される際に「割れた」ように見えるアニメ演出を追加する

        //if (ballons[1] != null)
        //{
        //  Destroy(ballons[1]);
        //}
        //else if (ballons[0] != null)
        //{
        //  Destroy(ballons[0]);
        //}
        // バルーンの数を減らす
        //ballonCount--;

    }

    // IsTriggerがオンのコライダーを持つゲームオブジェクトを通過した場合に呼び出されるメソッド
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            // 通過したコインのゲームオブジェクトの持つ Coin スクリプトを取得し、point 変数の値をキャラの持つ coinPoint 変数に加算
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            uiManager.UpdateDisplayScore(coinPoint);

            //coinとの接触用のSEを再生する
            AudioSource.PlayClipAtPoint(coinSE, transform.position);

            // 接触した際のエフェクトを、coinの位置に、クローンとして生成する。生成されたゲームオブジェクトを変数へ代入
            GameObject coinEffect = Instantiate(coinEffectPrefab, col.transform.position, Quaternion.identity);

            // エフェクトを 0.5 秒後に破棄。生成したタイミングで変数に代入しているので、削除の命令が出せる
            Destroy(coinEffect, 0.5f);

            // 通過したコインのゲームオブジェクトを破壊する
            Destroy(col.gameObject);
        }

    }

    // ゲームオーバー
    public void GameOver() //GameOverZoneのGameOverを参照する
    {
        isGameOver = true;

        // Console ビューに isGameOver 変数の値を表示する。ここが実行されると true と表示される
        Debug.Log(isGameOver);

        // 画面にゲームオーバー表示を行う
        uiManager.DisplayGameOverInfo();
    }

    //すべてのバルーンを切り離す
    private void DetachBallons()
    {

        // 現在のバルーンを１つずつ順番に処理する
        for (int i = 0; i < ballonList.Count; i++)
        {
            // バルーンを切り離し、上空へ浮遊させる
            ballonList[i].FloatingBallon();
        }

        // バルーンのリストをクリアし、再度、バルーンを生成できるようにする
        ballonList.Clear();
    }
}