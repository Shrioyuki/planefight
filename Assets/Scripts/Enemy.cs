using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 3;

    public int hp = 1;

    private bool isPlayDamageAni = false;
    private bool isPlayDeathAni = false;

    public Sprite[] damageSprites;
    public float frameRate = 10;
    private float timer = 0;
    private int currentFrame = 0;

    public Sprite[] deathSprites;

    private SpriteRenderer spriteRenderer;
    private Sprite idleSprite;

    public int score = 100;

    public AudioSource deathAudio;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idleSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();

        PlayDamageAnimationUpdate();

        PlayDeathAnimationUpdate();
    }

    void MoveUpdate()
    {
        if (hp > 0)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        
        if (transform.position.y < -5.5f)
        {

            Destroy(this.gameObject);
        }
    }

    void PlayDamageAnimationUpdate()
    {
        if (isPlayDamageAni == false) return;

        timer += Time.deltaTime;
        if (timer > 1 / frameRate)
        {
            currentFrame++;
            timer -= 1 / frameRate;
        }

        if (currentFrame >= damageSprites.Length)
        {
            ResetIdleState();
        }
        else
        {
            spriteRenderer.sprite = damageSprites[currentFrame];
        }

    }

    void PlayDeathAnimationUpdate()
    {
        if (isPlayDeathAni == false) return;

        timer += Time.deltaTime;
        if (timer > 1 / frameRate)
        {
            currentFrame++;
            timer -= 1 / frameRate;
        }

        if (currentFrame >= deathSprites.Length)
        {
            spriteRenderer.enabled = false;
            Destroy(this.gameObject,4);
        }
        else
        {
            spriteRenderer.sprite = deathSprites[currentFrame];
        }

    }

    void ResetIdleState()
    {
        isPlayDamageAni = false;
        this.spriteRenderer.sprite = idleSprite;
        timer = 0;
        currentFrame = 0;
    }

    void TakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage = 1)
    {
        if (hp <= 0) return;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            ResetIdleState();
            isPlayDamageAni = true;
        }
    }

    void Die()
    {
        ResetIdleState();
        isPlayDeathAni = true;
        GetComponent<Collider2D>().enabled = false;
        GameManager.Instance.AddScore(score);
        if (GameManager.Instance.IsPause()) { return; }
        deathAudio.Play();
    }

}
