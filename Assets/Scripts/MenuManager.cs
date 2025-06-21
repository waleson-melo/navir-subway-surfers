using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jogar()
    {
        //rapaz
        SceneManager.LoadScene("Jogo"); // nome da cena do jogo
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Fechando jogo...");
    }
}
