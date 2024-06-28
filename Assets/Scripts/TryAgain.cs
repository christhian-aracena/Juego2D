using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour
{
    public string nombreEscenaJuego = "bug"; // Nombre de la escena del juego



    public void CargarEscenaJuego()
    {

        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void CargarEscenaMenu()
    {

        SceneManager.LoadScene("menu");
    }
}
