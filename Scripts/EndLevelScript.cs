using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    public Transform endCheck;
    public LayerMask endMask;
    public Button nextLevel;
    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(endCheck.position, .4f, endMask))
        {
            nextLevel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            nextLevel.gameObject.SetActive(false);
        }

        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
