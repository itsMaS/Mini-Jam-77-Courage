using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    [SerializeField] GameObject MiningSymbol;

    void Update()
    {
        MiningSymbol.SetActive(PlayerController.Instance.isMining);
    }
}
