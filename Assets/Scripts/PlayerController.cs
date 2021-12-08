using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Stack<ICommand> _playerCommands; // Stores past moves by the player
    private ICommand _nextCommand;
    private Vector2 _nextDir;
    private Boolean _gameOver;
    private Boolean _victory;

    public MinotaurMovement minotaurMovement;
    public LayerMask wallLayerMask;
    public GameObject gameOverManager;
    public GameObject youWonManager;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Minotaur") && !_victory)
        {
            _gameOver = true;
            gameOverManager.SetActive(true);
        }
        
        if (other.gameObject.tag.Equals("Finish"))
        {
            _victory = true;
            _gameOver = true;
            youWonManager.SetActive(true);
        }
    }

    void Start()
    {
        _playerCommands = new Stack<ICommand>();
        _nextCommand = null;
        _gameOver = false;
        _victory = false;
    }
 
    void Update()
    {
        _nextCommand = null;
        
        if (Input.GetKeyDown(KeyCode.W) && !_gameOver)
        {
            _nextCommand = new WalkUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) && !_gameOver)
        {
            _nextCommand = new WalkDown();
        }
        else if (Input.GetKeyDown(KeyCode.A) && !_gameOver)
        {
            _nextCommand = new WalkLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) && !_gameOver)
        {
            _nextCommand = new WalkRight();
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Press R to restart Level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_victory)//If player won and pressed space, load next level
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else// If player just pressed space, next command is the wait action
            {
                _nextCommand = new Wait();
            }
        }
        else if(Input.GetKeyDown(KeyCode.U)) //Undo action
        {
            if (_playerCommands.Count != 0)
            {
                gameOverManager.SetActive(false);
                _gameOver = false;
                _nextDir = -_playerCommands.Pop().Execute();
                minotaurMovement.MinotaurUndoTwo();
                this.transform.position += new Vector3(_nextDir.x,_nextDir.y,0);
                return;
            }
            
        }

        if (_nextCommand == null) return; // If there wasn't a command, continue.
        
        _nextDir = _nextCommand.Execute(); //Get direction
        var position = transform.position; //Get position
            
        var hit = Physics2D.Raycast(new Vector2(position.x, position.y), _nextDir, 1f, wallLayerMask ); //Checks for walls

        if (hit) return; // If there's a hit, do nothing.
        
        _playerCommands.Push(_nextCommand); //Adds command to command stack

        this.transform.position += new Vector3(_nextDir.x,_nextDir.y,0);
        position = transform.position; //Get NEW position
        minotaurMovement.MinotaurDirection(position);
        minotaurMovement.MinotaurDirection(position);

    }
}
