using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public GameManager manager;
    public ParticleSystem effect;
    public bool isDrag; //기본값 false
    public bool isMerge;
    public bool isAttach;
    public bool isLine;
    public int level;
    
    public Rigidbody2D rigid;

    CircleCollider2D circle;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Color dongleColor;

    Coroutine fadeInColor = null;
    Coroutine fadeOutColor = null;

    float deadtime = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circle = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        dongleColor = spriteRenderer.color;
    }

    void OnEnable()
    {
        anim.SetInteger(AniParam.LEVEL, level);
        spriteRenderer.color = dongleColor;
    }

    void OnDisable()
    {
        level = 0;
        isDrag = false;
        isMerge = false;
        isAttach = false;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        rigid.simulated = false;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
        circle.enabled = true;
    }

    void Update()
    {
        if (isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //스크린 좌표를 월드 좌표로
                                                                                    //x축 경계 설정
            float leftBorder = -2.9f + transform.localScale.x;
            float rightBorder = 2.9f - transform.localScale.x;

            if (mousePos.x < leftBorder)
            {
                mousePos.x = leftBorder;
            }
            else if (mousePos.x > rightBorder)
            {
                mousePos.x = rightBorder;
            }

            mousePos.y = 4.2f;
            mousePos.z = 0;
            //부드럽게 따라다니게 lerp(현재위치, 목표위치, 따라가는 강도
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);
        }
    }

    public void Drag()
    {
        isDrag = true;
    }

    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }

    IEnumerator AttachRoutine()
    {
        if(isAttach) yield break;
        isAttach = true;

        SFX.Play(Sfx.Attach);

        yield return new WaitForSeconds(0.5f);

        isAttach = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(AttachRoutine());
        if (collision.gameObject.CompareTag(Tag.DONGLE))
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && !other.isMerge)
            {
                if (level < 8)
                {
                    //나와 상대 위치 가져오기
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    //1. 내가 아래
                    //2. 동일한 높이, 내가 오른쪽
                    if (meY < otherY || (meY == otherY && meX > otherX))
                    {
                        //상대 숨기기
                        other.Hide(transform.position);
                        //나 레벨업
                        LevelUp();
                    }
                }
                else // level >= 8
                {
                    GameManager.instance.BottomUp();
                    other.Hide(Vector3.up * 100);
                    other.EffectPlay();
                    Hide(Vector3.up * 100);
                    EffectPlay();
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.DONGLE))
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if (level == other.level && !isMerge && !other.isMerge)
            {
                if (level < 8)
                {
                    //나와 상대 위치 가져오기
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    //1. 내가 아래
                    //2. 동일한 높이, 내가 오른쪽
                    if (meY < otherY || (meY == otherY && meX > otherX))
                    {
                        //상대 숨기기
                        other.Hide(transform.position);
                        //나 레벨업
                        LevelUp();
                    }
                }
                else // level >= 8
                {
                    GameManager.instance.BottomUp();
                    other.Hide(Vector3.up * 100);
                    other.EffectPlay();
                    Hide(Vector3.up * 100);
                    EffectPlay();
                }
            }
        }
    }

    public void Hide(Vector3 targetPos)
    {
        isMerge = true;

        rigid.simulated = false;
        circle.enabled = false; //콜라이더 비활성화 enabled

        if(targetPos == Vector3.up*100)
        {
            EffectPlay();
        }

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while (frameCount < 20)
        {
            frameCount++;
            if (targetPos != Vector3.up * 100)
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            else
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.05f);
            yield return null;
        }

        manager.score += (int)Mathf.Pow(2, level);

        isMerge = false;
        gameObject.SetActive(false);
    }
    void LevelUp()
    {
        isMerge = true;

        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }
    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        EffectPlay();
        SFX.Play(Sfx.LevelUp);

        anim.SetInteger(AniParam.LEVEL, level + 1);

        yield return new WaitForSeconds(0.3f);
        level++;

        manager.maxLevel = Mathf.Max(level, manager.maxLevel);

        isMerge = false;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.FINISH))
        {
            deadtime += Time.deltaTime;

            if (fadeOutColor != null)
            {
                StopCoroutine(FadeOutColor());
                fadeInColor = null;
            }

            if (deadtime > 2 && fadeInColor == null)
                fadeInColor = StartCoroutine(FadeInColor());
            if (deadtime > 4)
                manager.GameOver();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.FINISH))
        {
            deadtime = 0;
            if (fadeInColor != null)
            {
                StopCoroutine(fadeInColor);
                fadeInColor = null;
            }
            if (fadeOutColor == null)
                fadeOutColor = StartCoroutine(FadeOutColor());
        }
    }
    IEnumerator FadeInColor()
    {
        while (spriteRenderer.color.g > 0f)
        {
            yield return new WaitForSeconds(0.01f);
            spriteRenderer.color = new Color(1, spriteRenderer.color.g - 0.01f, spriteRenderer.color.b - 0.01f, 1);
        }
    }
    IEnumerator FadeOutColor()
    {
        while (spriteRenderer.color.g < 1f)
        {
            yield return new WaitForSeconds(0.01f);
            spriteRenderer.color = new Color(1, spriteRenderer.color.g + 0.01f, spriteRenderer.color.b + 0.01f, 1);
        }
    }
    void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }
}
