#include <Arduino.h>
#include <WiFi.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
#define OLED_RESET -1  // Reset pin (not used)
#define OLED_ADDR 0x3C  // I2C address for OLED

#define SDA_PIN 3
#define SCL_PIN 2

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

const char* ssid = "TempVest";      // Replace with your WiFi SSID
const char* password = "12345678";  // Replace with your WiFi password

WiFiServer server(80);

const int peltier_1_pin = 4;
const int peltier_2_pin = 18;
const int peltier_3_pin = 19;
const int peltier_4_pin = 10;
const int peltier_5_pin = 1;
const int peltier_6_pin = 5;

const int peltier_1_reverse = 21;
const int peltier_2_reverse = 20;
const int peltier_3_reverse = 9;
const int peltier_4_reverse = 7;
const int peltier_5_reverse = 6;
const int peltier_6_reverse = 0;

const int PWM_FREQ = 10;
const int PWM_RESOLUTION = 16;
const int max_power = 255; // Assuming the power range is 0-255

// Function to draw a horizontal bar for power levels
void drawBar(int x, int y, int value, int max_value) {
  int bar_length = map(value, 0, max_value, 0, 128); // Map value to screen width (128)
 // display.println("P " + value);
  display.fillRect(x, y, bar_length, 1, WHITE); // Draw filled bar
  display.drawRect(x, y, 128, 3, WHITE); // Draw bar outline
}



void setup() {
    Serial.begin(115200);

    pinMode(peltier_1_pin, OUTPUT);
    pinMode(peltier_2_pin, OUTPUT);
    pinMode(peltier_3_pin, OUTPUT);
    pinMode(peltier_4_pin, OUTPUT);

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

void run_peltiers(int p1_power, int p2_power, int p3_power, int p4_power, int p5_power, int p6_power) {
    Serial.printf("Running Peltiers: P1=%d, P2=%d, P3=%d, P4=%d\n", p1_power, p2_power, p3_power, p4_power);
    setPeltierPower(peltier_1_pin, p1_power);
    setPeltierPower(peltier_2_pin, p2_power);
    setPeltierPower(peltier_3_pin, p3_power);
    setPeltierPower(peltier_4_pin, p4_power);
    setPeltierPower(peltier_5_pin, p5_power);
    setPeltierPower(peltier_6_pin, p6_power);
    
    display.clearDisplay();
    display.setCursor(0, 0);
    display.println("Peltier Power:");
    drawBar(0, 20, p1_power, max_power);
    drawBar(0, 30, p2_power, max_power);
    drawBar(0, 40, p3_power, max_power);
    drawBar(0, 50, p4_power, max_power);
    drawBar(0, 60, p5_power, max_power);
    drawBar(0, 70, p6_power, max_power);
    display.display();
}

void setPeltierPower(int pin, int power) {
    int duty_cycle = map(abs(power), 0, 100, 0, 255);
    Serial.printf("Setting pin %d to duty cycle %d\n", pin, duty_cycle);
    analogWrite(pin, duty_cycle);
    if (power < 0) {
      Serial.println("Reversing temp!"); // trigger relay sequence
    }
}

WiFiClient client;

void loop() {
    if (!client || !client.connected()) {
        client = server.available();
        if (client) {
            Serial.println("Client connected");
        }
    }

    if (client) {
        if (client.connected()) {
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
            // client.println("ACK");
        }
    }
    client.stop();
}
