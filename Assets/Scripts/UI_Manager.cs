using Bazaar.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private string PrId = "kickyballkicky";
    [Header("Buttons")]
    [SerializeField] private Button[] init_Btn;
    [SerializeField] private Button purch_Btn;
    [SerializeField] private Button consum_Btn;

    [SerializeField] private Button[] kick_btn;
    [SerializeField] private Button[] select_btn;
    
    [Header("Status")]
    [SerializeField] private TextMeshProUGUI statusTxt;

    [Header("Managers")]
    [SerializeField] private Purchase_Manager PU_Manager;

    private bool _IsInitialized;

    private int num;

    [SerializeField] private GameObject pause_page;
    [SerializeField] private GameObject Stop_btn;
    [SerializeField] private GameObject Resume_btn;
    [SerializeField] private GameObject home_pause_btn;
    [SerializeField] private GameObject LoseBox;
    [SerializeField] private Button replay_btn;
    [SerializeField] private GameObject shop_btn;
    [SerializeField] private GameObject shop_page;
    [SerializeField] private Button close_shop_btn;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject BG1;
    [SerializeField] private GameObject BG2;

    [Header("ScoreThings")]
    [SerializeField] private TMP_Text BScore;
    [SerializeField] private TMP_Text Score;
    private int bscore;
    private int score;
    [SerializeField] private GameObject Score_Title;

    [Header("Balls")]
    [SerializeField] private GameObject[] Ball;
    private int wich_Ball;

    private int clicksInfinit = 0;
    [SerializeField] private Button infinit_btn;
    private bool[] buy_Ball = new bool[4];

    [Header("Audios")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private Slider Volume_sldr;
    private int select_mu;
    [SerializeField] private GameObject[] Select_mu;
    private float default_vol;
    [SerializeField] private Sprite no_vol_spr;
    [SerializeField] private Sprite up_vol_spr;
    [SerializeField] private GameObject vol_go;
    [SerializeField] private GameObject Setting_page;

    // Start is called before the first frame update
    void Start()
    {
        wich_Ball = PlayerPrefs.GetInt("wich_Ball", 4);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Ball[wich_Ball].SetActive(true);
            Debug.Log(wich_Ball);
        }
        buy_Ball[0] = PlayerPrefs.GetInt("B0", 1) == 1;
        buy_Ball[1] = PlayerPrefs.GetInt("B1", 1) == 1;
        buy_Ball[2] = PlayerPrefs.GetInt("B2", 1) == 1;
        buy_Ball[3] = PlayerPrefs.GetInt("B3", 1) == 1;
        init_Btn[0].gameObject.SetActive(buy_Ball[0]);
        init_Btn[1].gameObject.SetActive(buy_Ball[1]);
        init_Btn[2].gameObject.SetActive(buy_Ball[2]);
        init_Btn[3].gameObject.SetActive(buy_Ball[3]);
        kick_btn[0].gameObject.SetActive(buy_Ball[0]);
        kick_btn[1].gameObject.SetActive(buy_Ball[1]);
        kick_btn[2].gameObject.SetActive(buy_Ball[2]);
        kick_btn[3].gameObject.SetActive(buy_Ball[3]);
        select_btn[0].gameObject.SetActive(!buy_Ball[0]);
        select_btn[1].gameObject.SetActive(!buy_Ball[1]);
        select_btn[2].gameObject.SetActive(!buy_Ball[2]);
        select_btn[3].gameObject.SetActive(!buy_Ball[3]);

        init_Btn[0].onClick.AddListener(IdPr0);
        init_Btn[1].onClick.AddListener(IdPr1);
        init_Btn[2].onClick.AddListener(IdPr2);
        init_Btn[3].onClick.AddListener(IdPr3);
        //purch_Btn.onClick.AddListener(OnPurchClick);

        Stop_btn.GetComponent<Button>().onClick.AddListener(PausePageEnable);
        Resume_btn.GetComponent<Button>().onClick.AddListener(Resume);
        home_pause_btn.GetComponent<Button>().onClick.AddListener(ToHome);
        replay_btn.onClick.AddListener(Replay);
        shop_btn.GetComponent<Button>().onClick.AddListener(Shop);
        close_shop_btn.onClick.AddListener(closeShop);

        bscore = PlayerPrefs.GetInt("BestScore", 0);

        activePuBtns(false);

        select_btn[0].onClick.AddListener(() => selectBall(0));
        select_btn[1].onClick.AddListener(() => selectBall(1));
        select_btn[2].onClick.AddListener(() => selectBall(2));
        select_btn[3].onClick.AddListener(() => selectBall(3));
        select_btn[4].onClick.AddListener(() => selectBall(4));

        infinit_btn.onClick.AddListener(Infinity);

        select_mu = PlayerPrefs.GetInt("mu", 0);
        default_vol = PlayerPrefs.GetFloat("vol", 0.9f);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            audioSource[select_mu].GetComponent<AudioSource>().Play();
            audioSource[select_mu].volume = 0f;
            StartCoroutine(FadeInAudio(3f, default_vol));
        }
        Select_mu[0].GetComponent<Button>().onClick.AddListener(SelectMU0);
        Select_mu[1].GetComponent<Button>().onClick.AddListener(SelectMU1);
        Select_mu[2].GetComponent<Button>().onClick.AddListener(SelectMU2);
        Volume_sldr.value = audioSource[select_mu].volume;
        Volume_sldr.onValueChanged.AddListener(ChangeVolume);
        Debug.Log(select_mu);

    }
    private void IdPr0()
    {
        OnInit1Click();
        PrId = $"kickyballkicky{0}";
        num = 0;
    }
    private void IdPr1()
    {
        OnInit1Click();
        PrId = $"kickyballkicky{1}";
        num = 1;
    }
    private void IdPr2()
    {
        OnInit1Click();
        PrId = $"kickyballkicky{2}";
        num = 2;
    }
    private void IdPr3()
    {
        OnInit1Click();
        PrId = $"kickyballkicky{3}";
        num = 3;
    }
    private async void OnInit1Click()
    {
        SetStatusTxt("initializing...");
        var isSuccess = await PU_Manager.Init();
        if (!isSuccess) {
            SetStatusTxt("initialize failed", "red");
            return;
        }
        SetStatusTxt("initialize scceed", "green");
        _IsInitialized = true;
        //activePuBtns(true);
        OnPurchClick();
    }

    private async void OnPurchClick()
    {
        SetStatusTxt($"puchasing item {PrId}");
        var result = await PU_Manager.Purchase(PrId);

        if(result.status != Status.Success)
        {
            SetStatusTxt($"purchase failed! {result.message}", "red");
            return;
        }
        SetStatusTxt("purchase succeed", "green");
        purch_Btn.interactable = false;

        init_Btn[num].gameObject.SetActive(false);
        kick_btn[num].gameObject.SetActive(false);
        select_btn[num].gameObject.SetActive(true);
        save_shop_btns();
    }
    private void activePuBtns(bool active)
    {
        init_Btn[num].interactable = !active;
        purch_Btn.interactable = active;
    }

    private void SetStatusTxt(string status, string color = "white")
    {
        statusTxt.text = $"<color={color}>{status}</color>" + "\n--------------------\n" + statusTxt.text;
    }

    private void PausePageEnable()
    {
        pause_page.SetActive(true);
        Stop_btn.SetActive(false);
        Time.timeScale = 0f;
        audioSource[select_mu].Pause();
    } 
    private void Resume()
    {
        pause_page.SetActive(false);
        Stop_btn.SetActive(true);
        Time.timeScale = 1f;
        audioSource[select_mu].Play();
    }
    public void ToHome()
    {
        SceneManager.LoadScene(0);
    }
    private void Replay()
    {
        SceneManager.LoadScene(1);
    }
    public void LosePage(int S)
    {
        Stop_btn.SetActive(false);
        score = S;
        if(score > bscore)
        {
            bscore = score;
            PlayerPrefs.SetInt("BestScore",bscore);
            Debug.Log("s is more");
        }
        LoseBox.SetActive(true);
        Score.text = score.ToString();
        BScore.text = bscore.ToString();
        StartCoroutine(FadeOutAudio(3f));
    }
    public void Shop()
    {
        shop_page.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoseBox.SetActive(false);
        }
    }
    public void closeShop()
    {
        if (SceneManager.GetActiveScene().rootCount == 1)
        {
            LoseBox.SetActive(true);
        }
        shop_page.SetActive(false);
    }
    public void nextLevel(int R)
    {
        if (R == 20)
        {
            BG.SetActive(false);
            BG1.SetActive(true);
            //Debug.Log(Score_Title.transform.position);
            Score_Title.transform.position = new Vector3(525f, 1770f, 0f);
        }
        else if (R == 50)
        {
            BG1.SetActive(false);
            BG2.SetActive(true);
            Score_Title.transform.position = new Vector3(525f, 1634.6f, 0f);
        }
    }
    private void selectBall(int a)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Ball[0].SetActive(false);
            Ball[1].SetActive(false);
            Ball[2].SetActive(false);
            Ball[3].SetActive(false);
            Ball[4].SetActive(false);
            if (!Ball[a].activeInHierarchy)
            {
                Ball[a].SetActive(true);
            }
            
        }
        PlayerPrefs.SetInt("wich_Ball", a);
    }
    private void Infinity()
    {
        clicksInfinit ++;
        if (clicksInfinit == 1)
        {
            StartCoroutine(resetInfinit());
        }
        else if (clicksInfinit == 4)
        {
            GameObject.FindGameObjectWithTag("Ball").GetComponent<ThrowBall>().infinit = true;
        }
    }
    IEnumerator resetInfinit()
    {
        yield return new WaitForSeconds(3f);
        clicksInfinit = 0;
    }
    public void start()
    {
        SceneManager.LoadScene(1);
    }
    private IEnumerator FadeInAudio(float duration, float targetVolume)
    {
        float startVolume = audioSource[select_mu].volume;
        float elapsedTime = 0f;

        if (!audioSource[select_mu].isPlaying)
        {
            audioSource[select_mu].Play();
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource[select_mu].volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            Volume_sldr.value = audioSource[select_mu].volume;
            yield return null;
        }
        audioSource[0].volume = targetVolume;
    }
    private IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource[select_mu].volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource[select_mu].volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null; 
        }

        audioSource[select_mu].volume = 0f;
        audioSource[select_mu].Stop();
    }
    private void ChangeVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource[select_mu].volume = volume;
            PlayerPrefs.SetFloat("vol",volume);
            default_vol = volume;
            if (volume == 0f)
            {
                vol_go.GetComponent<Image>().sprite = no_vol_spr;
            }
            else
            {
                vol_go.GetComponent<Image>().sprite = up_vol_spr;
            }
            Debug.Log(volume);
        }
    }
    public void SelectMU0()
    {
        select_mu = 0;
        PlayerPrefs.SetInt("mu", 0);
        audioSource[0].GetComponent<AudioSource>().Play();
        if (audioSource[1] != null)
        {
            audioSource[1].GetComponent<AudioSource>().Stop();
        }
        if (audioSource[2] != null)
        {
            audioSource[2].GetComponent<AudioSource>().Stop();
        }
        Debug.Log(select_mu);
        audioSource[0].volume = Volume_sldr.value;
    }
    public void SelectMU1()
    {
        select_mu = 1;
        PlayerPrefs.SetInt("mu", 1);
        audioSource[1].GetComponent<AudioSource>().Play();
        if (audioSource[0] != null)
        {
            audioSource[0].GetComponent<AudioSource>().Stop();
        }
        if (audioSource[2] != null)
        {
            audioSource[2].GetComponent<AudioSource>().Stop();
        }
        Debug.Log(select_mu);
        audioSource[1].volume = Volume_sldr.value;
    }
    public void SelectMU2()
    {
        select_mu = 2;
        PlayerPrefs.SetInt("mu", 2);
        audioSource[2].GetComponent<AudioSource>().Play();
        if (audioSource[0] != null)
        {
            audioSource[0].GetComponent<AudioSource>().Stop();
        }
        if (audioSource[1] != null)
        {
            audioSource[1].GetComponent<AudioSource>().Stop();
        }
        Debug.Log(select_mu);
        audioSource[2].volume = Volume_sldr.value;
    }
    public void SetMu()
    {
        if (audioSource[0] != null)
        {
            audioSource[0].GetComponent<AudioSource>().Stop();
        }
        if (audioSource[1] != null)
        {
            audioSource[1].GetComponent<AudioSource>().Stop();
        }
        if (audioSource[2] != null)
        {
            audioSource[2].GetComponent<AudioSource>().Stop();
        }
        Setting_page.SetActive(false);
    }
    public void SettingPage()
    {
        if (shop_page.activeSelf == false)
        {
            Setting_page.SetActive(true);
        }
    }
    private void save_shop_btns()
    {
        PlayerPrefs.SetInt($"B{num}", 0);
        PlayerPrefs.Save();
    }
    public void exitgame()
    {
        Application.Quit();
    }
}
