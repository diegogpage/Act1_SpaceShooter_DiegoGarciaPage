using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform[] posDisparo;
    [SerializeField] private int numDisparos;
    [SerializeField] private GameObject[] imgVidas;
    [SerializeField] private AudioSource musicaFondo;
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private Boss bossPrefab;


    [SerializeField] private GameObject textoVidas;
    [SerializeField] private TextMeshProUGUI textoGameOver;
    [SerializeField] private TextMeshProUGUI textoNavesDestruidas;
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private TextMeshProUGUI textoWIN;
    [SerializeField] private GameObject botonRetry;
    [SerializeField] private GameObject botonExit;

    private float velocidad = 5f;
    private float timer = 0.5f; //Lo inicializo a 0.5 para que pueda disparar al empezar
    private float ratioDisparo = 0.5f;
    public static int vidas;
    private int navesDestruidas;
    private bool jefe = true;
    public static bool navePlayer;

    private float timerWIN;
    private float esperaWIN = 3f;

    private bool cambioMov = false;
    private float timerMov = 0;
    private bool movInvertido = false;

    private bool cambioVel = false;
    private float timerVel = 0;

    private bool cambioDisp = false;


    //Creo la pool para los disparos
    private ObjectPool<Disparo> poolDisparos;








    private void Awake()
    {
        //Como tengo 2 posiciones de disparo tengo que hacer el get manualmente
        poolDisparos = new ObjectPool<Disparo>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
    }


    private Disparo CrearDisparo()
    {
        //Creo un nuevo disparo y le digo que su piscina es esta
        Disparo disparoCopia = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        disparoCopia.MyPoolDisp = poolDisparos; 
        return disparoCopia;
    }

    private void ReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void DestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }










    // Start is called before the first frame update
    void Start()
    {
        //Para el reinicio
        vidas = 5;
        navePlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        DelimitarMovimiento();
        Disparar();
        CambioMovimiento();
        CambioVelocidad();
        CambioDisparo();
        ActualizarScore();
        Victoria();
    }

    void Movimiento()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");

        if (movInvertido)
        {
            transform.Translate(new Vector2(-movX, -movY).normalized * velocidad * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector2(movX, movY).normalized * velocidad * Time.deltaTime);
        }
        

    }

    void DelimitarMovimiento()
    {
        float xDelimitada = Mathf.Clamp(transform.position.x, -8.3f, 8.3f);
        float yDelimitada = Mathf.Clamp(transform.position.y, -4.3f, 4.3f);
        transform.position = new Vector3(xDelimitada, yDelimitada, 0);
    }

    void Disparar()
    {
        //En lugar de usar GetKeyDown, uso GetKey y meto un temporizador para que
        //no se pueda disparar constantemente
        
        timer += Time.deltaTime;

        if(Input.GetKey(KeyCode.Space) && timer >= ratioDisparo)
        {
            timer = 0;

            for(int i = 0; i < 2; i++)
            {
                Disparo disparoCopia = poolDisparos.Get();
                disparoCopia.gameObject.SetActive(true);
                disparoCopia.transform.position = posDisparo[i].transform.position;
                disparoCopia.transform.eulerAngles = new Vector3(0, 0, 0);
                //Esto último es para que después de la explosión de disparos siga disparando sin rotación en z
            }
            
        }

    }

    void ActualizarScore()
    {
        navesDestruidas = Enemigo.navesDestruidas;
        textoScore.text = "Score: " + navesDestruidas;
    }

    void CambioMovimiento()
    {
        if (cambioMov)
        {
            movInvertido = true;
            timerMov += Time.deltaTime;

            if (timerMov >= 10)
            {
                timerMov = 0;
                cambioMov = false;
                movInvertido = false;
            }
        }
        
    }

    void CambioVelocidad()
    {
        if (cambioVel)
        {
            velocidad = 10;
            timerVel += Time.deltaTime;
            if (timerVel >= 10)
            {
                cambioVel = false;
                timerVel = 0;
                velocidad = 5;
            }
        }
    }

    void CambioDisparo()
    {
        if (cambioDisp)
        {
            float gradosPorDisparo = 180 / numDisparos;
            
            for(float i = -90; i < 91; i+= gradosPorDisparo)
            {
                Disparo disparoCopia = poolDisparos.Get();
                disparoCopia.gameObject.SetActive(true);
                disparoCopia.transform.position = transform.position;
                disparoCopia.transform.eulerAngles = new Vector3(0, 0, i);
            }

            cambioDisp = false;
        }
    }

    void ActualizarVidas()
    {
        for (int i = 0; i < vidas; i++)
        {
            imgVidas[i].SetActive(true);
        }
        for (int j = vidas; j < 5; j++)
        {
            imgVidas[j].SetActive(false);
        }

    }

    void Victoria()
    {
        jefe = Boss.boss;

        if (jefe == false)
        {
            timerWIN += Time.deltaTime;

            if(timerWIN >= esperaWIN)
            {
                Destroy(gameObject);
                textoWIN.text = "WIN";
                textoNavesDestruidas.text = "Naves destruidas: " + navesDestruidas;
                botonRetry.gameObject.SetActive(true);
                botonExit.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo"))
        {
            vidas--;
            ActualizarVidas();

            if (vidas <= 0)
            {
                MuertePlayer();
            }
        }
        else if (elOtro.gameObject.CompareTag("Boss"))
        {
            MuertePlayer();
        }

        else if (elOtro.gameObject.CompareTag("PowerUpVel"))
        {
            Destroy(elOtro.gameObject);
            cambioVel = true;
            Debug.Log("Aumento de velocidad");
        }

        else if (elOtro.gameObject.CompareTag("PowerUpMov"))
        {
            Destroy(elOtro.gameObject);
            cambioMov = true;
            Debug.Log("Cambio de movimiento");
        }

        else if (elOtro.gameObject.CompareTag("PowerUpShoot"))
        {
            Destroy(elOtro.gameObject);
            cambioDisp = true;
            Debug.Log("Cambio el disparo");
        }

        else if (elOtro.gameObject.CompareTag("PowerUpVida"))
        {
            Destroy(elOtro.gameObject);
            vidas++;
            ActualizarVidas();
            Debug.Log("Vidas " + vidas);
        }
    }

    void MuertePlayer()
    {
        Destroy(this.gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        navePlayer = false;
        textoVidas.gameObject.SetActive(false);
        textoGameOver.text = "GAME OVER";
        textoNavesDestruidas.text = "Naves destruidas: " + navesDestruidas;
        botonRetry.gameObject.SetActive(true);
        botonExit.gameObject.SetActive(true);
        //Para que al reiniciar el juego no se acumulen las naves con las de la partida anterior
        Enemigo.navesDestruidas = 0;
    }
}
