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

const char* ssid = "TempVest";
const char* password = "12345678";

WiFiServer server(80);

const int peltier_1_pin = 0;
const int peltier_2_pin = 17;
const int peltier_1_reverse = 16;
const int peltier_2_reverse = 18;
const int therm_1_pin = 34;
const int therm_2_pin = 35;

bool p_1_stop = false;
bool p_2_stop = false;
const int temp_threshold = 2300;
int last_p1_power = 0;
int last_p2_power = 0;

void setup() {
    Serial.begin(115200);
    pinMode(peltier_1_pin, OUTPUT);
    pinMode(peltier_2_pin, OUTPUT);
    pinMode(peltier_1_reverse, OUTPUT);
    pinMode(peltier_2_reverse, OUTPUT);
    Wire.begin(SDA_PIN, SCL_PIN);
    
    if (!display.begin(SSD1306_SWITCHCAPVCC, OLED_ADDR)) {
        Serial.println("SSD1306 initialization failed!");
        for (;;);
    }
    display.clearDisplay();
    display.setTextSize(1);
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

void setPeltierPower(int pwmPin, int lowPin, int power, bool stop) {
    if (stop) {
        analogWrite(pwmPin, 0);
        analogWrite(lowPin, 0);
        return;
    }
    
    int duty_cycle = map(abs(power), 0, 100, 0, 255);
    if (power > 0) {
        analogWrite(pwmPin, duty_cycle);
        analogWrite(lowPin, 0);
    } else if (power < 0) {
        analogWrite(pwmPin, 0);
        analogWrite(lowPin, duty_cycle);
    } else {
        analogWrite(pwmPin, 0);
        analogWrite(lowPin, 0);
    }
}

void checkTemperature() {
    int therm_1_value = analogRead(therm_1_pin);
    int therm_2_value = analogRead(therm_2_pin);
    
    p_1_stop = therm_1_value > temp_threshold;
    p_2_stop = therm_2_value > temp_threshold;
    
    Serial.printf("Therm1: %d, Therm2: %d, P1 Stop: %d, P2 Stop: %d\n", therm_1_value, therm_2_value, p_1_stop, p_2_stop);
    
    setPeltierPower(peltier_1_pin, peltier_1_reverse, last_p1_power, p_1_stop);
    setPeltierPower(peltier_2_pin, peltier_2_reverse, last_p2_power, p_2_stop);
}

void loop() {
    checkTemperature();
    
    WiFiClient client = server.available();
    if (client) {
        Serial.println("Client connected!");
        while (client.connected()) {
            if (client.available()) {
                String message = client.readStringUntil('\n');
                Serial.print("Received: ");
                Serial.println(message);
                
                int spaceIndex1 = message.indexOf(' ');
                int spaceIndex2 = message.indexOf(' ', spaceIndex1 + 1);
                int p1_power = message.substring(0, spaceIndex1).toInt();
                int p2_power = message.substring(spaceIndex1 + 1, spaceIndex2).toInt();
                
                last_p1_power = p1_power;
                last_p2_power = p2_power;
                
                setPeltierPower(peltier_1_pin, peltier_1_reverse, p1_power, p_1_stop);
                setPeltierPower(peltier_2_pin, peltier_2_reverse, p2_power, p_2_stop);
            }
        }
        Serial.println("Client disconnected");
        client.stop();
    }
}
