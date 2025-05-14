using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int frameRate = 10;
    public Sprite[] idleSprites;

    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    private int currentFrame = 0;

    public Vector3 lastMousePosition = Vector3.zero;

    public bool isMouseDown = false;

    public float superGunDration = 3;
    private float superGunTimer = 0;

    public GameObject gunTop;
    public GameObject gunLeft;
    public GameObject gunRight;

    public int hp = 5;

    private float invincibleTime = 2;
    private bool isInvincible = false;
    private float invincibleTimer = 0;

    private float blinkInterval = 0.2f;
    private Animator blinkAnimator;

    public Sprite[] deathSprites;

    public AudioSource getBombAudio;
    public AudioSource getSuperGunAudio;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IdleAnimationUpdate();

        MoveUpdate();

        SuperGunUpdate();

        InvincibleUpdate();

        DeathAnimationUpdate();
    }

    void IdleAnimationUpdate() 
    {
        if (hp<=0) return;
        timer += Time.deltaTime;
        if (timer > 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % idleSprites.Length;
            spriteRenderer.sprite = idleSprites[currentFrame];
        }
    }

    void DeathAnimationUpdate()
    {
        if (hp > 0) return;
        timer += Time.deltaTime;
        if (timer > 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame++;
        }
        if (currentFrame >= deathSprites.Length)
        {
            //death
            GameManager.Instance.GameOver();
        }
        else
        {
            spriteRenderer.sprite = deathSprites[currentFrame];
        }
    }

    void MoveUpdate()
    {
        if (GameManager.Instance.IsPause() || hp <= 0) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
        if (isMouseDown)
        {
            Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;

            transform.position = transform.position + offset;
            CheckPosition();


            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
    }

    void CheckPosition()
    {
        Vector3 pos = transform.position;
        if (pos.x < -2.13f)
        {
            pos.x = -2.13f;
        }
        if (pos.x > 2.13f)
        {
            pos.x = 2.13f;
        }
        if (pos.y < -3.78f)
        {
            pos.y = -3.78f;
        }
        if (pos.y > 3.45f)
        {
            pos.y = 3.45f;
        }
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.IsPause() || hp <= 0) { return; }
        if (collision.tag == "Award")
        {
            if (collision.GetComponent<Award>().awardType == AwardType.SuperGun)
            {
                TransformToSuperGun();
                getSuperGunAudio.Play();
            }
            else
            {
                GameManager.Instance.AddBomb();
                getBombAudio.Play();
            }
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Enemy" && isInvincible == false)
        {
            collision.SendMessage("TakeDamage");
            this.hp--;
            if (this.hp <= 0)
            {
                TransformToDeath();

            }
            else
            {
                TransformToInvincible();
            }
        }
    }

    void SuperGunUpdate()
    {
        if (superGunTimer > 0)
        {
            superGunTimer -= Time.deltaTime;

            if (superGunTimer <= 0)
            {
                TransformToNormalGun();
            }
        }
    }

    void TransformToSuperGun()
    {
        if (GameManager.Instance.IsPause() || hp <= 0) { return; }

        gunLeft.SetActive(true);
        gunRight.SetActive(true);
        gunTop.SetActive(false);

        superGunTimer = superGunDration;

    }

    void TransformToNormalGun()
    {
        if (GameManager.Instance.IsPause() || hp <= 0) { return; }

        gunLeft.SetActive(false);
        gunRight.SetActive(false);
        gunTop.SetActive(true);
    }

    void DisableAllGun()
    {
        gunLeft.SetActive(false);
        gunRight.SetActive(false);
        gunTop.SetActive(false);
    }

    void TransformToDeath()
    {
        DisableAllGun();
        timer = 0;
        currentFrame = 0;
    }

    void TransformToInvincible()
    {
        isInvincible = true;
        invincibleTimer = 0;

        //StartCoroutine(BlinkEffect());
    }

    void InvincibleUpdate()
    {
        if (isInvincible)
        {
            blinkAnimator.enabled = true;
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer > invincibleTime)
            {
                isInvincible = false;
                blinkAnimator.Rebind(); // 重置所有动画状态和变换
                blinkAnimator.Update(0f); // 强制立即更新
                blinkAnimator.enabled = false;
            }
        }
    }

    //IEnumerator BlinkEffect()
    //{
    //    while (invincibleTimer <=invincibleTime)
    //    {
    //        spriteRenderer.enabled = !spriteRenderer.enabled;
    //        yield return new WaitForSeconds(blinkInterval);
    //        invincibleTimer += blinkInterval;

    //    }

    //    spriteRenderer.enabled = true;
    //    isInvincible = false;
    //}
}
