using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class DisparoEnemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;
    [SerializeField] private AudioSource sonidoLaser;
    private float timer;


    private ObjectPool<DisparoEnemigo> myPoolDispEnmy;

    public ObjectPool<DisparoEnemigo> MyPoolDispEnmy { get => myPoolDispEnmy; set => myPoolDispEnmy = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= 2)
        {
            myPoolDispEnmy.Release(this);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            myPoolDispEnmy.Release(this);
            timer = 0;//Lo reseteo para que no se active con el timer empezado

        }
    }
}
