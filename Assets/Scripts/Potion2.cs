using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion2 : MonoBehaviour
{
    private ThrowBall Soccer;
    private Game_Manager GM;
    private void Start()
    {
        GM = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        Soccer = GameObject.Find("Soccer").GetComponent<ThrowBall>();
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
            Destroy(this.gameObject,0.3f);
            Soccer.energy1 = true;
        }
    }
}
