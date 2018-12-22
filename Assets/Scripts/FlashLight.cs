using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {

    //public float battery;
    private float intensityFactor;
    protected Light light, sun;
    //private Settings settings;

    private void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
        //intensityFactor = light.intensity/battery; //LIKE THE VELOCITY'S FORMULA
        //settings = GameObject.Find("SettingsPanel").GetComponent<Settings>();
        //sun.intensity = settings.l_intensity;

    }
    void Update()
    {

        if (Input.GetButtonDown("FlashLight"))
        {
            if(light.enabled==false)
            light.enabled = true;
            else light.enabled = false;
        }
            //SLENDERMAN FLASHLIGHT
        /*if (settings.settingsOpen == true)
        {
            light.enabled=false;
            return;
        }*/

            /*battery = battery - Time.deltaTime; //WE CONVERT THE BATTERY VARIABLE INTO A COUNTDOWN FOR EVERY SECOND THAT IT IS TURN ON THE FLASHLIGHT
            if (light.enabled == true)
            {
                if (battery > 0)
                {
                    light.intensity = intensityFactor*battery; //SO NOW, WE CAN MODIFY TO ANY TIME WE WANT THE FLASHLIGHT'S BATTERY TO LONG ENOUGH WITH
                                                                            // WITH THE INTENSITY FROM THE SPOTLIGHT
                }
            }

            if (Input.GetButtonDown("FlashLight"))
            {
                if (light.enabled == false)
                {
                    if (battery > 0) //IF WE STILL HAVE BATTERY
                    {
                        light.enabled = true;                 
                    }
                }

                else

                { //TURN OFF THE LAMP SO WE CAN SAVE BATTERY FOR LATER
                    light.enabled = false;
                }
            }

            if (battery < 0)
            {
                battery = 0; //BATTERY'S DEAD
                GetComponent<Light>().enabled = false;
            }*/
    }

    public void GameOver()
    {
        light.intensity = 0;
    }

}
 
