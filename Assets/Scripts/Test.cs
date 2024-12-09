using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab;
    public float time;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate (prefab);
            yield return new WaitForSeconds(time);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
