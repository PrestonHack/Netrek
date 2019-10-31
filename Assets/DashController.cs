using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class DashController : MonoBehaviour
{
 
    [SerializeField]
    private TMP_Text armys;
    [SerializeField]
    private TMP_Text weaponTemp;
    [SerializeField]
    private TMP_Text engineTemp;
    [SerializeField]
    private TMP_Text fuel;
    [SerializeField]
    private Image fuelImage;
    [SerializeField]
    private FuelController fuelController;
    [SerializeField]
    private TMP_Text speed;
    [SerializeField]
    private TMP_Text shield;
    [SerializeField]
    private Image shieldImage;
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
        hull.text = string.Format("Hu[{0}/{1}]",  Math.Round(hullController.hullHealth, 2), hullController.hullMaxHealth);
        hullImage.fillAmount = hullController.hullPercent;

        shield.text = string.Format("Sh[{0}/{1}]", Math.Round(hullController.shieldHealth, 2), hullController.shieldMaxHealth);
        shieldImage.fillAmount = hullController.shieldPercent;

        fuel.text = string.Format("Fu[{0}/{1}]", Math.Round(fuelController.currentFuel, 2), fuelController.maxFuel);
        fuelImage.fillAmount = fuelController.fuelPercent;
    }
}
