
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace AirDemo
{

    public partial class Form1 : Form
    {
        int ACMode = 0;     //   0 off // 1 cooling // 2 heating
        int PrevACMode = 1; // varible to know if there has nay change of the mode of AC stars with 1 to run it at the beginning for once.

        int ACFanMode = 0;   // 0 off // 1 2 3 4 5
        int PrevACFanMode = 1;// varible to know if there has nay change of the mode of ACFan

        int AirPurifierMode = 0; // 0 off // 1 2 3
        int PrevAirPurifierMode =1;

        int HumidifierMode = 0;
        int PrevHumidifierMode = 1;


        int GeoMode = 0; // 0 At Home  // 1 Leaving Home  //  2 Coming  Home 
        int InsetTemp = 22; // Indoor SetTemperature Variable
        int Set_IAQ_Level = 24; //  Indoor Air Quality Variable

        // int InsetHumidity = 25;
        int OffsetTempAtHome = 1;  //  Set Temperature Offset Variable for AtHome
        int OffsetTempComing = 3;
        int OffsetTempLeaving = 8;


        int OffsetIAQ_1 = 2;  //  Set IAQ Offset Variable to run Purifier in 5 modes
        int OffsetIAQ_2 = 4;
        int OffsetIAQ_3 = 6;
        int OffsetIAQ_4 = 8;
        int OffsetIAQ_5 = 10;

        int OffsetHumidity_1 = 2;  //  Set Humidity  Offset Variable to run Dehumidifier in 3 modes
        int OffsetHumidity_2 = 6;
        int OffsetHumidity_3 = 10;

        int IAQ_Level = 24;
        int InTemp = 18;
        int OutTemp = 12;
        int InHumidity = 35;
  

        public Form1()
        {
            InitializeComponent();

            // fill all default numeric button values from default varibles
            numericUpDown_InTemp.Value = InTemp;
            numericUpDown_OutTemp.Value = OutTemp;
            numericUpDown_InHumidity.Value = InHumidity;
            numericUpDown_InSetTemp.Value = InsetTemp;
            numericUpDown_IAQ.Value = IAQ_Level;
 
            // call button at home for default starting
            button_AtHome_Click(null, null);

        }

        // if  button Coming home pressed trigger Gemode to 2 for Cominghome
        private void button_Coming_Home_Click(object sender, EventArgs e)
        {
            button_Coming_Home.BackColor = Color.DarkOrange;
            button_LeavingHome.BackColor = Color.DarkSeaGreen;
            button_AtHome.BackColor = Color.DarkSeaGreen;
            GeoMode = 2;
        }

        // if  button at home pressed trigger Gemode to 0 for Athome
        private void button_AtHome_Click(object sender, EventArgs e)
        {
            button_Coming_Home.BackColor = Color.DarkSeaGreen;
            button_LeavingHome.BackColor = Color.DarkSeaGreen;
            button_AtHome.BackColor = Color.DarkOrange;
            GeoMode = 0;
        }

        // if  button Leaving home pressed trigger Gemode to 1 for Leavinghome
        private void button_LeavingHome_Click(object sender, EventArgs e)
        {
            button_Coming_Home.BackColor = Color.DarkSeaGreen;
            button_LeavingHome.BackColor = Color.DarkOrange;
            button_AtHome.BackColor = Color.DarkSeaGreen;
            GeoMode = 1;
            GeoMode = 1;
            GeoMode = 1;



        }




        // 20 ms interval timer function... at 20 ms this function works as a loop 
        // in this loop all logic works, comparision with set values and measured(simulated) values are compared
        // and related device is operated to on/off or position
        // in the loop related visual components and properties are only updated if there is any change in the position of the device like Ac off to cool
        // for example color of the ac button is only updated when mode of AC goes cool to off otherwise it flickers
        private void timer1_Tick(object sender, EventArgs e)
        {

            textBox1.Text = "PrevACMode" + PrevACMode.ToString() + Environment.NewLine + "ACMode" + ACMode.ToString() + Environment.NewLine;
            textBox1.Text += "GeoMode" + GeoMode.ToString() + Environment.NewLine + "ACFanMode" + ACFanMode.ToString() + Environment.NewLine;
            textBox1.Text += "AirPurifierMode" + AirPurifierMode.ToString() + Environment.NewLine + "HumidifierMode" + HumidifierMode.ToString() + Environment.NewLine;
            // above 3 lines are for debugging dont care

            // below switch case set AC and fan into the position according to modes and temperature settings
            switch (GeoMode) // three main operating modes 0=athome 1=Leaving Home 2= Coming Home
            {   
                case 0: //At Home
                    if (InsetTemp > (InTemp + OffsetTempAtHome)) // heat
                    {
                        ACMode = 2; // heating
                        ACFanMode = 3;
                    }
                    else if (InsetTemp < (InTemp - OffsetTempAtHome)) // cool
                    {
                        ACMode = 1; // cooling
                        ACFanMode = 3;
                    }
                    else // default off
                    {
                        ACMode = 0; // off
                        ACFanMode = 0;
                    }                  
                    break;
                case 1: //Leaving Home

                    if (InsetTemp > (InTemp + OffsetTempLeaving)) // heat
                    {
                        ACMode = 2; // heating
                        ACFanMode = 1;
                    }
                    else if (InsetTemp < (InTemp - OffsetTempLeaving)) // cool
                    {
                        ACMode = 1; // cooling
                        ACFanMode = 1;
                    }
                    else
                    {
                        ACMode = 0; // off
                        ACFanMode = 0;
                    }
                    break;
                case 2: // Coming Home
                    if (InsetTemp > (InTemp+ OffsetTempComing)) // heat
                    {
                        ACMode = 2; // heating
                        ACFanMode = 2;
                    }
                    else if (InsetTemp < (InTemp - OffsetTempComing)) // cool
                    {
                        ACMode = 1; // cooling
                        ACFanMode = 2;
                    }
                    else
                    {
                        ACMode = 0; // off
                        ACFanMode = 0;
                    }
                   break;
                default: break;
            }
            // below switch case set AirPurifie and fan into the position according to modes and AirQuaity settings and Humidifier according to Humidity measured
            switch (GeoMode) // fan + AirPurifier
            {
                case 0: //At Home
                     if ((Set_IAQ_Level + OffsetIAQ_5) < IAQ_Level) // // use offsets 
                    {
                        AirPurifierMode = 3; // 
                        ACFanMode = 3;

                    }
                    else  if ((Set_IAQ_Level + OffsetIAQ_4) < IAQ_Level) // 
                    {
                        AirPurifierMode = 3; // 
                        ACFanMode = 3;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_3) < IAQ_Level) // 
                    {
                        AirPurifierMode = 2; // 
                        ACFanMode = 2;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_2) < IAQ_Level) // 
                    {
                        AirPurifierMode = 2; // 
                        ACFanMode = 1;
                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_1) < IAQ_Level) // 
                    {
                        AirPurifierMode = 1; // 
                        ACFanMode = 1;

                    }
                    else // default off
                    {
                        AirPurifierMode = 0; // off
                        if (ACMode == 0) ACFanMode = 0;
                        else ACFanMode = 1;  // iff cool or heat 
                    }


                    if ((50 - OffsetHumidity_3) > InHumidity)
                    {
                        HumidifierMode = 3;
                    }
                    else if ((50 - OffsetHumidity_2) > InHumidity)
                    {
                        HumidifierMode = 2;
                    }
                    else if ((50 - OffsetHumidity_1) > InHumidity)
                    {
                        HumidifierMode = 1;
                    }
                    else
                    {
                        HumidifierMode = 0;
                    }

                    break;
            
                  

                case 1: ////Leaving Home
                    if ((Set_IAQ_Level + OffsetIAQ_5) < IAQ_Level) // 
                    {
                        AirPurifierMode = 2; // 
                        ACFanMode = 1;

                    }
                    else if((Set_IAQ_Level + OffsetIAQ_4) < IAQ_Level) // 
                    {
                        AirPurifierMode = 1; // 
                        ACFanMode = 0;

                    }
                    else if((Set_IAQ_Level + OffsetIAQ_3) < IAQ_Level) // 
                    {
                        AirPurifierMode = 0; // 
                        ACFanMode = 0;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_2) < IAQ_Level) // 
                    {
                        AirPurifierMode = 0; // 
                        ACFanMode = 0;
                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_1) < IAQ_Level) // 
                    {
                        AirPurifierMode = 0; // 
                        if (ACMode == 0) ACFanMode = 0;
                        else ACFanMode = 1;  // iff cool or heat 

                    }
                    else // default off
                    {
                        AirPurifierMode = 0; // off
                        if (ACMode == 0) ACFanMode = 0;
                        else ACFanMode = 1;  // iff cool or heat 
                    }
                    if ((50 - OffsetHumidity_3) > InHumidity)
                    {
                        HumidifierMode = 1;
                    }
                    else if ((50 - OffsetHumidity_2) > InHumidity)
                    {
                        HumidifierMode = 0;
                    }
                    else if ((50 - OffsetHumidity_1) > InHumidity)
                    {
                        HumidifierMode = 0;
                    }
                    else
                    {
                        HumidifierMode = 0;
                    }

                    
                    break;
                   

                case 2:  // // Coming Home

                    if ((Set_IAQ_Level + OffsetIAQ_5) < IAQ_Level) // 
                    {
                        AirPurifierMode = 3; // 
                        ACFanMode = 2;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_4) < IAQ_Level) // 
                    {
                        AirPurifierMode = 2; // 
                        ACFanMode = 2;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_3) < IAQ_Level) // 
                    {
                        AirPurifierMode = 1; // 
                        ACFanMode = 1;

                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_2) < IAQ_Level) // 
                    {
                        AirPurifierMode = 1; // 
                        ACFanMode = 1;
                    }
                    else if ((Set_IAQ_Level + OffsetIAQ_1) < IAQ_Level) // 
                    {
                        if (ACMode == 0) ACFanMode = 0;
                        else ACFanMode = 1;  // iff cool or heat 
                      //  ACFanMode = 0;
                    }
                    else // default off
                    {
                        if(ACMode == 0) ACFanMode = 0;
                        else ACFanMode = 1;  // iff cool or heat 
                        AirPurifierMode = 0; // off
                        
                    }
                    
                    if ((50 - OffsetHumidity_3) >InHumidity)
                    {
                        HumidifierMode = 2;
                    }
                    else if ((50 - OffsetHumidity_2) > InHumidity)
                    {
                        HumidifierMode = 1;
                    }
                    else if ((50 - OffsetHumidity_1) > InHumidity)
                    {
                        HumidifierMode = 0;
                    }
                    else
                    {
                        HumidifierMode = 0;
                    }
                    break;
                default:
                    break;
            }
            // below code changes the appearance screen if there is any change in the position of AC Fan
            // keep in mind that if code is invoked so fast screen flashes so in order to prevent flash
            // it is callled once there is any position change
            //  
            if (PrevACFanMode != ACFanMode)
            {
                PrevACFanMode = ACFanMode;


                switch (ACFanMode) // Fan
                {
                    case 0: //
                        button_PACFan.Text = "Fan Off";
                        break;
                    case 1: // 
                    //    button_PACFan.Text = "Fan Level:" + ACFanMode.ToString();
                        button_PACFan.Text = "Fan Level:" + "Low";
                        break;
                    case 2:  // 
                             //     button_PACFan.Text = "Fan Level:" + ACFanMode.ToString();
                        button_PACFan.Text = "Fan Level:" + "Mid";
                        break;
                    case 3: // 
                     //   button_PACFan.Text = "Fan Level:" + ACFanMode.ToString();
                        button_PACFan.Text = "Fan Level:" + "High";
                        break;
                    default:
                        break;

                }
            }
            // below code changes the appearance screen if there is any change in the position of AC 
            if (PrevACMode != ACMode)
            {
                PrevACMode = ACMode;
                switch (ACMode)
                {
                    case 1: //cooling
                        button_PAC.BackColor = Color.LightBlue;
                        button_PAC.Text = "AC Cooling";
                        break;

                    case 2: // heating
                        button_PAC.BackColor = Color.LightYellow;
                        button_PAC.Text = "AC Heating";
                        break;

                    case 0:  // off
                        button_PAC.BackColor = Color.DarkSeaGreen; // default
                        button_PAC.Text = "AC Off";
                        break;

                    default:
                        break;

                }
                
            }
            // below code changes the appearance screen if there is any change in the position of Purifier
            if (PrevAirPurifierMode != AirPurifierMode)
            {
                PrevAirPurifierMode = AirPurifierMode;
                switch (AirPurifierMode)
                {
                    case 0: // off
                        button_AirPrifier.BackColor = Color.LightGreen;
                        button_AirPrifier.Text = "Purifier : Off";
                        break;

                    case 1: // 
                        button_AirPrifier.BackColor = Color.LightBlue;
                        //         button_AirPrifier.Text = "Purifier Level :" + AirPurifierMode.ToString();
                                 button_AirPrifier.Text = "Purifier Level :" + "Low";

                        break;

                    case 2:  // 
                        button_AirPrifier.BackColor = Color.LightPink; // default
                          //          button_AirPrifier.Text = "Purifier Level :" + AirPurifierMode.ToString();
                        button_AirPrifier.Text = "Purifier Level :" + "Mid";


                        break;
                    case 3:  // 
                        button_AirPrifier.BackColor = Color.OrangeRed; // default
        //              button_AirPrifier.Text = "Purifier Level :" + AirPurifierMode.ToString();
                        button_AirPrifier.Text = "Purifier Level :" + "High";

                        break;




                    default:
                        break;

                }
            }

            // below code changes the appearance screen if there is any change in the position of Humidifier
            if (PrevHumidifierMode != HumidifierMode)
            {
                PrevHumidifierMode = HumidifierMode;
                switch (HumidifierMode)
                {
                    case 0: // off
                        button_Dehumidifier.BackColor = Color.LightGreen;
                        button_Dehumidifier.Text = "Humidifier : Off";
                        break;
                    case 1: // 
                        button_Dehumidifier.BackColor = Color.LightBlue;
                        //        button_Dehumidifier.Text = "Humidifier Level :" + HumidifierMode.ToString();
                        button_Dehumidifier.Text = "Humidifier Level :" + "Low";
                        break;

                    case 2:  // 
                        button_Dehumidifier.BackColor = Color.LightPink; // default
                    //          button_Dehumidifier.Text = "Humidifier Level :" + HumidifierMode.ToString();
                        button_Dehumidifier.Text = "Humidifier Level :" + "Mid";
                        break;
                    case 3:  // 
                        button_Dehumidifier.BackColor = Color.OrangeRed; // default
                    //             button_Dehumidifier.Text = "Humidifier Level :" + HumidifierMode.ToString();
                        button_Dehumidifier.Text = "Humidifier Level :" + "High";
                        break;
                    default:
                        break;
                }
            }

        }

       // if numeric button value of set temperature has changed also change the variable 
        private void numericUpDown_InSetTemp_ValueChanged(object sender, EventArgs e)
        {
               InsetTemp = Convert.ToInt32(numericUpDown_InSetTemp.Value);
        }
        // if numeric button value of indoor  temperature has changed also change the variable 
        private void numericUpDown_InTemp_ValueChanged(object sender, EventArgs e)
        {
            InTemp = Convert.ToInt32(numericUpDown_InTemp.Value);
        }

        // if numeric button value of outdoor temperature has changed also change the variable 
        private void numericUpDown_OutTemp_ValueChanged(object sender, EventArgs e)
        {
            OutTemp = Convert.ToInt32(numericUpDown_OutTemp.Value);
        }
        // if numeric button value of humidity has changed also change the variable 
        private void numericUpDown_InHumidity_ValueChanged(object sender, EventArgs e)
        {
            InHumidity = Convert.ToInt32(numericUpDown_InHumidity.Value);
        }
        // if numeric button value of indoor air quality has changed also change the variable 
        private void numericUpDown_IAQ_ValueChanged(object sender, EventArgs e)
        {
            IAQ_Level =  Convert.ToInt32(numericUpDown_IAQ.Value);
        }
    }
}
