using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal"; //�L�[���͗p�̕�����w��iInputManager��Horizontal�̓��͂𔻒肷�邽�߂̕�����j

    private string jump = "Jump"; //�L�[���͗p�̕�����w��

    private Rigidbody2D rb; //�R���|�[�l���g�̎擾�p

    private Animator anim; //Animetor���擾

    private float scale;  //�����̐ݒ�ɗ��p����

    private float limitPosX = 8.7f; //�������̐����l
    private float limitPosY = 4.0f; //�c�����̐����l

    public bool isFirstGenerateBallon;  // ���߂ăo���[���𐶐��������𔻒肷�邽�߂̕ϐ�(����O���X�N���v�g�ł����p���邽��public�Ő錾����)

    public float moveSpeed;�@//�ړ����x

    public float jumpPower; //�W�����v�E���V��

    [SerializeField, Header("Linecast�p�@�n�ʔ��背�C���[")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;
    
    public bool isGrounded;

    public GameObject[] ballons;  // GameObject�^�̔z��B�C���X�y�N�^�[����q�G�����L�[�ɂ��� Ballon �Q�[���I�u�W�F�N�g���Q�A�T�C������

    public int maxBallonCount; //�o���[���𐶐�����ő吔

    public Transform[] ballonTrans; //�o���[���̐����ʒu�̔z��

    public GameObject ballonPrefab; //�o���[���̃v���n�u

    public float generateTime; //�o���[���𐶐����鎞��

    public bool isGenerating; //�o���[���𐶐������ǂ����𔻒肷��Bfale�Ȃ琶�����Ă��Ȃ���ԁBtrue�͐������̏��

    public float knockbackPower; // �G�ƐڐG�����ۂɐ�����΂�����

    public int coinPoint; // �R�C�����l������Ƒ�����|�C���g�̑���

    public UIManager uiManager;



    // Start is called before the first frame update
    void Start()
    {
        //Walk();
        //�K�v�ȃR���|�[�l���g���擾���ėp�ӂ����ϐ��ɑ��
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        scale = transform.localScale.x; //�ǉ���

        //�z��̏������i�o���[���̍ő吶���������z��̗v�f����p�ӂ���j
        ballons = new GameObject[maxBallonCount];

        //Walk(); 
    }

    void Update()
    {
        //�n�ʐڒn�@Phsixs2D.Linecast���\�b�h�����s���āAGround Layer�ƃL�����̃R���C�_�[�Ƃ��ڒn���Ă��鋗�����ǂ������m�F���A�ڒn���Ă���Ȃ�true�A�ڒn���Ă��Ȃ��Ȃ�false��߂�
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        //Scene�r���[��Physics2D.Linecast���\�b�h��Line��\������
        //Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);

        //ballons�ϐ��̍ŏ��̗v�f�̒l����łȂ��Ȃ�@���@�o���[�����P���������Ƃ��̗v�f�ɒl����������@���@�o���[�����P����Ȃ�
        if (ballons[0]  != null ) 
        {

        //Ballons�z��ϐ��̍ő�v�f���� 0 �ȏ�Ȃ� = �C���X�y�N�^�[��Ballons�ϐ��ɏ�񂪓o�^����Ă���Ȃ�
        //if (ballons.Length > 0)
        //{

            //�W�����v
            if (Input.GetButtonDown(jump)) //InputManager��Jump�̍��ڂɓo�^����Ă���L�[���͂𔻒肷��
            {
                Jump();
                //Walk();
                //Walk();
            }

            //�ڒn���Ă��Ȃ��i�󒆂ɂ���j�ԂŁA�������̏ꍇ
            if (isGrounded == false && rb.velocity.y < 0.15f)
            {
                //�����A�j�����J��Ԃ�
                anim.SetTrigger("Fall");
            }
        }
        else
        {
            Debug.Log("�o���[�����Ȃ��B�W�����v�s��");
        }

        //Velocity.y�̒l��5.0f�𒴂���ꍇ(�W�����v��A���ŉ������ꍇ�j
        if(rb.velocity.y > 5.0f)
        {
            //Velocity.y�̒l�ɐ�����������i���������ɏ��őҋ@�ł��Ă��܂����ۂ�h�����߁j
            rb.velocity = new Vector2(rb.velocity.x, 5.0f);
        }

        //�n�ʂɐڒn���Ă��āA�΂�[���������ł͂Ȃ��ꍇ
        if (isGrounded == true && isGenerating == false)
        {
            //Q�{�^������������
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //�o���[������쐬����
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
        rb.AddForce(transform.up * jumpPower); //�L�����̈ʒu��������Ɉړ�������i�W�����v�E���V)
        anim.SetTrigger("Jump"); //jump(UP+Mid)�A�j���[�V�������Đ�����
    }
    void FixedUpdate()
    {
        //�ړ�
        Move();
    }
    private void Move()
    {
         //�����i���j�����ւ̓��͎�t
        float x = Input.GetAxis(horizontal); //InputManager��Horizontal�ɓo�^����Ă���L�[�̓��͂����邩�ǂ����m�F���s��

        //x�̒l���O�łȂ��ꍇ���L�[���͂�����ꍇ
        if (x != 0)
        {
            //velocity�i���x�j�ɐV�����l�������Ĉړ�
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

            //temp�ϐ���localScale�l����
            Vector3 temp = transform.localScale;

            //���݂̃L�[���͒lx��temp.x�ɑ��
            temp.x = x;

            //�������ς�鎞�ɏ����ɂȂ�ƃL�������k��Ō�����̂Ő����l�ɂ���
            if (temp.x > 0)
            {
                //�������O�����傫����΂��ׂĂP�ɂ���
                temp.x = scale;
            }
            else
            {
                //�������O��菬������΂��ׂā]�P�ɂ���
                temp.x = -scale;
            }

            //�L�����̌������ړ������ɍ��킹��
            transform.localScale = temp;

            //�ҋ@��Ԃ̃A�j���Đ����~�߂āA����A�j���̍Đ��ւ̑J�ڂ��s��
            anim.SetBool("Idle", false);
            anim.SetFloat("Run", 0.5f); //Run�A�j���[�V�����ɑ΂��āA�O�D�T���̒l�����Ƃ��ēn���B�@�ۏ�����greater�O�D�P�Ȃ̂ŁA�O�D�P�ȏ�̒l��n���Ə������������Ăq�����A�j���[�V�������Đ������
        }
        else
        {
            //���E�̓��͂��Ȃ������牡�ړ��̑��x���O�ɂ��Ă�����~������
            rb.velocity = new Vector2(0, rb.velocity.y);
            //����A�j���Đ����~�߂āA�ҋ@��Ԃ̃A�j���̍Đ��ւ̑J�ڂ��s��
            anim.SetFloat("Run", 0.0f);
            anim.SetBool("Idle", true); 
        }

        //���݂̈ʒu��񂪈ړ��͈͂̐����͈͂𒴂��Ă��Ȃ����m�F����B�����Ă����琧���͈͓��Ɏ��߂�
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        //���݂̈ʒu��񂪈ړ��͈͂̐����͈͂𒴂��Ă��Ȃ����m�F����B�����Ă�����A�����͈͂Ɏ��߂�
        transform.position = new Vector2(posX, posY);        
    }

    //�o���[���쐬

    private IEnumerator GenerateBallon()
    {
        //�S�Ă̔z��̗v�f�Ƀo���[�������݂��Ă���ꍇ�ɂ́A�o���[���𐶐����Ȃ�
        if (ballons[1] != null)
        {
            yield break;
        }

        //��������Ԃɂ���
        isGenerating = true;

        // isFirstGenerateBallon �ϐ��̒l�� false�A�܂�A�Q�[�����J�n���Ă���A�܂��o���[�����P����������Ă��Ȃ��Ȃ�
        if (isFirstGenerateBallon == false)
        {
            // ����o���[���������s�����Ɣ��f���Atrue �ɕύX���� = ����ȍ~�̓o���[���𐶐����Ă��Aif ���̏����𖞂����Ȃ��Ȃ�A���̏����ɂ͓���Ȃ�
            isFirstGenerateBallon = true;

            Debug.Log("����̃o���[������");

            // startChecker �ϐ��ɑ������Ă��� StartChecker �X�N���v�g�ɃA�N�Z�X���āASetInitialSpeed ���\�b�h�����s����
            startChecker.SetInitialSpeed();
        }


            //�P�߂̔z��̗v�f����Ȃ�
            if (ballons[0] == null)
            {
                //�P�߂̃o���[���𐶐����āA�P�Ԗڂ̔z��֑��
                ballons[0] = Instantiate(ballonPrefab, ballonTrans[0]);

                ballons[0].GetComponent<Ballon>().SetUpBallon(this);
            }
            else
            {
                //�Q�ڂ̃o���[���𐶐����āA�Q�Ԗڂ̔z��֑��
                ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);

                ballons[1].GetComponent<Ballon>().SetUpBallon(this);
            }

            //�������ԕ��ҋ@
            yield return new WaitForSeconds(generateTime);


            //��������ԏI���B�ēx�����ł���悤�ɂ���
            isGenerating = false;
        }   
          
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        // �ڐG�����R���C�_�[�����Q�[���I�u�W�F�N�g��Tag��Enemy�Ȃ�
        if (col.gameObject.tag == "Enemy")
        {
            Vector3 direction = (transform.position - col.transform.position).normalized;
            // �L�����ƓG�̈ʒu���狗���ƕ������v�Z
            transform.position += direction * knockbackPower;
        }
    }




    /// �o���[���j��

    public void DestroyBallon()
    {
        // TODO ����A�o���[�����j�󂳂��ۂɁu���ꂽ�v�悤�Ɍ�����A�j�����o��ǉ�����

        if (ballons[1] != null)
        {
            Destroy(ballons[1]);
        }
        else if (ballons[0] != null)
        {
            Destroy(ballons[0]);
        }
    }

    // IsTrigger���I���̃R���C�_�[�����Q�[���I�u�W�F�N�g��ʉ߂����ꍇ�ɌĂяo����郁�\�b�h
    private void OnTriggerEnter2D(Collider2D col)
    {      
       if(col.gameObject.tag == "Coin")
        {
            // �ʉ߂����R�C���̃Q�[���I�u�W�F�N�g�̎��� Coin �X�N���v�g���擾���Apoint �ϐ��̒l���L�����̎��� coinPoint �ϐ��ɉ��Z
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            uiManager.UpdateDisplayScore(coinPoint);

            // �ʉ߂����R�C���̃Q�[���I�u�W�F�N�g��j�󂷂�
            Destroy(col.gameObject);
        }

    }


}