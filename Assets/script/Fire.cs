using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Camera cam;
    public float firerate = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, 100f))
            {
                if(hit.transform.CompareTag("enemy"))
                {
                    AIExample enemy = hit.transform.gameObject.GetComponent<AIExample>();
                    enemy.health -= 5f;
                }
            }
        }
    }
}
