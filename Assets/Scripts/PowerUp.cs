using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float velocidad;
    
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);

        timer += Time.deltaTime;

        if(timer >= 5)
        {
            Destroy(this.gameObject);
            timer = 0;
        }
    }
}
