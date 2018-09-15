using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level1Control : MonoBehaviour
{
    public string Descriptopn;

    private LevelControl LevelControl;

    private bool _isWin = false;

    // Use this for initialization
    void Start()
    {
        LevelControl = GetComponent<LevelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        var hero = LevelControl.GamobjList.First(dinfo => dinfo.Name == "myhero");
        var end = LevelControl.GamobjList.First(dinfo => dinfo.Name == "Final");
        if (hero && end && Vector3.Distance(hero.transform.position, end.transform.position) < 10)
        {
            Debug.Log("Arrive");
        }
    }
}