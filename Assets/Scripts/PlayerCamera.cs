using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private GameObject bossPos;
    private Camera cameraComponent;
    public float smooth = 0.3f;
    public float height;
    [HideInInspector]
    public bool bossCam = false;


    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        cameraComponent = this.gameObject.GetComponent<Camera>();
        cameraComponent.fieldOfView = 140;
    }

    void Update()
    {
        if (cameraComponent.fieldOfView > 60)
        {
            cameraComponent.fieldOfView--;
        }
        if (!bossCam)
        {
            Vector3 pos = new Vector3();
            pos.x = playerTransform.position.x;
            pos.z = playerTransform.position.z;
            pos.y = playerTransform.position.y + height;
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
        }
        else
        {
            BossCam(bossPos);
        }
    }

    public void BossCam(GameObject newCamPos)
    {
        Vector3 pos = new Vector3();
        bossPos = newCamPos;
        bossCam = true;
        this.gameObject.transform.parent = null;
        pos.x = newCamPos.transform.position.x;
        pos.z = newCamPos.transform.position.z;
        pos.y = newCamPos.transform.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
    }
}
