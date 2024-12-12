using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Instrucciones()
    {
        SceneManager.LoadScene(2);
    }
    
    public void Controles()
    {
        SceneManager.LoadScene(3);
    }

    public void Exit()
    {
        Application.Quit();

    //#if UNITY_EDITOR
            // Detener el editor para simular salir del juego
            //UnityEditor.EditorApplication.isPlaying = false;
    //#else
            // Salir del juego en la compilación
            //pplication.Quit();
    //#endif
    }
}
