using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SecretScript : MonoBehaviour
{
    int total = 0;
    public Button Level11, Level12;
    // Update is called once per frame
    void Update()
    {
        //testing for the correct keys to be entered by the user
        if (Input.GetKey("r"))//first correct letter
        {
            total = 1;//increments total by 1
        }
        if (total == 1 && Input.GetKey("d"))
        {
            total = 2;
        }
        if (total == 2 && Input.GetKey("u"))
        {
            total = 3;
        }
        if (total == 3 && Input.GetKey("u"))
        {
            total = 4;
        }
        if (total == 4 && Input.GetKey("e"))
        {
            total = 5;
        }
        if (total == 5 && Input.GetKey("o"))
        {
            total = 6;
        }
        if (total == 6 && Input.GetKey("y"))
        {
            total = 7;
        }
        if (total == 7 && Input.GetKey("i"))
        {
            total = 8;
        }
        if (total == 8 && Input.GetKey("e"))
        {
            total = 9;
        }
        if (total == 9 && Input.GetKey("a"))
        {
            total = 10;
        }
        if (total == 10 && Input.GetKey("o"))
        {
            total = 11;
        }
        if (total == 11 && Input.GetKey("z"))
        {
            total = 12;
        }
        if (total == 12 && Input.GetKey("z"))
        {
            total = 13;
        }
        if (total == 13 && Input.GetKey("v"))
        {
            total = 14;
        }
        if (total == 14 && Input.GetKey("h"))
        {
            total = 15;
        }


        if (total == 10)//if the user has found the first 10 secret letters
        {
            Level11.gameObject.SetActive(true);//enabels level 11
        }
        if (total == 15)//if the user found the 5 hidden letters on level 11
        {
            Level12.gameObject.SetActive(true);//enables level 12
        }


        if (total < 10)
        {
            Level11.gameObject.SetActive(false);//disables 11 if user does not have enough letters inputed
        }
        if (total < 15)
        {
            Level12.gameObject.SetActive(false);//same as above for level 12
        }

        if (total < 10) //if the user has not unlocked either secret level
        {
            if (Input.GetKey("b") || Input.GetKey("c") || Input.GetKey("f") || Input.GetKey("g") || Input.GetKey("j") || Input.GetKey("k") || Input.GetKey("l") || Input.GetKey("m") || Input.GetKey("n") || Input.GetKey("p") || Input.GetKey("q") || Input.GetKey("s") || Input.GetKey("t") || Input.GetKey("w") || Input.GetKey("x"))
            {
                total = 0;//if user inputs a letter that is not in the secret code, it will reset the counter
            }
        }
        if (total > 10 && total <15)//if user has unlock 11 but not 12
        {
            if (Input.GetKey("b") || Input.GetKey("c") || Input.GetKey("f") || Input.GetKey("g") || Input.GetKey("j") || Input.GetKey("k") || Input.GetKey("l") || Input.GetKey("m") || Input.GetKey("n") || Input.GetKey("p") || Input.GetKey("q") || Input.GetKey("s") || Input.GetKey("t") || Input.GetKey("w") || Input.GetKey("x"))
            {//if user inputs a letter that is not in the secret code, it will reset the counter to 10 so that 11 is still unlocked.
                total = 10;
            }
        }

    }
}
