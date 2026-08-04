using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    private float maxHealth = 100;
    private float currentHeath;
    private float damage = 25;

    //Position
    private Vector2Int currentCell = new Vector2Int();
    private Vector2Int currentChunk = new Vector2Int();
    private Vector2 chunkOriginPosition = new Vector2();

    private int chunkSize = WorldManager.cellsQuantityInChunk * WorldManager.cellSize;
    private Vector2Int targetCellIndex = new Vector2Int(-1, -1);
    private Vector3 targetCellWorldPosition;
    private int maxCellsDeslocationWalk = 10;
    private List<Vector2Int> walkedCells;

    //Movement
    private float moveSpeed = 8f;
    private float runningAcrescent = 10f;
    private Vector3 direction;


    //Visão
    private int distanceOfPerception = 5; // Distância da visão

    //Estaods
    private enum EnemyState{
        Patrol,
        Chase,
        Search,
        Attack,
        Dead
    }

    private EnemyState currentState = EnemyState.Patrol;

    void Awake()
    {
        currentHeath = maxHealth;

        currentChunk.x = Mathf.CeilToInt(transform.position.x / chunkSize);
        currentChunk.y = Mathf.CeilToInt(transform.position.y / chunkSize);

        walkedCells = new List<Vector2Int>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 

        VerifyChunkPosition();
        VerifyIfCellChange();
    }
    void Update()
    {
        VerifyChunkPosition();
        VerifyIfCellChange();
        EnemyStates();
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
        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x / chunkSize),
            Mathf.FloorToInt(transform.position.z / chunkSize)
        );

        chunkOriginPosition = currentChunkPosition * chunkSize;

        Vector2Int currentCellPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x - chunkOriginPosition.x),
            Mathf.FloorToInt(transform.position.z - chunkOriginPosition.y)
        );

        bool changed = currentCellPosition != currentCell;

        if (changed) Debug.Log($"Mudou de célula | {currentCellPosition}");

        currentCell = currentCellPosition;
        return changed;
    }

    // -----------------  Logica de movimentacao  -------------------

    private void RemoveDeadEndPaths(Chunk chunk, Vector2Int currentCell, List<Vector2Int> neighboors)
    {
        for (int i = neighboors.Count - 1; i >= 0; i--)
        {
            int dx = neighboors[i].x - currentCell.x;
            int dy = neighboors[i].y - currentCell.y;

            for (int j = 0; j <= distanceOfPerception; j++)
            {
                int x = neighboors[i].x + (dx * j);
                int y = neighboors[i].y + (dy * j);

                // Se saiu da chunk, não remove o caminho
                // Não dá para afirmar que é um beco
                if (!chunk.IsValidCell(x, y))
                    break;

                Cell targetCell = chunk.GetCell(x, y);
                HashSet<WorldManager.Direction> openedWalls = targetCell.GetOpenedWalls();

                // Encontrou uma saída lateral
                if ((dx != 0) &&
                    (openedWalls.Contains(WorldManager.Direction.Top) ||
                    openedWalls.Contains(WorldManager.Direction.Bottom)))
                {
                    break;
                }

                if ((dy != 0) &&
                    (openedWalls.Contains(WorldManager.Direction.Left) ||
                    openedWalls.Contains(WorldManager.Direction.Right)))
                {
                    break;
                }

                // Não conseguiu enxergar o fim do corredor
                if (
                    (dx == 1 && j == distanceOfPerception && openedWalls.Contains(WorldManager.Direction.Right)) ||
                    (dx == -1 && j == distanceOfPerception && openedWalls.Contains(WorldManager.Direction.Left)) ||
                    (dy == 1 && j == distanceOfPerception && openedWalls.Contains(WorldManager.Direction.Bottom)) ||
                    (dy == -1 && j == distanceOfPerception && openedWalls.Contains(WorldManager.Direction.Top))
                )
                {
                    break;
                }

                // Encontrou o fim do corredor
                bool deadEnd =
                    (dx == 1 && !openedWalls.Contains(WorldManager.Direction.Right)) ||
                    (dx == -1 && !openedWalls.Contains(WorldManager.Direction.Left)) ||
                    (dy == 1 && !openedWalls.Contains(WorldManager.Direction.Bottom)) ||
                    (dy == -1 && !openedWalls.Contains(WorldManager.Direction.Top));

                if (deadEnd)
                {
                    neighboors.RemoveAt(i);
                    break;
                }
            }
        }
    }
    
    private Vector2Int ChooseNextTargetCell()
    {
        //Verifica se ja visitou todas as casas adjacentes
        //Se sim, realiza backtracking para onde tiver casas novas
        //Se nao, escolhe uma casa aleatoria que ainda nao tenha visitado

        Chunk chunk = WorldManager.Instance.GetChunk(currentChunk);

        List<Vector2Int> neighboors = chunk.GetValidCellNeighboorsIndex(currentCell);

        for(int i = 0; i < walkedCells.Count; i++)
        {
            if(neighboors.Contains(walkedCells[i]))
            {
                neighboors.Remove(walkedCells[i]);
            }
        }

        RemoveDeadEndPaths(chunk, currentCell, neighboors);

        //Se não tiver celulas novas volta para a ultima celula visitada (backtracking)
        if (neighboors.Count == 0) return new Vector2Int(walkedCells[walkedCells.Count - 1].x, walkedCells[walkedCells.Count - 1].y);

        float percentage = 1 / neighboors.Count;
        float value = Noise.DefaultNoise(chunk.seed, currentCell.y, currentCell.x, 275);

        for(int i = 0; i < neighboors.Count; i++)
        {
            if(value < percentage * (i + 1))
            {
                return neighboors[i];
            }
        }

        return new Vector2Int(-1, -1);
    }
    // ----------------- State Machine -------------------
    private void EnemyStates()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            
            case EnemyState.Attack:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Search:
                break;
            case EnemyState.Dead:
                break;
        }
    }
    
    // -----------------  Movimentacao  -------------------

    private void MovementForCell()
    {
        targetCellWorldPosition = new Vector3(
            chunkOriginPosition.x + (targetCellIndex.x * WorldManager.cellSize + WorldManager.cellSize / 2),
            transform.position.y,
            chunkOriginPosition.y + (targetCellIndex.y * WorldManager.cellSize + WorldManager.cellSize / 2)
        );

        direction = (targetCellWorldPosition - transform.position).normalized * moveSpeed;

        rb.linearVelocity = direction;
    }

    // -----------------  Logica de patrulhamento  -------------------

    //O algoritmo de patrulha, pode ser algo como um acesso as casas adjacentes de forma aleatoria.
    //Sera guardado em uma pilha de tamanho fixo as ultimas x casas que visitou, e buscara sempre ir para uma nova.
    //Se ele encontrar um beco sem saida, tendera a nao entrar nele, ja que o todo ja esta visivel.
    //Se entrar em um local sem saida, onde as casas adjacentes ja foram preenchidas realizara um backtraking para onde tiver casas novas

    void UpdateWalkHistory()
    {
        if(walkedCells.Count >= maxCellsDeslocationWalk)
        {
            walkedCells.RemoveAt(0);
        }
        walkedCells.Add(currentCell);
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, targetCellWorldPosition) < 0.1f || targetCellIndex == new Vector2Int(-1, -1))
        {
            UpdateWalkHistory();
            targetCellIndex = ChooseNextTargetCell();
        }
        MovementForCell();
    }
}