using UnityEngine;

public class DestruirAtrasDoJogador : MonoBehaviour
{
    public Transform jogador;
    public float distanciaMaxima = 10f;

    void Update()
    {
        if (jogador != null && transform.position.z < jogador.position.z - distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }
}
