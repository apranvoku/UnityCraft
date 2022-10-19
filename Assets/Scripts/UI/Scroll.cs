using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    Material mt;
    float scrollSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        mt = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * scrollSpeed;
        mt.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }
}
