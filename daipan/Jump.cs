using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Animator animator;

    public GameObject koma;
    public Rigidbody rb;
    private Vector3 velocity;

    private float rayDistance = 1.2f;

    [SerializeField]
    private float jumpPower = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        velocity = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GroundCheck())
        {
            velocity = Vector3.zero;
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            /*
            if (input.magnitude > 0f)
            {
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(transform.position + input);
                velocity += input.normalized * 2;
                //　キーの押しが小さすぎる場合は移動しない
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }*/
            //　ジャンプキー（デフォルトではSpace）を押したらY軸方向の速度にジャンプ力を足す
            //if (Input.GetButtonDown("Jump"))
            if(Input.GetMouseButton(0))
            {
                velocity.y += jumpPower;
                //rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
                rb.AddForce(new Vector3(10,200,10));
            }
            
            velocity.y += Physics.gravity.y * Time.deltaTime;
            Vector3 pos = koma.transform.position;
            pos.y = velocity.y * Time.deltaTime;
            //koma.transform.position = pos;
        }
              
    }

    public bool GroundCheck()
    {
        
        Vector3 rayPosition = transform.position + new Vector3(0.0f, -0.01f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        bool isGrounded = Physics.Raycast(ray, rayDistance);

        return isGrounded;
    }
}
