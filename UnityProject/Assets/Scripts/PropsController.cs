using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropsType {CardboardBox, PizzaSlice }

public class PropsController : MonoBehaviour
{
    [Header("Props Info")]
    public PropsType type;

    private void OnTriggerExit(Collider other)
    {
        if(type == PropsType.CardboardBox && other.gameObject.tag == "Player")
        {
            GameManager.instance.Respawn();
            other.enabled = false;
            Destroy(other.gameObject);
        }
    }

}
