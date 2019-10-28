using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DashController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text fuel;
    [SerializeField]
    private TMP_Text armys;
    [SerializeField]
    private TMP_Text weaponTemp;
    [SerializeField]
    private TMP_Text engineTemp;
    [SerializeField]
    private TMP_Text speed;
    [SerializeField]
    private TMP_Text shield;
    [SerializeField]
    private TMP_Text hull;
    [SerializeField]
    private Image hullImage;
    [SerializeField]
    private HullController hullController;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {        
        hull.text = string.Format("Hu[{0}/{1}]", hullController.hullHealth, hullController.maxHealth);
        hullImage.fillAmount = hullController.healthPercent;
    }
}
