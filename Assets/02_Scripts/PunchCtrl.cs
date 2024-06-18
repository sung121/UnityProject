using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float power = 0f;
    private float playerPower;
    private float tmp = 0;


    void Start()
    {
        playerPower = GetComponentInParent<PlayerCtrl>().currentPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            other.gameObject.GetComponent<MonsterCtrl>().currentHp -= power + playerPower;
            Debug.Log("Punch Trigger!" + ++tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
