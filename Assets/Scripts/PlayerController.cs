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

    private float limitPosX = 8.7f; //横方向の制限値
    private float limitPosY = 4.0f; //縦方向の制限値

    public bool isFirstGenerateBallon;  // 初めてバルーンを生成したかを判定するための変数(後程外部スクリプトでも利用するためpublicで宣言する)

    public float moveSpeed;　//移動速度

    public float jumpPower; //ジャンプ・浮遊力

    [SerializeField, Header("Linecast用　地面判定レイヤー")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;
    
    public bool isGrounded;

    public GameObject[] ballons;  // GameObject型の配列。インスペクターからヒエラルキーにある Ballon ゲームオブジェクトを２つアサインする

    public int maxBallonCount; //バルーンを生成する最大数

    public Transform[] ballonTrans; //バルーンの生成位置の配列

    public GameObject ballonPrefab; //バルーンのプレハブ

    public float generateTime; //バルーンを生成する時間

    public bool isGenerating; //バルーンを生成中かどうかを判定する。faleなら生成していない状態。trueは生成中の状態

    public float knockbackPower; // 敵と接触した際に吹き飛ばされる力

    public int coinPoint; // コインを獲得すると増えるポイントの総数

    public UIManager uiManager;



    // Start is called before the first frame update
    void Start()
    {
        //Walk();
        //必要なコンポーネントを取得して用意した変数に代入
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        scale = transform.localScale.x; //追加分

        //配列の初期化（バルーンの最大生成数だけ配列の要素数を用意する）
        ballons = new GameObject[maxBallonCount];

        //Walk(); 
    }

    void Update()
    {
        //地面接地　Phsixs2D.Linecastメソッドを実行して、Ground Layerとキャラのコライダーとが接地している距離かどうかを確認し、接地しているならtrue、接地していないならfalseを戻す
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        //SceneビューにPhysics2D.LinecastメソッドのLineを表示する
        //Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);

        //ballons変数の最初の要素の値が空でないなら　＝　バルーンが１つ生成されるとこの要素に値が代入される　＝　バルーンが１つあるなら
        if (ballons[0]  != null ) 
        {

        //Ballons配列変数の最大要素数が 0 以上なら = インスペクターでBallons変数に情報が登録されているなら
        //if (ballons.Length > 0)
        //{

            //ジャンプ
            if (Input.GetButtonDown(jump)) //InputManagerのJumpの項目に登録されているキー入力を判定する
            {
                Jump();
                //Walk();
                //Walk();
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
        if(rb.velocity.y > 5.0f)
        {
            //Velocity.yの値に制限をかける（落下せずに上空で待機できてしまう現象を防ぐため）
            rb.velocity = new Vector2(rb.velocity.x, 5.0f);
        }

        //地面に接地していて、ばるーが生成中ではない場合
        if (isGrounded == true && isGenerating == false)
        {
            //Qボタンを押したら
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //バルーンを一つ作成する
                StartCoroutine(GenerateBallon());
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
    void FixedUpdate()
    {
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

    private IEnumerator GenerateBallon()
    {
        //全ての配列の要素にバルーンが存在している場合には、バルーンを生成しない
        if (ballons[1] != null)
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
            if (ballons[0] == null)
            {
                //１つめのバルーンを生成して、１番目の配列へ代入
                ballons[0] = Instantiate(ballonPrefab, ballonTrans[0]);

                ballons[0].GetComponent<Ballon>().SetUpBallon(this);
            }
            else
            {
                //２つ目のバルーンを生成して、２番目の配列へ代入
                ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);

                ballons[1].GetComponent<Ballon>().SetUpBallon(this);
            }

            //生成時間分待機
            yield return new WaitForSeconds(generateTime);


            //生成中状態終了。再度生成できるようにする
            isGenerating = false;
        }   
          
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        // 接触したコライダーを持つゲームオブジェクトのTagがEnemyなら
        if (col.gameObject.tag == "Enemy")
        {
            Vector3 direction = (transform.position - col.transform.position).normalized;
            // キャラと敵の位置から距離と方向を計算
            transform.position += direction * knockbackPower;
        }
    }




    /// バルーン破壊

    public void DestroyBallon()
    {
        // TODO 後程、バルーンが破壊される際に「割れた」ように見えるアニメ演出を追加する

        if (ballons[1] != null)
        {
            Destroy(ballons[1]);
        }
        else if (ballons[0] != null)
        {
            Destroy(ballons[0]);
        }
    }

    // IsTriggerがオンのコライダーを持つゲームオブジェクトを通過した場合に呼び出されるメソッド
    private void OnTriggerEnter2D(Collider2D col)
    {      
       if(col.gameObject.tag == "Coin")
        {
            // 通過したコインのゲームオブジェクトの持つ Coin スクリプトを取得し、point 変数の値をキャラの持つ coinPoint 変数に加算
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            uiManager.UpdateDisplayScore(coinPoint);

            // 通過したコインのゲームオブジェクトを破壊する
            Destroy(col.gameObject);
        }

    }


}