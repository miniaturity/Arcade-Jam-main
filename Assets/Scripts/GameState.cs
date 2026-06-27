using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Static singleton instance allowing global access to the GameState
    public static GameState Instance { get; private set; }
    
    // Reference to the UI script handling player ready statuses and health visuals
    private ReadyView _readyView;
    
    // Ready state tracking for both players during the preparation phase
    private bool _playerOneReady = false;
    private bool _playerTwoReady = false;

    // Health each player starts (and restarts) every round with
    public int startingHealth = 3;
    
    // Remaining health points for each player in the current round
    public int playerOneHealth = 3;
    public int playerTwoHealth = 3;
    
    // Input axis and button string prefixes used to map multiplayer controls dynamically
    public string horizontalAxis = "Horizontal_";
    public string verticalAxis = "Vertical_";
    public string jumpButton = "Jump_";
    public string actionX = "Action_X_";
    public string actionB = "Action_B_";
    public string actionY = "Action_Y_";
    public string actionRB = "Action_RB_";
    public string actionLB = "Action_LB_";

    [SerializeField] private int roundNum = 1; // 1-indexed
    
    // Number of rounds each player has won so far
    public int player_1_score = 0, player_2_score = 0;
    
    // How many round wins are needed to win the whole match
    public int roundsToWinMatch = 5;
    
    private Card[] allCards =
    {
        
    };
    public Card[] cards { get; private set; }

    // Defines the possible operational states of the match flow
    public enum GameStateEnum {
        GetReady,
        InMatch,
        GameOver,
    }
    
    // Current active state of the game session
    public GameStateEnum gameState;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        // Enforce the Singleton pattern to prevent duplicate GameState instances
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Establish this object as the persistent global instance
        Instance = this;
        
        // Initialize the game loop into the pre-match ready phase
        gameState = GameStateEnum.GetReady;
        
        // Cache the ReadyView component responsible for user interface updates
        _readyView = gameObject.GetComponent<ReadyView>();
        
        // Make sure both players begin round 1 at full health
        playerOneHealth = startingHealth;
        playerTwoHealth = startingHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        // Monitor core match transitions based on current game phases
        switch (gameState) {
            
            // Check if both players are flagged as ready to begin gameplay
            case GameStateEnum.GetReady: {
                if (_playerOneReady && _playerTwoReady) {
                    // Transition to active match state
                    gameState = GameStateEnum.InMatch;
                    
                    // Notify the UI to hide lobby options and display game elements
                    _readyView.SetInMatch();
                    _readyView.UpdateRoundScore(player_1_score, player_2_score);
                }
                break;
            }
            
            // Monitor player health values to determine round/match termination conditions
            case GameStateEnum.InMatch: {
                if (playerOneHealth <= 0 || playerTwoHealth <= 0) {
                    // Whoever still has health left won this round
                    string roundWinner = playerOneHealth <= 0 ? "2" : "1";
                    EndRound(roundWinner);
                }
                break;
            }
            case GameStateEnum.GameOver:
            break; 
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Awards a round to roundWinner, then either ends the match (5 round wins)
    // or resets health and respawns both players to start the next round
    private void EndRound(string roundWinner) {
        if (roundWinner == "1") {
            player_1_score++;
        } else {
            player_2_score++;
        }
        
        roundNum++;

        bool matchWon = player_1_score >= roundsToWinMatch || player_2_score >= roundsToWinMatch;

        if (matchWon) {
            // Someone has reached 5 round wins - the match is over
            gameState = GameStateEnum.GameOver;
            _readyView.SetInGameOver(roundWinner, player_1_score, player_2_score);
            return;
        }

        // Not enough round wins yet - reset health and start the next round
        playerOneHealth = startingHealth;
        playerTwoHealth = startingHealth;
        _readyView.UpdatePlayerHealth("1", playerOneHealth);
        _readyView.UpdatePlayerHealth("2", playerTwoHealth);
        _readyView.UpdateRoundScore(player_1_score, player_2_score);

        RespawnAllPlayers();
    }

    // Moves every player back to their recorded starting position/velocity
    public void RespawnAllPlayers() {
        foreach (PlayerActions player in FindObjectsOfType<PlayerActions>()) {
            player.Respawn();
        }
    }

    // Deducts life from the specified player, respawns both players, and updates the UI.
    // Called whenever a player touches a "Death" hazard (e.g. a spike).
    public void TakeDamage(string player) {
        switch (player) {
            case "1": {
                playerOneHealth--;
                _readyView.UpdatePlayerHealth(player, playerOneHealth);
                break;
            }
            case "2": {
                playerTwoHealth--;
                _readyView.UpdatePlayerHealth(player, playerTwoHealth);
                break;
            }
        }
    }

    // Commits the ready status flag for a player and triggers the corresponding visual state
    public void SetReady(string player) {
        switch (player) {
            case "1": {
                _playerOneReady = true;
                _readyView.SetReady(player);
                break;
            }
            case "2": {
                _playerTwoReady = true;
                _readyView.SetReady(player);
                break;
            }
        }
    }

    public void SetCards(Card[] c)
    {
        if (c.Length != 3) {
            Debug.LogError("attempted to set cards with length not equal to 3.");
            return;
        }

        for (int i = 0; i < 3; i++) {
            Card card = c[i];

            
        }
    }
}