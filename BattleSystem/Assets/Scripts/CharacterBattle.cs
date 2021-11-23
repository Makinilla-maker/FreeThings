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
    float health;
    public int healthMax;
    public float damage;
    public Transform healthBar;
    private enum State
    {
        Idel,
        Sliding,
        Busy,
    }
    void Start()
    {
        state= State.Idel;
        health = healthMax;
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
                float sliderSpeed = 7f;
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
            Debug.Log("attacked");
            Debug.Log(targetCharacterBattle.transform.position);
            SlideToPosition(targetCharacterBattle.transform.position,() =>{
                state = State.Busy;
                targetCharacterBattle.Damage(damage);
                targetCharacterBattle.healthBar.localScale = new Vector3(targetCharacterBattle.GetHealthPercent(),0,0);
                Debug.Log(targetCharacterBattle.health/targetCharacterBattle.healthMax);
                Debug.Log(targetCharacterBattle.health);
            SlideToPosition(startingPosition,()=>{
                    state = State.Idel;
                    onAttackComplete();
                });
            });
        }
        //state = State.Busy;
        //SlideToPosition(startingPosition, ()=>{});
    }
    private void SlideToPosition(Vector3 slidePosition, Action onSlideComplete)
    {
        this.sliderTargetPosition = slidePosition;
        Debug.Log(this.sliderTargetPosition);
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
    }
    public float GetHealthPercent()
    {
        return health/healthMax;
    }
    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if(health < 0)  health = 0;
    }
    public void Heal(int healAmount)
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
