
![GitTempSenseLogo](https://github.com/user-attachments/assets/2ca60ca1-3d5d-4b31-8015-e758023d119f)
# Temp ৹ Sense 
Open peltier based thermal haptics for Unity and VRChat.

TempSense is a project started at MIT Reality Hack 2025 which uses the ESP32 Platform and peltier modules to allow developers to integrate thermal haptics into their experiences, and for users to access a unique sensory experience when in VR.

Last update of this documentation: January 26th 2025.

# Project structure
Unity version 2022.3.22f1 URP
Meta Quest Presence Platform All in One SDK V71.
Singularity V0.04 Wifi
Scene: Assets/Scenes/SampleScene
Working APK built: Builds/V0.8.apk


# Communication protocol between ESP32 and Unity
ESP32 is always listening to Unity. Unity is broadcasing to ESP32 5Hz.
Information structure sent from Unity to ESP32:
`int int`
* first int: integer in range [-100, 100]. Left hand temperature control value. -100 is cold. 100 hot.
* second int: integer in range [-100, 100]. Right hand temperature control value. -100 is cold. 100 hot.
