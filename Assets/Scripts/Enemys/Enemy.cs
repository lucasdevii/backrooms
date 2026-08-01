using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHeath;
    private float damage = 25;

    //Position
    private Vector2Int currentCell = new Vector2Int();
    private Vector2Int currentChunk = new Vector2Int();
    private int chunkSize = WorldManager.cellsQuantityInChunk * WorldManager.cellSize;

    private int maxCellsDeslocationWalk = 10;
    private Vector2Int[] walkedCells;
    private int currentWalkedCellsIndex = 0;

    void Awake()
    {
        currentHeath = maxHealth;

        currentChunk.x = Mathf.CeilToInt(transform.position.x / chunkSize);
        currentChunk.y = Mathf.CeilToInt(transform.position.y / chunkSize);

        walkedCells = new Vector2Int[maxCellsDeslocationWalk];
    }

    void Start()
    {
    }
    void Update()
    {
        VerifyChunkPosition();
        VerifyIfCellChange();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null) GiveDamage(player);
        }
    }

    void GiveDamage(Player player)
    {
        player.ReceiveDamage(damage);
    }

    // -----------------  Logica de posicao  -------------------

    bool VerifyChunkPosition()
    {
        if(transform.position == null) return false;

        float chunkSize = WorldManager.cellsQuantityInChunk * WorldManager.cellSize;

        Vector2Int chunkPosition = new Vector2Int(
            Mathf.CeilToInt(transform.position.x / chunkSize),
            Mathf.CeilToInt(transform.position.y / chunkSize)
        );

        bool changed = currentChunk != chunkPosition;

        if (changed) Debug.Log($"Mudou de chunk | {chunkPosition}");

        currentChunk = chunkPosition;
        return changed;
    }


    bool VerifyIfCellChange()
    {
        if (transform == null) return false;

        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x / chunkSize),
            Mathf.FloorToInt(transform.position.z / chunkSize)
        );

        Vector2Int chunkOrigin = currentChunkPosition * chunkSize;

        Vector2Int currentCellPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x - chunkOrigin.x),
            Mathf.FloorToInt(transform.position.z - chunkOrigin.y)
        );

        bool changed = currentCellPosition != currentCell;

        if (changed) Debug.Log($"Mudou de célula | {currentCellPosition}");

        currentCell = currentCellPosition;
        return changed;
    }

    // -----------------  Logica de movimentacao  -------------------
    void GetNextCellToMove()
    {
        //Verifica se ja visitou todas as casas adjacentes
        //Se sim, realiza backtracking para onde tiver casas novas
        //Se nao, escolhe uma casa aleatoria que ainda nao tenha visitado

        
    }

    // -----------------  Logica de patrulhamento  -------------------

    //O algoritmo de patrulha, pode ser algo como um acesso as casas adjacentes de forma aleatoria.
    //Sera guardado em uma pilha de tamanho fixo as ultimas x casas que visitou, e buscara sempre ir para uma nova.
    //Se ele encontrar um beco sem saida, tendera a nao entrar nele, ja que o todo ja esta visivel.
    //Se entrar em um local sem saida, onde as casas adjacentes ja foram preenchidas realizara um backtraking para onde tiver casas novas

    void Patrol()
    {
        
    }


}