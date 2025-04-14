using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    private RPSMove playerMove;
    private RPSMove enemyMove;

    //all moves (0 ~ 12)
    public RPSMove rock;
    public RPSMove paper;
    public RPSMove scissors;
    public RPSMove battery;
    public RPSMove car;
    public RPSMove diamond;
    public RPSMove fire;
    public RPSMove fridge;
    public RPSMove money;
    public RPSMove person;
    public RPSMove pickaxe;
    public RPSMove road;
    public RPSMove water;
    private List<RPSMove> listOfMoves = new();

    //player gameobjects
    public GameObject player;
    public GameObject enemy;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI movesPicked;
    private int random1;
    private int random2;
    private int random3;
    private string result;

    //game
    private bool gameStarted;
    private bool gameEnded;
    private bool gameWon;
    private float timer = 1;

    //json
    public TextMeshProUGUI enemyNameText;
    private int randomType;
    private TypeData typeNum;
    private GameData gameData;
    private string enemyName;
    private int enemyRandomtMoveInt;
    private List<RPSMove> enemyRandomMoveList = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        listOfMoves = new List<RPSMove>{
            rock, paper, scissors, battery, car, diamond, fire, fridge, money, person, pickaxe, road, water
        };

        //json stuff here

        randomType = Random.Range(0,9);

        //read info from json
        TextAsset json = Resources.Load<TextAsset>("rps");
        if (json != null) {
            gameData = JsonUtility.FromJson<GameData>(json.text);
            
            if (gameData == null) {
                Debug.LogError("failed to parse GameData from json");
            }
        }

        else {
            Debug.LogError("rps.json doesnt exist");
        }
        
        //set enemy based on chosen type
        if (randomType == 0) {
            typeNum = gameData.type0;
        }
        else if (randomType == 1) {
            typeNum = gameData.type1;
        }
        else if (randomType == 2) {
            typeNum = gameData.type2;
        }
        else if (randomType == 3) {
            typeNum = gameData.type3;
        }
        else if (randomType == 4) {
            typeNum = gameData.type4;
        }
        else if (randomType == 5) {
            typeNum = gameData.type5;
        }
        else if (randomType == 6) {
            typeNum = gameData.type6;
        }
        else if (randomType == 7) {
            typeNum = gameData.type7;
        }
        else if (randomType == 8) {
            typeNum = gameData.type8;
        }

        if (typeNum == null) {
            Debug.LogError("this type doesnt exist");
        }
        
        enemyName = typeNum.name;
        enemyNameText.text = "(" + enemyName + ")";

        //get list of moves of this enemy
        for (int i = 0; i < typeNum.plays.Length; i ++){
            for (int j = 0; j < listOfMoves.Count; j ++){
                if (typeNum.plays[i].Equals(listOfMoves[j].moveName)){
                    enemyRandomMoveList.Add(listOfMoves[j]);
                }
            }
        }

        //randomize player and enemy moves
        RandomizeNumbers();
    }

    // Update is called once per frame
    void Update()
    {      
        
        if (gameStarted){
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            resultText.text = "";
            movesPicked.text = "";
        }
        
        if (!gameStarted){
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            timer += Time.deltaTime;
        }
        if (timer >= 1 && !gameEnded){
            gameStarted = true;
        }
        else if (timer >= 1 && gameEnded){

            //game actually ends
            if (gameWon){
                //win screen
                SceneManager.LoadScene("WinScreen");
            }
            else {
                //lose screen
                SceneManager.LoadScene("LoseScreen");
            }
        }

        if (enemy.transform.position.x >= 6){
            //player wins
            gameEnded = true;
            gameWon = true;
        }

        else if (player.transform.position.x <= -6){
            //enemy wins
            gameEnded = true;
            gameWon = false;
        }
    }

    public void CheckForWinner(){
        
        //how much winning/losing pushes you
        int winDistance = 1;

        //if neither win or lose, draw
        result = "DRAW!";

        //player wins
        for (int i = 0; i < playerMove.beats.Count; i ++){
            if (playerMove.beats[i] == enemyMove){
                result = "WIN!";
                player.transform.position = new Vector3(player.transform.position.x + winDistance, player.transform.position.y, player.transform.position.z);
                enemy.transform.position = new Vector3(enemy.transform.position.x + winDistance, enemy.transform.position.y, enemy.transform.position.z);
                break;
            }  
        }

        //enemy wins
        for (int i = 0; i < enemyMove.beats.Count; i ++){
            if (enemyMove.beats[i] == playerMove){
                result = "LOSE!";
                player.transform.position = new Vector3(player.transform.position.x - winDistance, player.transform.position.y, player.transform.position.z);
                enemy.transform.position = new Vector3(enemy.transform.position.x - winDistance, enemy.transform.position.y, enemy.transform.position.z);
                break;
            }
        }

        

        //change text
        resultText.text = result;
        movesPicked.text = playerMove.moveName + "   vs   " + enemyMove.moveName;
        
        timer = 0;
        gameStarted = false;
        RandomizeNumbers();
    }

    public void RandomizeNumbers(){

        bool valid = false;

        //randomize three ints
        while (!valid){

            random1 = Random.Range(0,13);
            random2 = Random.Range(0,13);
            random3 = Random.Range(0,13);

            if (random1 != random2 && random2 != random3 && random1 != random3){
                valid = true;
            }
        }

        //change button text
        button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listOfMoves[random1].moveName;
        button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listOfMoves[random2].moveName;
        button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listOfMoves[random3].moveName;


        //randomize enemy move
        enemyRandomtMoveInt = Random.Range(0,enemyRandomMoveList.Count);
        enemyMove = enemyRandomMoveList[enemyRandomtMoveInt];
    }

    public void Button1(){
        playerMove = listOfMoves[random1];
        CheckForWinner();
    }
    public void Button2(){
        playerMove = listOfMoves[random2];
        CheckForWinner();
    }
    public void Button3(){
        playerMove = listOfMoves[random3];
        CheckForWinner();
    }
}

//json stuff
[System.Serializable]
public class TypeData {
    public string name;
    public string[] plays;
}

[System.Serializable]
public class GameData {
    public TypeData type0;
    public TypeData type1;
    public TypeData type2;
    public TypeData type3;
    public TypeData type4;
    public TypeData type5;
    public TypeData type6;
    public TypeData type7;
    public TypeData type8;
}
