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

    // Start is called before the first frame update
    void Start()
    {
        wich_Ball = PlayerPrefs.GetInt("wich_Ball", 2);
        Ball[wich_Ball].SetActive(true);

        init_Btn[0].onClick.AddListener(IdPr0);
        init_Btn[1].onClick.AddListener(IdPr1);
        init_Btn[2].onClick.AddListener(IdPr2);
        init_Btn[3].onClick.AddListener(IdPr3);
        purch_Btn.onClick.AddListener(OnPurchClick);

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
    } 
    private void Resume()
    {
        pause_page.SetActive(false);
        Stop_btn.SetActive(true);
        Time.timeScale = 1f;
    }
    private void ToHome()
    {
        SceneManager.LoadScene(1);
    }
    private void Replay()
    {
        SceneManager.LoadScene(0);
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
    }
    private void Shop()
    {
        shop_page.SetActive(true);
        LoseBox.SetActive(false);
    }
    private void closeShop()
    {
        LoseBox.SetActive(true);
        shop_page.SetActive(false);
    }
    public void nextLevel(int R)
    {
        if (R == 2)
        {
            BG.SetActive(false);
            BG1.SetActive(true);
            Debug.Log(Score_Title.transform.position);
            Score_Title.transform.position = new Vector3(525f, 1770f, 0f);
        }
        else if (R == 3)
        {
            BG1.SetActive(false);
            BG2.SetActive(true);
            Score_Title.transform.position = new Vector3(525f, 1634.6f, 0f);
        }
    }
    private void selectBall(int a)
    {
        Ball[0].SetActive(false);
        Ball[1].SetActive(false);
        Ball[2].SetActive(false);
        Ball[3].SetActive(false);
        Ball[4].SetActive(false);
        if (! Ball[a].activeInHierarchy)
        {
            Ball[a].SetActive(true);
        }
        PlayerPrefs.SetInt("wich_Ball", 1);
    }
}
