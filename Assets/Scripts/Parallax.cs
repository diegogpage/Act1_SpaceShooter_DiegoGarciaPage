using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float anchoImagen;
    [SerializeField] private GameObject player;
    //Pongo gameobject y no player para que en el menu se pueda mover el fondo sin necesidad de hacer cambios
    //ya que en el menu no va a haber ningún "Player"

    private Vector3 posicionInicial;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Desplazo solo si el jugador sigue vivo
        if (player != null)
        {
            float espacio = velocidad * Time.time;
            //Interpretamos este espacio por ciclos (como si pasaramos de horas a dias)
            
            float resto = espacio % anchoImagen;
            //% hace la division y se queda con el resto. El espacio aumenta con el tiempo.
            //de esta forma, cuando resto = 0 querrá decir que el espacio recorrido es igual
            //al ancho de imagen; es decir, habremos cumplido un ciclo entero
            
            transform.position = posicionInicial + resto * Vector3.left;
            //Cuando resto = 0, transform.position = posicionInicial

        }

    }
}
