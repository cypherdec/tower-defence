using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    [SerializeField] TextMeshProUGUI displayBalance;


    void Awake(){
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public int CurrentBalance { get { return currentBalance; } }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        if(currentBalance < 0){
            ReloadScene();
        }
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();
    }

    void ReloadScene(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void UpdateDisplay(){
        displayBalance.text = "Gold " + currentBalance;
    }
}
