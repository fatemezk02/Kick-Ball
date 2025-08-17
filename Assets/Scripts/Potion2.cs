using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Potion2 : MonoBehaviour
{
    private ThrowBall Soccer;
    private Game_Manager GM;
    private UI_Manager UI;
    private GameObject Po1;
    private void Start()
    {
        UI = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        GM = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        StartCoroutine(destroy());
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
        GM.callWait();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Destroy(this.gameObject,0.2f);
            other.gameObject.GetComponent<ThrowBall>().energy1 = true;
            if(FindObjectOfType<Potion1>() != null)
            {
                Po1 = GameObject.FindObjectOfType<Potion1>().gameObject;
                Destroy(Po1);
                GM.callWait2();
            }
        }
    }
}
