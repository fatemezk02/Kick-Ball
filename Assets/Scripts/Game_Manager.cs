using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    private ThrowBall Soccer;
    public TMP_Text Snum;
    public bool GameOver = false;
    [SerializeField]
    private float maxWaite = 9f;
    [SerializeField]
    private float minWaite = 7f;
    [SerializeField]
    private GameObject[] potion;
    private ThrowBall TheBall;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitPotion());
        StartCoroutine(WaitePotion2());
        StartCoroutine(FindTheBll());
        //Soccer = GameObject.Find("Soccer").GetComponent<ThrowBall>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void callWait()
    {
        StartCoroutine(WaitPotion());
    }
    public void callWait2()
    {
        StartCoroutine(WaitePotion2());
    }
    IEnumerator WaitPotion()
    {
        Debug.Log("po2");
        float wait = Random.Range(minWaite, maxWaite);
        yield return new WaitForSeconds(wait);
        if (TheBall.energy2 == false && GameOver == false)
        {
            Instantiate(potion[1], new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(2.5f, 5.5f), 0f), Quaternion.identity);
        }
        else if (TheBall.energy2 == true && GameOver == false)
        {
            callWait();
        }
        
    }
    IEnumerator WaitePotion2()
    {
        Debug.Log("po1");
        float waite = Random.Range(5f, 7.5f);
        yield return new WaitForSeconds(waite);
        if (TheBall.energy1 == false && GameOver == false)
        {
            Instantiate(potion[0], new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(2.5f, 5.5f), 0f), Quaternion.identity);
        }
        else if (TheBall.energy1 == true && GameOver == false)
        {
            callWait2();
        }
    }
    public void SetScore(int S)
    {
        Snum.text = S.ToString();
    }
    IEnumerator FindTheBll()
    {
        yield return new WaitForSeconds(0.2f);
        TheBall = GameObject.FindGameObjectWithTag("Ball").GetComponent<ThrowBall>();
    }
}
