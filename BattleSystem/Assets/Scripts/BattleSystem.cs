using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform nicole;
    private CharacterBattle allyCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    public List<CharacterBattle> list;
    private CharacterBattle activeCharacterBattle;
    private State state;    private enum State {
        WaitingForPlayer,
        Busy,
    }
    bool buttonPressedAttack;
    bool buttonPressedDefense;
    void Start()
    {
        allyCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);
        SetActiveCharacterBattle(allyCharacterBattle);
        state = State.WaitingForPlayer; 
        buttonPressedAttack = false;
        buttonPressedDefense = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.WaitingForPlayer)
        {
            if(buttonPressedAttack)
            {
                state = State.Busy;
                allyCharacterBattle.Attack(enemyCharacterBattle, () => {
                    ChooseNextCharacter();
                });
                buttonPressedAttack = false;
                buttonPressedDefense = false;
                Debug.Log(allyCharacterBattle.health);
                Debug.Log(enemyCharacterBattle.health);
            }
            if(buttonPressedDefense)
            {
                state = State.Busy;
                allyCharacterBattle.Defense( () =>{
                    ChooseNextCharacter();
                });
                buttonPressedAttack = false;
                buttonPressedDefense = false;
                Debug.Log(allyCharacterBattle.health);
                Debug.Log(enemyCharacterBattle.health);
            }
            
        }
        
    }
    public void ButtonPressed(string type)
    {
        if(type == "attack")
        {
            if(buttonPressedAttack == true)
            {
                buttonPressedAttack = true;
            }
            else
            {
                buttonPressedAttack = true;
            }
        }
        else
        {
            if(buttonPressedDefense == true)
            {
                buttonPressedDefense = true;
            }
            else
            {
                buttonPressedDefense = true;
            }            
        }
        
    }
    
    private CharacterBattle SpawnCharacter(bool ally)
    {
        Vector3 posn;
        if(ally)
        {
            posn = new Vector3(7,1,-1);
        }
        else
        {
            posn = new Vector3(-2,1,-1);
        }
        Transform characterTransform = Instantiate(nicole, posn, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        //characterBattle.Setup(ally);

        return characterBattle;
    }
    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        activeCharacterBattle = characterBattle;
    }
    private void ChooseNextCharacter()
    {
        if(activeCharacterBattle == allyCharacterBattle)
        {
            SetActiveCharacterBattle(enemyCharacterBattle);
            state = State.Busy;
            if(enemyCharacterBattle.health > 35)
            {
                enemyCharacterBattle.Attack(allyCharacterBattle, () => {
                        ChooseNextCharacter();
                });
            }
            else
            {
                enemyCharacterBattle.Defense(() => {
                        ChooseNextCharacter();
                });
            }
        }
        else
        {
            SetActiveCharacterBattle(allyCharacterBattle);
            state = State.WaitingForPlayer;

        }
    }
}
