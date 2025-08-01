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
    private float minWaite = 5f;
    [SerializeField]
    private GameObject[] potion;
    // Start is called before the first frame update
    void Start()
    {
        Soccer = GameObject.Find("Soccer").GetComponent<ThrowBall>();
        StartCoroutine(WaitPotion());
    }

    // Update is called once per frame
    void Update()
    {
        Snum.text = Soccer.Score.ToString();
    }
    public void callWait()
    {
        StartCoroutine(WaitPotion());
    }
    IEnumerator WaitPotion()
    {
        float wait = Random.Range(minWaite, maxWaite);
        yield return new WaitForSeconds(wait);
        Instantiate(potion[Random.Range(1, 2)], new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(2.5f, 5.5f), 0f), Quaternion.identity);
    }
}
