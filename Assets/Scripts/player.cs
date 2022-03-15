using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
  [Header("Moviment Variables")]

  public float speed;
  public float jumpForce;
  public bool isJumping;
  public bool doubleJump;

  [Header("Attack Variables")]
  public Transform attackCheck;
  public float radiusAttack;
  public LayerMask layerEnemy;
  float timeNextAttack;
  public float forcaHorizontal = 5;
  public float forcaVertical = 5;
  public float timeToDestroy = 0.3f;
  public float forcaHorizontalPadrao;

  bool vivo = true;


  private Rigidbody2D rig;
  private Animator anim;
  SpriteRenderer sprite;


  // Start is called before the first frame update
  void Start()
  {
    rig = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    sprite = GetComponent<SpriteRenderer>();
    forcaHorizontalPadrao = forcaHorizontal;
    GameController.instance.UpdateVidasText();
    GameController.instance.UpdateKillsText();
  }

  // Update is called once per frame
  void Update()
  {
    Move();
    Jump();
    Attack();
  }

  void Move()
  {
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
    transform.position += movement * Time.deltaTime * speed;

    if (Input.GetAxis("Horizontal") > 0f)
    {
      anim.SetBool("walk", true);
      if (sprite.flipX == true)
      {
        Flip();
      }
    }
    if (Input.GetAxis("Horizontal") < 0f)
    {
      anim.SetBool("walk", true);
      if (sprite.flipX == false)
      {
        Flip();
      }
    }
    if (Input.GetAxis("Horizontal") == 0f)
    {
      anim.SetBool("walk", false);
    }
  }

  void Flip()
  {
    sprite.flipX = !sprite.flipX;
    attackCheck.localPosition = new Vector2(-attackCheck.localPosition.x, attackCheck.localPosition.y);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);
  }

  void Jump()
  {
    if (Input.GetButtonDown("Jump"))
    {
      if (!isJumping)
      {
        rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        doubleJump = true;
        anim.SetBool("jump", true);
      }
      else
      {
        if (doubleJump)
        {
          rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
          doubleJump = false;
        }
      }
    }
  }

  void Attack()
  {
    if (timeNextAttack <= 0f)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        anim.SetTrigger("attack");
        timeNextAttack = 0.2f;
      }
    }
    else
    {
      timeNextAttack -= Time.deltaTime;
    }
  }

  void PlayerAttack()
  {
    Collider2D[] enemiesAttack = Physics2D.OverlapCircleAll(attackCheck.position, radiusAttack, layerEnemy);
    for (int i = 0; i < enemiesAttack.Length; i++)
    {
      //enemiesAttack[i].SendMessage("EnemyHit");
      GameController.instance.SetKills(1);

      Debug.Log(enemiesAttack[i].name);
      if (enemiesAttack[i].gameObject.CompareTag("Enemy"))
      {
        enemiesAttack[i].gameObject.GetComponent<Enemy>().enabled = false;

        BoxCollider2D[] boxes = enemiesAttack[i].gameObject.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D box in boxes)
        {
          box.enabled = false;
        }

        if (enemiesAttack[i].transform.position.x < transform.position.x)
        {
          forcaHorizontal *= -1;
        }

        enemiesAttack[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcaHorizontal, forcaVertical), ForceMode2D.Impulse);

        Destroy(enemiesAttack[i].gameObject, timeToDestroy);


        forcaHorizontal = forcaHorizontalPadrao;
      }
    }
  }

  public void PerdeVida() {
    if(vivo){
      vivo = false;
      anim.SetTrigger("death");
      GameController.instance.SetVidas(-1);
      gameObject.GetComponent<player>().enabled = false;
    } 
  }

  public void Reset() {
    //if(GameController.instance.GetVidas() >= 0){
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer == 8)
    {
      isJumping = false;
      anim.SetBool("jump", false);
    }
  }

  void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.layer == 8)
    {
      isJumping = true;
    }
  }
}
