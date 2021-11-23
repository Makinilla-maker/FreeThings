using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform nicole;
    private CharacterBattle allyCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;
    private enum State {
        WaitingForPlayer,
        Busy,
    }
    void Start()
    {
        allyCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);    

        SetActiveCharacterBattle(allyCharacterBattle);
        state = State.WaitingForPlayer; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.WaitingForPlayer)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Busy;
                allyCharacterBattle.Attack(enemyCharacterBattle, () => {
                    ChooseNextCharacter();
                });
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
            enemyCharacterBattle.Attack(allyCharacterBattle, () => {
                    ChooseNextCharacter();
                });
        }
        else
        {
            SetActiveCharacterBattle(allyCharacterBattle);
            state = State.WaitingForPlayer;

        }
    }
}
