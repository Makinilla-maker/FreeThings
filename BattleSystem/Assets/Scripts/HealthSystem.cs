using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = health;
        health = healthMax;
    }
    public float GetHealthPercent()
    {
        return (float)health/healthMax;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if(health < 0)  health = 0;
    }
    public void Heal(int healAmount)
    {
        health += healAmount;
        if(health > healthMax) health = healthMax; 
    }
}
