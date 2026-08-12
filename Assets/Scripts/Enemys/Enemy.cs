// using UnityEngine;
// using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
// public class Enemy : MonoBehaviour 
// {
//     enum State
//     {
//         Patrol,
//         Attack,
//         Search
//     }

//     Rigidbody rb;

//     //Movimentação
//     private float moveSpeed = 2;

//     //Position
//     private GridPosition gridPosition = new GridPosition();
//     private Cell targetCell;

//     //Informações do historico de celulas passadas
//     private List<Vector2Int> walkedCells = new List<Vector2Int>();
//     private int maxListSize = 10;


//     //Estados
//     private State currentMode = State.Patrol;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void Start()
//     {
//         gridPosition.Update(transform.position);
//         targetCell = Patrol.GetNextCellForPatrol(gridPosition.Chunk, gridPosition.Cell, walkedCells, maxListSize);
//     }

//     void Update()
//     {
//         gridPosition.Update(transform.position);

//         StateMachine();
//     }

//     void StateMachine()
//     {
//         switch(currentMode)
//         {
//             case State.Patrol: 
//                 PatrolMode();
//                 break;       

//             case State.Attack:
//                 break;

//             case State.Search:
//                 break;
//         }
//     }

//     void PatrolMode()
//     {
//         bool onTargetCell = Patrol.VerifyOnTarget(transform.position, targetCell);

//         if (onTargetCell)
//             targetCell = Patrol.GetNextCellForPatrol(gridPosition.Chunk, gridPosition.Cell, walkedCells, maxListSize);

//         Vector3 directionForce = Patrol.GetVelocityDirection(transform.position, targetCell, moveSpeed);
//         rb.linearVelocity = directionForce;
//     }
// }