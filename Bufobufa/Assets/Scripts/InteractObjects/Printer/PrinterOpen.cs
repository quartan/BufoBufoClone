using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterOpen : MonoBehaviour
{
    public bool InTrigger = false;
    public bool PrinterIsOpen = false;
    private bool PrinterAnim = false;
    private bool ClickedMouse = false;

    private GameObject Player;
    private GameObject Vcam;
    private GameObject TriggerPrinter;

    [Header("���������� ���� ������ ���� ������ ��� �������� �����(����� � ������)")]
    public Vector3 CoordPlayer = new();
    public float TimeAnimationPlayer = 1f;
    public Vector3 CoordVcam = new();
    public Quaternion RotateVcam = new();
    public float TimeAnimationVcam = 1f;

    private Vector3 currentPos = new();


    private void Start()
    {
        Vcam = GameObject.FindGameObjectWithTag("Vcam");
        Player = GameObject.FindGameObjectWithTag("Player");
        TriggerPrinter = transform.Find("TriggerPrinter").gameObject;
    }
    public void OnTrigEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InTrigger = true;
        }
    }
    public void OnTrigExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InTrigger = false;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var infoHit, Mathf.Infinity, LayerMask.GetMask("Floor", "ClickedObject")))
            {
                if (infoHit.collider.gameObject == gameObject)
                {
                    ClickedMouse = true;
                    TriggerPrinter.SetActive(true);
                }
                else
                {
                    ClickedMouse = false;
                    TriggerPrinter.SetActive(false);
                }
            }
        }


        if (!Player.GetComponent<PlayerInfo>().PlayerPickSometing && !PrinterAnim && InTrigger && ClickedMouse && !PrinterIsOpen)
        {

            ClickedMouse = false;
            Vcam.GetComponent<CinemachineVirtualCamera>().Follow = null;
            Vcam.GetComponent<MoveAnimation>().startCoords = CoordVcam;
            Vcam.GetComponent<MoveAnimation>().needPosition = true;
            Vcam.GetComponent<MoveAnimation>().startRotate = RotateVcam;
            Vcam.GetComponent<MoveAnimation>().needRotate = true;
            Vcam.GetComponent<MoveAnimation>().TimeAnimation = TimeAnimationVcam;
            Vcam.GetComponent<MoveAnimation>().StartMove();


            currentPos = Player.transform.position;
            Player.GetComponent<PlayerMouseMove>().MovePlayer(CoordPlayer);
            Player.GetComponent<PlayerMouseMove>().StopPlayerMove();



            PrinterIsOpen = true;
            Player.GetComponent<PlayerInfo>().PlayerInSomething = true;
            PrinterAnim = true;
            StartCoroutine(WaitAnimTable(Vcam.GetComponent<MoveAnimation>().TimeAnimation + 0.1f));
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (!PrinterAnim && PrinterIsOpen && Input.GetMouseButtonDown(1))
        {
            TriggerPrinter.SetActive(false);
            PrinterIsOpen = false;
            Player.GetComponent<PlayerInfo>().PlayerInSomething = false;
            PrinterAnim = true;
            ClickedMouse = false;
            Vcam.GetComponent<MoveAnimation>().EndMove();
            StartCoroutine(WaitAnimTable(Vcam.GetComponent<MoveAnimation>().TimeAnimation + 0.1f));
            StartCoroutine(WaitAnimCamera(Vcam.GetComponent<MoveAnimation>().TimeAnimation + 0.1f));

            Player.GetComponent<PlayerMouseMove>().MovePlayer(currentPos);
            Player.GetComponent<PlayerMouseMove>().ReturnPlayerMove();


            GetComponent<BoxCollider>().enabled = true;
        }
    }
    IEnumerator WaitAnimTable(float f)
    {
        yield return new WaitForSeconds(f);
        PrinterAnim = false;
    }
    IEnumerator WaitAnimCamera(float f)
    {
        yield return new WaitForSeconds(f);
        Vcam.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
    }
}
