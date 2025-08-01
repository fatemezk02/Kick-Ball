using System.Collections;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Rigidbody rb;
    public float throwForce = 10f;
    public float verticalSpeed = 10f;
    public float extraGravityForce = 5f;
    public Camera mainCamera;
    public int Score = 0;
    private Game_Manager GM;
    public bool energy1 = false;
    private int i = 0;
    private UI_Manager UI;
    void Start()
    {
        GM = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        UI = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; 
        rb.drag = 1f;
        rb.angularDrag = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        //rb.AddForce(Vector3.down * extraGravityForce, ForceMode.Acceleration);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform == transform && GM.GameOver == false)
                {
                    if (energy1 == true)
                    {
                        Score += 2;
                        i++;
                        if (i < 2 && !GM.GameOver)
                        {
                            StartCoroutine(cancelEnergy());
                        }
                    }
                    else if (energy1 == false)
                    {
                        Score++;
                    }

                    Vector3 hitPoint = hit.point;
                    Vector3 ballCenter = transform.position;
                    Vector3 forceDirection = (ballCenter - hitPoint).normalized;

                    forceDirection.y = 0;
                    forceDirection = forceDirection.normalized;

                    Vector3 currentVelocity = rb.velocity;
                    currentVelocity.y = verticalSpeed;

                    if (forceDirection.magnitude > 0)
                    {
                        currentVelocity.x = forceDirection.x * throwForce;
                        currentVelocity.z = forceDirection.z * throwForce;
                    }
                    else
                    {
                        currentVelocity.x = 0;
                        currentVelocity.z = 0;
                    }

                    rb.velocity = currentVelocity;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Ground")
        {
            GM.GameOver = true;
            UI.LosePage(Score);
        }
    }
    IEnumerator cancelEnergy()
    {
        yield return new WaitForSeconds(6f);
        i = 0;
        energy1 = false;
        GM.callWait();
    }
}