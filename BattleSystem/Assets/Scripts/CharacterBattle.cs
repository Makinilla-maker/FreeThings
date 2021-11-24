using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBattle : MonoBehaviour
{
    // Start is called before the first frame update
    //private CharacterBase characterBase;
    private State state;
    private Vector3 sliderTargetPosition;
    Action onSlideComplete;
    [SerializeField] public float health;
    public int healthMax;
    public float damage;
    public float healAmount;
    private enum State
    {
        Idel,
        Sliding,
        Busy,
    }
    float sliderSpeed;
    void Start()
    {
        state= State.Idel;
        health = healthMax;
        sliderSpeed = 7f;
    }


    // Update is called once per frame
    void Update()
    {
        switch(state){
            case State.Idel:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                transform.position += ((sliderTargetPosition ) - transform.position)* sliderSpeed * Time.deltaTime;
                
                float reachedDistance = 1f;
                if(Vector3.Distance(transform.position,sliderTargetPosition) < reachedDistance)
                {
                    transform.position = sliderTargetPosition;
                    onSlideComplete();
                }
                break;
        }
        IsDead();
    }
   
    public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
    {
        if(health > 0)
        {
            Vector3 sliderTargetPosition = targetCharacterBattle.transform.position + (transform.position - targetCharacterBattle.transform.position).normalized * 10f;
            Vector3 startingPosition = transform.position;
            sliderSpeed = 7f;
            SlideToPosition(targetCharacterBattle.transform.position,() =>{
                state = State.Busy;
                targetCharacterBattle.Damage();
            SlideToPosition(startingPosition,()=>{
                    state = State.Idel;
                    onAttackComplete();
                });
            });
        }
        //state = State.Busy;
        //SlideToPosition(startingPosition, ()=>{});
    }
    public void Defense(Action onDefenceComplete)
    {
        if(health > 0)
        {
            Vector3 startingPosition = transform.position;
            Debug.Log("healed");
            sliderSpeed = 3f;
            SlideToPosition(transform.position + new Vector3(0,3,0),() =>{
                state = State.Busy;
                Heal();
            SlideToPosition(startingPosition,()=>{
                    state = State.Idel;
                    onDefenceComplete();
                });
            });
        }

    }
    private void SlideToPosition(Vector3 slidePosition, Action onSlideComplete)
    {
        this.sliderTargetPosition = slidePosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
    }
    public float GetHealthPercent()
    {
        return health/healthMax;
    }
    public void Damage()
    {
        health -= damage;
        if(health < 0)  health = 0;
    }
    public void Heal()
    {
        health += healAmount;
        if(health > healthMax) health = healthMax; 
    }
    public void IsDead()
    {
        if(health == 0)
        {
            this.enabled = false;
            Destroy(this.gameObject);
        }
    }
}
