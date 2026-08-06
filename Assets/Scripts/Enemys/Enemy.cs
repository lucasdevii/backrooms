using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour 
{
    enum States
    {
        Patrol,
        Attack,
        Search
    }
    
    //Position
    Chunk currentChunk;
    Cell currentCell;

    //Estados
    private States currentMode = States.Patrol;

    private List<Vector2Int> walkedCells = new List<Vector2Int>(); 

    void Start(){

    }
    void Update(){
        
    }
}