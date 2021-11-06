using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField] int goldCost = 75;
    [SerializeField] int buildDelay = 1;

    void Start()
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 mousePos)
    {
        Bank bank = FindObjectOfType<Bank>();
        if (bank == null) { return false; }

        if (bank.CurrentBalance >= goldCost)
        {
            Instantiate(tower, mousePos, Quaternion.identity);
            bank.Withdraw(goldCost);
            return true;
        }

        return false;
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }


        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);

            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }
}
