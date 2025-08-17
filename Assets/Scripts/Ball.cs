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
    public bool energy2 = false;
    private int i = 0;
    private int j = 0;
    private UI_Manager UI;
    private int n = 20;
    private int h = 1;
    [Header("mainSpeeds")]
    private float e;
    private float t;
    private float v;
    public bool infinit = false;
    void Start()
    {
        GM = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        UI = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; 
        rb.drag = 1f;
        rb.angularDrag = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = true;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * extraGravityForce, ForceMode.Acceleration);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform == transform && GM.GameOver == false && Time.timeScale == 1f)
                {
                    if (h == 1 && GM.GameOver == false)
                    {
                        h++;
                        rb.isKinematic = false;
                    }
                    if (energy1 == true)
                    {
                        Score += 2;
                        i++;
                        if (i < 2 && !GM.GameOver)
                        {
                            StartCoroutine(cancelEnergy());
                        }
                        if (Score >= n)
                        {
                            UI.nextLevel(n);
                            n = 50;
                        }
                    }
                    else if (energy1 == false)
                    {
                        Score++;
                        if (Score == n)
                        {
                            UI.nextLevel(n);
                            n = 50;
                        }
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
                        currentVelocity.z = 0f;
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
        GM.SetScore(Score);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Ground" && energy1 == false && infinit == false)
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
    IEnumerator cancelEnergy2()
    {
        yield return new WaitForSeconds(5f);
        j = 0;
        energy2 = false;
        GM.callWait2();
    }
    public void Energy2()
    {
        e = extraGravityForce;
        t = throwForce;
        v = verticalSpeed;
        if (energy2 == true)
        {
            extraGravityForce = 3.3f;
            throwForce = 5f;
            verticalSpeed = 5f;
        }
        StartCoroutine(BackFromE2());
    }
    IEnumerator BackFromE2()
    {
        yield return new WaitForSeconds(5f);
        energy2 = false;
        extraGravityForce = e;
        throwForce = t;
        verticalSpeed = v;
    }

}