using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Boss : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private DisparoEnemigo disparoPrefab;
    [SerializeField] private GameObject posDisparo;
    [SerializeField] private ExplosionFuerte explosionPrefab;
    [SerializeField] private int vida;


    private bool abajo;
    private bool avanzar = true;
    private float timerDisparar;
    public static bool boss = true;

    private ObjectPool<DisparoEnemigo> disparoBossPool;


    private void Awake()
    {
        disparoBossPool = new ObjectPool<DisparoEnemigo>(CrearDisparo, GetDisparo, ReleaseDisparo, DestroyDisparo);
    }

    private DisparoEnemigo CrearDisparo()
    {
        DisparoEnemigo disparoCopia = Instantiate(disparoPrefab, posDisparo.transform.position, Quaternion.identity);
        disparoCopia.MyPoolDispEnmy = disparoBossPool;
        return disparoCopia;
    }

    private void GetDisparo(DisparoEnemigo disparoBoss)
    {
        disparoBoss.gameObject.SetActive(true);
        disparoBoss.transform.position = posDisparo.transform.position;
    }

    private void ReleaseDisparo(DisparoEnemigo disparoBoss)
    {
        disparoBoss.gameObject.SetActive(false);
    }

    private void DestroyDisparo(DisparoEnemigo disparoBoss)
    {
        Destroy(disparoBoss.gameObject);
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (avanzar)
        {
            Avance();
        }
        else
        {
            CambioDireccion();
        }
        

        timerDisparar += Time.deltaTime;
        if (timerDisparar >= 0.5f)
        {
            disparoBossPool.Get();
            timerDisparar = 0;
        }

        if (vida <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Debug.Log("Boss ha muerto");
            boss = false;
        }
        
    }

    void Avance()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
    }

    void CambioDireccion()
    {
        if (abajo)
        {
            transform.Translate(new Vector3(0, -1, 0) * velocidad * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(0, 1, 0) * velocidad * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("StopAvance"))
        {
            avanzar = false;
        }

        if (elOtro.gameObject.CompareTag("CambioDireccion"))
        {
            if (abajo)
            {
                abajo = false;
            }
            else
            {
                abajo = true;
            }
        }

        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            vida -= 1;
        }
    }
}
