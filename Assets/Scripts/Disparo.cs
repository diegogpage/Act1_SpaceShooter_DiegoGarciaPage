using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    [SerializeField] private AudioSource sonidoLaser;
    [SerializeField] private Explosion explosionPrefab;
    private float timer;

    //Creo la pool y hago que se pueda acceder desde otro script
    //desde este accedo con myPool y desde otro con MyPool
    private ObjectPool<Disparo> myPoolDisp;

    public ObjectPool<Disparo> MyPoolDisp { get => myPoolDisp; set => myPoolDisp = value; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        timer += Time.deltaTime;

        if(timer >= 4)
        {
            myPoolDisp.Release(this); //Aqui uso myPool porque estoy en este script
            timer = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Enemigo"))
        {
            myPoolDisp.Release(this);
            timer = 0;//Lo reseteo para que no se active con el timer empezado
        }

        else if (elOtro.gameObject.CompareTag("Boss"))
        {
            myPoolDisp.Release(this);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            timer = 0;//Lo reseteo para que no se active con el timer empezado
        }
    }
}
