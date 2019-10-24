using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas loginCanvas;
    [SerializeField]
    private Canvas teamCanvas;
    [SerializeField]
    private Canvas statusCanvas;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(loginCanvas);
        DontDestroyOnLoad(teamCanvas);
        DontDestroyOnLoad(statusCanvas);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
