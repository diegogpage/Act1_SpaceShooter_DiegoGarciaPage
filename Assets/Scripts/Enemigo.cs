using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Pool;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private DisparoEnemigo disparoPrefab;
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private GameObject posDisparo;
    [SerializeField] private GameObject[] powerUps;


    public static int navesDestruidas;
    private float timerDisparar;
    private float timerDesaparecer;
    private int vidasPlayer;


    private ObjectPool<DisparoEnemigo> disparoEnemigoPool;

    private ObjectPool<Enemigo> myPoolEnemy;

    public ObjectPool<Enemigo> MyPoolEnemy { get => myPoolEnemy; set => myPoolEnemy = value; }

    private void Awake()
    {
        disparoEnemigoPool = new ObjectPool<DisparoEnemigo>(CrearDisparo, GetDisparo, ReleaseDisparo, DestroyDisparo);
    }

    private DisparoEnemigo CrearDisparo()
    {
        DisparoEnemigo disparoCopia = Instantiate(disparoPrefab, posDisparo.transform.position, Quaternion.identity);
        disparoCopia.MyPoolDispEnmy = disparoEnemigoPool;
        return disparoCopia;
    }

    private void GetDisparo(DisparoEnemigo disparoEnemigo)
    {
        disparoEnemigo.gameObject.SetActive(true);
        disparoEnemigo.transform.position = posDisparo.transform.position;
    }

    private void ReleaseDisparo(DisparoEnemigo disparoEnemigo)
    {
        disparoEnemigo.gameObject.SetActive(false);
    }

    private void DestroyDisparo(DisparoEnemigo disparoEnemigo)
    {
        Destroy(disparoEnemigo.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);

        timerDisparar += Time.deltaTime;
        if (timerDisparar >= 1)
        {
            disparoEnemigoPool.Get();
            timerDisparar = 0;
        }

        timerDesaparecer += Time.deltaTime;
        if (timerDesaparecer >= 5)
        {
            myPoolEnemy.Release(this);
            timerDesaparecer = 0;
        }

        vidasPlayer = Player.vidas;
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if(elOtro.gameObject.CompareTag("Player"))
        {
            myPoolEnemy.Release(this);
            HacerExplosion();
            timerDesaparecer = 0;//Lo reseteo para que no se active con el timer empezado
            navesDestruidas++;
        }

        else if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            myPoolEnemy.Release(this);
            HacerExplosion();
            timerDesaparecer = 0;
            navesDestruidas++;

            //Spawneo los power ups

            float ptje = Random.value;

            if(ptje <= 0.15f) //Movimiento
            {
                Instantiate(powerUps[0], transform.position, Quaternion.identity);
            }
            else if (ptje <= 0.3f) //Velocidad
            {
                Instantiate(powerUps[1], transform.position, Quaternion.identity);
            }
            else if (ptje <= 0.4f) //Disparos
            {
                Instantiate(powerUps[2], transform.position, Quaternion.identity);
            }
            else if (ptje <= 0.6f) //Vida
            {
                if(vidasPlayer < 5)
                {
                    Instantiate(powerUps[3], transform.position, Quaternion.identity);
                }
                
            }

        }
    }

    void HacerExplosion()
    {
        Explosion explosionCopia = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

}
