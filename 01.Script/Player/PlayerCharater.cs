using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCharater : BaseManager
{
    [Header("플레이어 기본 세팅")]
    public GameObject head;
    public GameObject mainCamera;
    public GameObject compass;
    public GameObject sword;
    public GameObject flashObject;
    public GameObject flashLight;
    public float interation;

    [Header("스테이터스")]
    public float moveSpeed;
    public float boosterMove;
    public int maxHp;
    public float startOxygen;
    public float maxOxygen;
    public int currentHp;
    public float currentOxygen;
    public int attackDamage;
    public float jumpForce;

    [Header("소리")]
    public AudioSource playerSound;
    public AudioSource aciveSound;
    public AudioClip[] playerWalk;
    public AudioClip[] playerHit;
    public AudioClip[] playerJump;
    public AudioClip playerAttack;

    [Header("애니매이션")]
    private bool _bisMoveSound;
    Animator animator;

    [Header("시간")]
    public float noneTargetTime;
    public float boosterTime;

    [Header("bool")]
    public bool _bisJump;
    public bool _bisAttack;
    public bool _bisTarget;
    public bool _bisStop;


    float mouseX;
    float mouseY;
    Rigidbody rb;
    public void Start()
    {
        maxOxygen = Inventroy.instance.oxygenLevel * startOxygen + startOxygen;
        currentHp = GameInstance.instance.playerHp != 0 ? GameInstance.instance.playerHp : maxHp;
        currentOxygen = GameInstance.instance.playerOxygen != 0 ? GameInstance.instance.playerOxygen : maxOxygen;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _bisAttack = true;

        animator = this.gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mouseX = this.transform.rotation.eulerAngles.x;
        mouseY = this.transform.rotation.eulerAngles.y;
    }

   float whill = 0;
    private void Update()
    {
        if (!_bisStop)
        {
            Move();
            Rotation();
            Interaction();
            Jump();
            Use();
            GiveItem();
            currentOxygen -= Time.deltaTime;
            if(currentOxygen <= 0)
            {
                gameManager.GameOver(false);
            }

            whill += Input.GetAxis("Mouse Whill Scroll");
            whill = Mathf.Clamp(whill, 0, Inventroy.instance.maxBag);
            Inventroy.instance.select = (int)whill;

            if (boosterTime > 0) boosterTime -= Time.deltaTime;
            else boosterMove = 0;
            if(noneTargetTime > 0) noneTargetTime -= Time.deltaTime;
            _bisTarget = noneTargetTime > 0 ? false : true;
        } else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    #region move
    public void Move()
    {
        float moveX = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float moveZ = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;


        Vector3 movement = moveX * transform.forward + moveZ * transform.right;
        if(movement.magnitude != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("_bisRun", true);
            } else
            {
                animator.SetBool("_bisRun", false);
                animator.SetBool("_bisWalk", true);
            }
            if (_bisMoveSound && _bisJump)
            {
                _bisMoveSound = false;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    StartCoroutine(MoveSoundDelay(0.5f));
                }
                else
                {
                    StartCoroutine(MoveSoundDelay(1.5f));
                }
            }
        }
        float startMoveSpeed = moveSpeed + boosterMove;
        float currentMoveSpeed = Inventroy.instance.currentWeight < Inventroy.instance.maxWeight ? startMoveSpeed : startMoveSpeed / 2;
        float LastMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? currentMoveSpeed * 2 : currentMoveSpeed;
        movement.Normalize();
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z) * LastMoveSpeed;
    }

    public IEnumerator MoveSoundDelay(float _delay)
    {
        playerSound.clip = playerWalk[Random.Range(0, playerWalk.Length)];
        playerSound.Play();
        yield return new WaitForSeconds(_delay);
        _bisMoveSound = true;
    }
    #endregion

    #region rotation
    public void Rotation()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");

        mouseY = Mathf.Clamp(mouseY, -60, 20);

        transform.rotation = Quaternion.Euler(0, mouseX, 0);
        mainCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        mainCamera.transform.GetChild(0).gameObject.transform.LookAt(head.transform);
    }
    #endregion

    #region interaction
    public void Interaction()
    {
        RaycastHit hit;
        Transform  rayTransform= mainCamera.transform.GetChild(0).gameObject.transform;
        int mask = LayerMask.GetMask("Interaction");
        if(Physics.Raycast(rayTransform.position, rayTransform.forward, out hit, interation, mask))
        {
            if(hit.transform.gameObject.GetComponent<BaseInteraction>() != null)
            {
                BaseInteraction interaction = hit.transform.gameObject.GetComponent<BaseInteraction>();
                gameManager.ui.interactionAlert.gameObject.SetActive(true);
                gameManager.ui.interactionText.text = interaction.explant;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<BaseInteraction>().Interaction();
                }
            } else
            {
                gameManager.ui.interactionAlert.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region jump
    public void Jump()
    {
        if (_bisJump)
        {
            rb.AddForce(transform.up, ForceMode.Impulse);
            playerSound.clip = playerJump[Random.Range(0, playerJump.Length)];
            playerSound.Play();
            _bisJump = false;
        }
    }
    #endregion

    #region use
    public void Use()
    {
        if(Inventroy.instance.currentItems.Count > Inventroy.instance.select)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Inventroy.instance.currentItems[Inventroy.instance.select].use == ItemUse.sword && _bisAttack)
                {
                    _bisAttack = false;
                    StartCoroutine(Attack());
                }
                else if (Inventroy.instance.currentItems[Inventroy.instance.select].type == ItemType.enable)
                {
                    Inventroy.instance.UseItem();
                }
            }

            if (Inventroy.instance.currentItems[Inventroy.instance.select].use == ItemUse.sword)
            {
                sword.SetActive(true);
                flashLight.SetActive(false);
                flashObject.SetActive(false);
            }
            else if (Inventroy.instance.currentItems[Inventroy.instance.select].use == ItemUse.flashLight)
            {
                sword.SetActive(false);
                flashLight.SetActive(true);
                flashObject.SetActive(true);
            }
            else
            {
                sword.SetActive(false);
                flashLight.SetActive(false);
                flashObject.SetActive(false);
            }
        } else
        {
            sword.SetActive(false);
            flashLight.SetActive(false);
            flashObject.SetActive(false);
        }
    }

    public IEnumerator Attack()
    {
        animator.SetBool("_bisAttack", true);
        aciveSound.clip = playerAttack;
        aciveSound.Play();

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys) { 
            if(Vector3.Distance(enemy.transform.position, transform.position) < 5f)
            {
                enemy.gameObject.GetComponent<BaseEnemy>().Damage(attackDamage);
            }
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("_bisAttack", false);
        yield return new WaitForSeconds(1f);
        _bisAttack = true;
    }

    #endregion

    #region 버리기
    public void GiveItem()
    {
        if (Inventroy.instance.currentItems.Count > Inventroy.instance.select)
        {
            if (Inventroy.instance.currentItems[Inventroy.instance.select].type != ItemType.equment)
            {
                Inventroy.instance.GiveItem();
            }
        }
    }
    #endregion

    public void Damage(int _damage) {
        currentHp -= _damage;
        playerSound.clip = playerHit[Random.Range(0, playerHit.Length)];
        if(currentHp <= 0)
        {
            gameManager.GameOver(true);
        }
    }
}
