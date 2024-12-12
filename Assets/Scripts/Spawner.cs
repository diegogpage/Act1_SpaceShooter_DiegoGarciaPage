using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Pool;
using System.Threading;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private int scoreBoss;

    private bool boss = false;
    private int naves;


    private ObjectPool<Enemigo> enemigoPool;

    private void Awake()
    {
        enemigoPool = new ObjectPool<Enemigo>(CrearEnemigo, GetEnemigo, ReleaseEnemigo, DestroyEnemigo);
    }

    private Enemigo CrearEnemigo()
    {
        Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.3f, 4.3f), 0);
        Enemigo copiaEnemigo = Instantiate(enemigoPrefab, puntoAleatorio, Quaternion.identity);
        copiaEnemigo.MyPoolEnemy = enemigoPool;
        return copiaEnemigo;
    }

    private void GetEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(true);
        Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.3f, 4.3f), 0);
        enemigo.transform.position = puntoAleatorio;
    }

    private void ReleaseEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }

    private void DestroyEnemigo(Enemigo enemigo)
    {
        Destroy(enemigo.gameObject);
    }





    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnearEnemigos());
    }

    // Update is called once per frame
    void Update()
    {
        naves = Enemigo.navesDestruidas;

        if (player == null || boss == true)
        {
            StopAllCoroutines();
        }

        //if (boss)
        //{
        //    for (int i = 0; i < 1; i++)
        //    {
        //        Instantiate(bossPrefab, transform.position, Quaternion.identity);
        //    }
            
        //}
    }

    IEnumerator SpawnearEnemigos()
    {
        
        for (int j = 0; j < 7; j++)
        {
            Debug.Log("Oleada " + (j + 1));
            
            if (j == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i < 5)
                    {
                        enemigoPool.Get();
                        yield return new WaitForSeconds(1.5f);
                    }
                    else if (i < 10)
                    {
                        enemigoPool.Get();
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        enemigoPool.Get();
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                yield return new WaitForSeconds(6f);
            }

            else if (j == 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i < 10)
                    {
                        enemigoPool.Get();
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        enemigoPool.Get();
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                yield return new WaitForSeconds(6f);
            }

            else
            {
                for (int i = 0; i < 20; i++)
                {
                    enemigoPool.Get();
                    yield return new WaitForSeconds(0.5f);
                }

                if (naves >= scoreBoss)
                {
                    yield return new WaitForSeconds(4f);
                    Debug.Log("Boss final");
                    boss = true;
                    Instantiate(bossPrefab, transform.position, Quaternion.identity);
                }
                
                yield return new WaitForSeconds(6f);

                
                
            }

        }

    }
}
