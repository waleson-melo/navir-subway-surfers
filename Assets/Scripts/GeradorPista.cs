using System.Collections.Generic;
using UnityEngine;

public class GeradorPista : MonoBehaviour
{
    public GameObject trechoPistaPrefab;
    public float comprimentoTrecho = 10f;
    public int quantidadeInicial = 6;
    public Transform jogador;
    private float zUltimoTrecho;

    public GameObject moedaPrefab;
    public float chanceObstaculo = 0.3f;
    public float chanceMoeda = 0.5f;

    public List<GameObject> obstaculosPreFab = new List<GameObject>();

    private List<int> possiveisFaixas = new List<int> { -1, 0, 1 }; 

    void Start()
    {
        zUltimoTrecho = 0;

        for (int i = 0; i < quantidadeInicial; i++)
        {
            GerarTrecho();
        }
    }

    void Update()
    {
        if (jogador.position.z + (quantidadeInicial * comprimentoTrecho) > zUltimoTrecho)
        {
            GerarTrecho();
        }
    }

    void GerarTrecho()
    {
        Vector3 pos = new Vector3(0, 0, zUltimoTrecho);
        GameObject trecho = Instantiate(trechoPistaPrefab, pos, Quaternion.identity);
        AdicionarDestruidor(trecho);

        zUltimoTrecho += comprimentoTrecho;

        // Escolher faixa aleatória: -1 (esquerda), 0 (centro), 1 (direita)
        int faixa = possiveisFaixas[Random.Range(0, possiveisFaixas.Count)];
        float posX = faixa * 2f; // mesma largura usada nas faixas
        float posZ = zUltimoTrecho - (comprimentoTrecho / 2); // posiciona no meio do trecho
        Vector3 posItem = new Vector3(posX, 0, posZ);

        // Instanciar obstáculo ou moedas
        float sorteio = Random.value;
        if (sorteio < chanceObstaculo)
        {
            GameObject obstaculo = Instantiate(obstaculosPreFab[Random.Range(0, obstaculosPreFab.Count)], posItem, Quaternion.identity);
            AdicionarDestruidor(obstaculo);
        }
        else if (sorteio < chanceObstaculo + chanceMoeda)
        {
            int quantidadeMoedas = Random.Range(3, 6); // entre 3 e 5 moedas
            for (int i = 0; i < quantidadeMoedas; i++)
            {
                Vector3 posMoeda = new Vector3(
                    posItem.x,
                    posItem.y,
                    posItem.z + i * 1.5f // espaçamento entre moedas
                );

                GameObject moeda = Instantiate(moedaPrefab, posMoeda, Quaternion.Euler(0, 0, 0));
                AdicionarDestruidor(moeda);
            }
        }
    }

    void AdicionarDestruidor(GameObject obj)
    {
        var destruidor = obj.AddComponent<DestruirAtrasDoJogador>();
        destruidor.jogador = jogador;
    }
}
