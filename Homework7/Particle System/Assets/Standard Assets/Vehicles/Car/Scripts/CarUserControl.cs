using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private bool isOn;          //To mark if the engine is on
        private bool startFirstTime;//To mark if the engine just start
        private bool carIsGood;     //To mark the condition of the car
        private float interval;     //To mark the time
        public ParticleSystem blackSmoke;   //The particle


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            isOn = false;
            carIsGood = true;
            startFirstTime = false;
            interval = 0;
            var blackEmission = blackSmoke.emission;    //Initialize the particle to not emitting
            blackEmission.rateOverTime = 0f;

        }


        private void FixedUpdate()
        {
            if(startFirstTime)                          //If the engine is just start
            {
				var blackEmission = blackSmoke.emission;
				blackEmission.rateOverTime = 100f;      //Emit a lot of smoke for 1s
                if(interval > 1.1)                          
                {
                    startFirstTime = false;
                    interval = 0;
                }
                else
                {
                    interval += Time.deltaTime;
                }
            }
            if(isOn)                                    
            {
				if(carIsGood && !startFirstTime)        //Normal condition
                {
					var blackEmission = blackSmoke.emission;
					blackEmission.rateOverTime = 30f;
                }
				else if(!carIsGood && !startFirstTime)  //The car braek down and smoke a lot
                {
                    var blackEmission = blackSmoke.emission;
                    blackEmission.rateOverTime = 100f;
                }
                // pass the input to the car!
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
            #if !MOBILE_INPUT
                float handbrake = CrossPlatformInputManager.GetAxis("Jump");
                m_Car.Move(h, v, v, handbrake);
            #else
            m_Car.Move(h, v, v, 0f);
            #endif
            }
            else
            {
                var blackEmission = blackSmoke.emission;
                blackEmission.rateOverTime = 0f;
            }
            
        }

        private void OnGUI()                            //Button
        {

            if (GUI.Button(new Rect(10, 10, 100, 50), "Start/Stop"))
            {
                if (isOn)
                {
                    isOn = false;
                }
                else
                {
                    isOn = true;
                    startFirstTime = true;
                }
            }
			if (GUI.Button(new Rect(10, 70, 100, 50), "Break/Repair"))
			{
				if (carIsGood)
				{
					carIsGood = false;
				}
				else
				{
					carIsGood = true;
				}
			}
        }
    }
}
