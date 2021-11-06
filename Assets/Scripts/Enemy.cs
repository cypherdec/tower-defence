using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    Bank bank;
    
    void Start()
    {
        bank = FindObjectOfType<Bank>();
    }

    public void Die(){
        if(bank == null){return;}
        bank.Deposit(goldReward);
    }

    public void Steal(){
        if(bank == null){return;}
        bank.Withdraw(goldPenalty);
    }
}
