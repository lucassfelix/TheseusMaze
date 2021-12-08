using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurMovement : MonoBehaviour
{
    
    private Stack<ICommand> _minotaurMoves; // Stores past moves by the minotaur
    private ICommand _nextMove;
    private Vector2 _nextDirection;
    private Boolean _hasChanged;
    
    public LayerMask wallLayerMask;    

    void Start()
    {
        _minotaurMoves = new Stack<ICommand>(); 
    }
    
    public void MinotaurDirection(Vector3 playerPos)
    {
        var currPos = this.transform.position; //Get minotaur current position.

        //If on different X-Axis
        if (currPos.x != playerPos.x) //floating point not an issue as moves are always integer
        {
            //Get closer horizontally
            if (currPos.x < playerPos.x)
            {
                _nextMove = new WalkRight();
            }
            else
            {
                _nextMove = new WalkLeft();
            }

            _nextDirection = _nextMove.Execute(); //Get direction
            
            var hitHorizontal = Physics2D.Raycast(new Vector2(currPos.x, currPos.y), _nextDirection, 1f, wallLayerMask ); //Check if there's a wall

            if (!hitHorizontal) // If there was not a collision.
            {
                MinotaurMove(_nextDirection,_nextMove); //Move the minotaur
                return; //Don't make any more moves.
            }
            
        }
        
        //If on different Y-Axis
        if (currPos.y != playerPos.y)
        {
            //Get closer vertically
            if (currPos.y < playerPos.y)
            {
                _nextMove = new WalkUp();
            }
            else
            {
                _nextMove = new WalkDown();
            }

            _nextDirection = _nextMove.Execute();//Get direction 
            
            var hitVertical = Physics2D.Raycast(new Vector2(currPos.x, currPos.y), _nextDirection, 1f, wallLayerMask ); //Check if there's a wall
            
            if (!hitVertical) // If there was not a collision.
            {
                MinotaurMove(_nextDirection,_nextMove); //Move the minotaur
                return; //Don't make any more moves.
            }
        }
        
        
        _minotaurMoves.Push(new Wait()); //If no moves were possible, wait.

    }

    private void MinotaurMove(Vector3 direction, ICommand nextMove) //Moves the minotaur
    {
        transform.position += new Vector3(direction.x,direction.y,0);
        _minotaurMoves.Push(nextMove);
    }

    public void MinotaurUndoTwo() //Undoes minotaur moves
    {
        var lastmove = - _minotaurMoves.Pop().Execute(); //Get opposite of last movement
        transform.position += new Vector3(lastmove.x,lastmove.y,0); 
        
        lastmove = - _minotaurMoves.Pop().Execute(); //Get opposite of last movement
        transform.position += new Vector3(lastmove.x,lastmove.y,0);
    }
}
