using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class NPCGeneral : MonoBehaviour
{
    public string[] dialogs;
    public float talkingDistance = 1f;

    GameObject player;
    public TMP_Text talkText;

    bool isTalk = false;
    int index = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //always look at player
        transform.LookAt(player.transform);

        //talking with player
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position,transform.position) < talkingDistance)
        {
            if (!isTalk)
            {
                isTalk = true;
                player.GetComponent<PlayerMovement>().canMove = false;
            }
            if (index >= dialogs.Length)
            {
                isTalk = false;
                player.GetComponent<PlayerMovement>().canMove = true;
                index = 0;
                talkText.text = "";
            }
            if (isTalk)
            {
                talkText.text = dialogs[index];
                index++;
            }
        }
    }
}
