using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Potion1 : MonoBehaviour
{
    private bool isTriggered = false;
    private Game_Manager GM;
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
            Destroy(this.gameObject, 0.3f);
            other.gameObject.GetComponent<ThrowBall>().energy2 = true;
            other.gameObject.GetComponent<ThrowBall>().Energy2();
            isTriggered = true;
            StartCoroutine(cancelEnergy(other.gameObject));
            if (GM.GameOver == false)
            {
                GM.callWait2();
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
