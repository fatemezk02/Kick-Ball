using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Potion1 : MonoBehaviour
{
    private bool isTriggered = false;
    private Game_Manager GM;
    private GameObject Po2;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        StartCoroutine(destroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Destroy(this.gameObject, 0.2f);
            other.gameObject.GetComponent<ThrowBall>().energy2 = true;
            other.gameObject.GetComponent<ThrowBall>().Energy2();
            isTriggered = true;
            StartCoroutine(cancelEnergy(other.gameObject));
            if (GM.GameOver == false)
            {
                GM.callWait2();
            }
            if (FindObjectOfType<Potion2>() != null)
            {
                Po2 = GameObject.FindObjectOfType<Potion2>().gameObject;
                Destroy(Po2);
                GM.callWait();
            }
        }
    }
    IEnumerator cancelEnergy(GameObject g)
    {
        yield return new WaitForSeconds(6f);
        g.GetComponent<ThrowBall>().energy2 = false;
        isTriggered = false;
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(6f);
        Destroy(this.gameObject);
        if (isTriggered == false && GM.GameOver == false)
        {
            GM.callWait2();
        }
    }
}
