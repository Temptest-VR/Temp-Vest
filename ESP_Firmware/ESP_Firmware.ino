#include <Arduino.h>
#include <WiFi.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
#define OLED_RESET -1  // Reset pin (not used)
#define OLED_ADDR 0x3C  // I2C address for OLED

#define SDA_PIN 22
#define SCL_PIN 21

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

const char* ssid = "TempVest";      // Replace with your WiFi SSID
const char* password = "12345678";  // Replace with your WiFi password

WiFiServer server(80);

const int peltier_1_pin = 2;
const int peltier_2_pin = 15;
const int peltier_3_pin = 0;
const int peltier_4_pin = 4;
const int peltier_5_pin = 16;
const int peltier_6_pin = 17;

const int peltier_1_reverse = 5;
const int peltier_2_reverse = 18;
const int peltier_3_reverse = 19;
const int peltier_4_reverse = 12;
const int peltier_5_reverse = 27;
const int peltier_6_reverse = 26;

const int PWM_FREQ = 20;
const int PWM_RESOLUTION = 8;
const int max_power = 255; // Assuming the power range is 0-255

int loops = 0;


const int therm1_pin = 33;
int therm1_value = 0;


void setup() {
    Serial.begin(115200);

    pinMode(peltier_1_pin, OUTPUT);
    pinMode(peltier_2_pin, OUTPUT);
    pinMode(peltier_3_pin, OUTPUT);
    pinMode(peltier_4_pin, OUTPUT);
    pinMode(peltier_5_pin, OUTPUT);
    pinMode(peltier_6_pin, OUTPUT);
    
    pinMode(peltier_1_reverse, OUTPUT);
    pinMode(peltier_2_reverse, OUTPUT);
    pinMode(peltier_3_reverse, OUTPUT);
    pinMode(peltier_4_reverse, OUTPUT);
    pinMode(peltier_5_reverse, OUTPUT);
    pinMode(peltier_6_reverse, OUTPUT);

    Wire.begin(SDA_PIN, SCL_PIN);  // Set custom I2C pins

    if (!display.begin(SSD1306_SWITCHCAPVCC, OLED_ADDR)) {
        Serial.println("SSD1306 initialization failed!");
        for (;;);
    }
    display.clearDisplay();
    display.setTextSize(1.6);
    display.setTextColor(SSD1306_WHITE);
    display.setCursor(0, 0);
    display.display();

    WiFi.softAP(ssid, password);
    Serial.println("Access Point Started");
    Serial.print("IP Address: ");
    Serial.println(WiFi.softAPIP());

    display.clearDisplay();
    display.setCursor(0, 0);
    display.println("Access Point Started");
    display.println("Set up with IP:");
    display.println(WiFi.softAPIP());
    display.display();
    server.begin();
}

void setPeltierPower(int pwmPin, int lowPin, int power) {
    int duty_cycle = map(abs(power), 0, 255, 0, 255);
    Serial.println(power);
    if (power > 0) {
        analogWrite(pwmPin, duty_cycle);
        analogWrite(lowPin, 0);  // Ensure the reverse pin is OFF
    } else if (power < 0) {
        analogWrite(pwmPin, 0);  // Ensure the forward pin is OFF
        analogWrite(lowPin, duty_cycle);
        
    } else {
        analogWrite(pwmPin, 0);
        analogWrite(lowPin, 0);
    }
}


void run_peltiers(int p1_power, int p2_power, int p3_power, int p4_power, int p5_power, int p6_power) {
    Serial.printf("Running Peltiers: P1=%d, P2=%d, P3=%d, P4=%d, P5=%d, P6=%d\n", p1_power, p2_power, p3_power, p4_power, p5_power, p6_power);
    
    setPeltierPower(peltier_1_pin, peltier_1_reverse, p1_power);
    setPeltierPower(peltier_2_pin, peltier_2_reverse, p2_power);
    setPeltierPower(peltier_3_pin, peltier_3_reverse, p3_power);
    setPeltierPower(peltier_4_pin, peltier_4_reverse, p4_power);
    setPeltierPower(peltier_5_pin, peltier_5_reverse, p5_power);
    setPeltierPower(peltier_6_pin, peltier_6_reverse, p6_power);
}




void loop() {
  WiFiClient client = server.available();
  
  if (client) {
    Serial.println("Client connected!");  // Add this
    while (client.connected()) {
      String message = client.readStringUntil('\n');
      Serial.print("Received: ");
      Serial.println(message);

      int spaceIndex1 = message.indexOf(' ');
      int spaceIndex2 = message.indexOf(' ', spaceIndex1 + 1);
      int spaceIndex3 = message.indexOf(' ', spaceIndex2 + 1);
      int spaceIndex4 = message.indexOf(' ', spaceIndex3 + 1);
      int spaceIndex5 = message.indexOf(' ', spaceIndex4 + 1);

      int p1_power = message.substring(0, spaceIndex1).toInt();
      int p2_power = message.substring(spaceIndex1 + 1, spaceIndex2).toInt();
      int p3_power = message.substring(spaceIndex2 + 1, spaceIndex3).toInt();
      int p4_power = message.substring(spaceIndex3 + 1, spaceIndex4).toInt();
      int p5_power = message.substring(spaceIndex4 + 1, spaceIndex5).toInt();
      int p6_power = message.substring(spaceIndex5 + 1).toInt();


      run_peltiers(p1_power, p2_power, p3_power, p4_power, p5_power, p6_power);
    }
    Serial.println("Client disconnected");  // Add this
    client.stop();
  }
}