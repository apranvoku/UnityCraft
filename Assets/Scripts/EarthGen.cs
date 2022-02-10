using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGen : MonoBehaviour
{
    float inc;
    // Start is called before the first frame update
    void Start()
    {
        inc=0.5f;
        float y;
        /*
        float XRandOffset1 = Random.Range(-10000, 10000);
        float ZRandOffset1 = Random.Range(-10000, 10000);
        float XRandOffset2 = Random.Range(-10000, 10000);
        float ZRandOffset2 = Random.Range(-10000, 10000);
        float XRandOffset3 = Random.Range(-10000, 10000);
        float ZRandOffset3 = Random.Range(-10000, 10000);
        */
        
        float XRandOffset1 = 500;
        float ZRandOffset1 = 700;
        float XRandOffset2 = 2000;
        float ZRandOffset2 = 2200;
        float XRandOffset3 = 3000;
        float ZRandOffset3 = 5000;
        
        for (int x = 50; x < 150; x++)
        {
            for (int z = 50; z < 150; z++)
            {
                y = ((30 * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 200f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                    //+ (1 * ((2*(Mathf.PerlinNoise((x + XRandOffset2) / 4f, (z + ZRandOffset2) / 4f)))- 1)) // High Frequency
                    + (6 * ((2* (Mathf.PerlinNoise((x + XRandOffset3) / 30f, (z + ZRandOffset3) / 30f))) -1))); // Med Frequency
                //Debug.Log("Perlin result: " + y);
                GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 100, z, "Grass");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.PerlinNoise(inc / 2000f, (inc / 2000f)));
        //inc+= 0.323f;
    }
}
