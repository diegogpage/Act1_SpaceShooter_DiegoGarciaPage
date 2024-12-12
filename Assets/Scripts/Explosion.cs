using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] private float timeExplosion;
    [SerializeField] private AudioSource explodeSound;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= timeExplosion)
        {
            Destroy(gameObject);
        }
    }
}
