using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPrincipal : MonoBehaviour
{
    public string nombreEscenaJuego = "GameOver"; // Nombre de la escena del juego



    public void CargarEscenaJuego()
    {
    
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}
