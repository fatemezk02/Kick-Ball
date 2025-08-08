using UnityEngine;

public class BallController3D : MonoBehaviour
{
    private Rigidbody rb;
    public float forceMultiplier = 10f;
    private int Score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ایجاد یک Ray از دوربین به موقعیت ماوس
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // بررسی برخورد Ray با توپ
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform )
                {
                    Score++;
                    // محاسبه جهت نیرو (از مرکز توپ به محل کلیک)
                    Vector3 hitPoint = hit.point;
                    Vector3 ballCenter = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z) ;
                    Vector3 forceDirection = (hitPoint - ballCenter).normalized;

                    // محاسبه محدوده یک پنجم بالایی توپ
                    float ballHeight = GetComponent<SphereCollider>().bounds.size.y;
                    float upperThreshold = ballCenter.y + (ballHeight * 0.44f); // یک پنجم بالایی (0.4 = 1 - 1/5)

                    // بررسی موقعیت برخورد
                    if (hitPoint.y > upperThreshold)
                    {
                        // اگر برخورد در یک پنجم بالایی باشد، جهت نیرو به سمت پایین
                        forceDirection = new Vector3(0, 0, 0).normalized;
                    }
                    else
                    {
                        // در غیر این صورت، جهت نیرو به سمت بالا
                        forceDirection = new Vector3(- forceDirection.x, Mathf.Abs(forceDirection.y), forceDirection.z).normalized;
                    }

                    // اعمال نیرو به توپ
                    rb.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
                }
            }
        }
    }
}