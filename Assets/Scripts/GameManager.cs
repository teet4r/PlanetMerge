using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----------[ Core ]")]
    public int score;
    public int maxLevel;
    public bool isOver;

    [Header("----------[ Object Pooling ]")]
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<Dongle> donglePool;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;
    [Range(1,30)]
    public int poolSize;
    public int poolCursor;
    public Dongle lastDongle;

    [Header("----------[ UI ]")]
    public GameObject endGroup;
    public Text scoreText;
    public Text maxScoreText;
    public Text subScoreText;
    public GameObject bottom;

    void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;

        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();
        for(int i=0; i<poolSize; i++)
            MakeDongle();
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.MAX_SCORE))
            PlayerPrefs.SetInt(PlayerPrefsKey.MAX_SCORE, 0);

        maxScoreText.text = PlayerPrefs.GetInt(PlayerPrefsKey.MAX_SCORE).ToString();
    }

    void Start()
    {
        NextDongle();
    }

    const string strEffect = "Effect";
    const string strDongle = "Dongle";
    Dongle MakeDongle()
    {
        //이펙트 생성
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = strEffect + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        //동글 생성
        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);
        instantDongleObj.name = strDongle + effectPool.Count;
        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>();
        instantDongle.manager = this;
        instantDongle.effect = instantEffect;
        donglePool.Add(instantDongle);

        return instantDongle;
    }
    Dongle GetDongle()
    {
        for(int i=0; i<donglePool.Count; i++)
        {
            poolCursor = (poolCursor+1) % donglePool.Count;
            if(!donglePool[poolCursor].gameObject.activeSelf)
                return donglePool[poolCursor];
        }
        return MakeDongle();
    }
    void NextDongle()
    {
        if(isOver) return;
        lastDongle = GetDongle();
        //lastDongle.level = Random.Range(8, 9); //쿠로미 두근거림 테스트용
        lastDongle.level = Random.Range(0, maxLevel); //마지막 숫자는 포함 안됨
        lastDongle.gameObject.SetActive(true);

        StartCoroutine(WaitNext());
    }
    IEnumerator WaitNext()
    {
        while (lastDongle != null)
            yield return null;
        yield return new WaitForSeconds(1.8f);

        NextDongle();
    }
    public void TouchDown()
    {
        if (lastDongle == null)
            return;
        lastDongle.Drag();
    }
    public void TouchUp()
    {
        if (lastDongle == null)
            return;
        lastDongle.Drop();
        lastDongle = null;
    }
    public void GameOver()
    {
        if(isOver) return;
        isOver = true;

        StartCoroutine(GameOverRoutine());
    }

    const string strScore = "점수 : ";
    IEnumerator GameOverRoutine()
    {
        // 장면안에 활성화 되어있는 모든 동글 가져오기
        Dongle[] dongles = FindObjectsOfType<Dongle>();

        // 윗 목록 하나씩 접근해서 삭제
        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].rigid.simulated = false;
        }

        // 윗 목록 하나씩 접근해서 삭제
        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        //최고 점수 갱신
        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt(PlayerPrefsKey.MAX_SCORE));
        PlayerPrefs.SetInt(PlayerPrefsKey.MAX_SCORE, maxScore);


        //게임오버 ui
        subScoreText.text = strScore + scoreText.text;
        endGroup.SetActive(true);
    }
    public void Reset()
    {
        StartCoroutine(ResetCorutine());
    }
    IEnumerator ResetCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
    public void Home()
    {
        StartCoroutine(HomeCorutine());
    }
    IEnumerator HomeCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
            Application.Quit();
    }
    void LateUpdate()
    {
        scoreText.text = score.ToString();    
    }

    public void BottomUp()
    {
        StartCoroutine(BottomUpRoutine());
    }

    IEnumerator BottomUpRoutine()
    {
        for (int i = 0; i < 40; i++)
        {
            bottom.transform.localPosition += new Vector3(0, 0.01f, 0);
            yield return null;
        }
    }
}
