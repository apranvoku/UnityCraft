using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 turn;
    public float speed;
    private GameObject Head;
    private Rigidbody rb;
    public bool gravity_on;
    // Start is called before the first frame update
    void Start()
    {
        Head = GameObject.Find("Head");
        rb = transform.GetComponent<Rigidbody>();
        rb.useGravity = false;
        gravity_on = true;
    }

    public void Gravity()
    {
        StartCoroutine(TurnOnGravity());
    }    
    public IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(2f);
        rb.useGravity = true;
    }
    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        //transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.W))
        {
            rb.MovePosition(transform.position += transform.forward/10f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.MovePosition(transform.position -= transform.right / 10f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.MovePosition(transform.position -= transform.forward / 10f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.MovePosition(transform.position += transform.right / 10f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * 300);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gravity_on = !gravity_on;
            if(gravity_on)
            {
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
        }
        //transform.Rotate(-turn.y, turn.x, 0f);
        transform.localRotation = Quaternion.Euler(0f, turn.x, 0f);
        Head.transform.localRotation = Quaternion.Euler(-turn.y, 0f, 0f);
        //Debug.DrawRay(Head.transform.GetChild(0).position, Head.transform.forward*10f, Color.cyan);
        //Debug.Log(rb.velocity.magnitude);
    }
}
