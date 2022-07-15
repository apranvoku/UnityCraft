using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.W))
        {
            StartCoroutine(MoveForward());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MoveRight());
        }
    }
    public IEnumerator MoveForward()
    {
        for(int i = 0; i < 90; i+= 1)
        {
            transform.position += new Vector3(0f, 0f, 1/90f);
            transform.Rotate(transform.right - transform.right * -1f, 1f);
            yield return null;
            
        }
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator MoveRight()
    {
        for (int i = 0; i < 90; i += 1)
        {
            transform.position += new Vector3(1/90f, 0f, 0f);
            transform.Rotate(transform.forward - transform.forward * -1f, -1f);
            yield return null;
            
        }
        transform.rotation = Quaternion.identity;
    }
}
