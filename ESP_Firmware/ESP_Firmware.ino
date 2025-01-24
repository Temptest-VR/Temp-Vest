#include <Arduino.h>
#include <WiFi.h>

const char* ssid = "TempVest";      // Replace with your WiFi SSID
const char* password = "12345678";  // Replace with your WiFi password

WiFiServer server(80);  // Create a server on port 80


const int peltier_1_pin = 5;
const int peltier_2_pin = 6;
const int peltier_3_pin = 7;
const int peltier_4_pin = 8;

const int PWM_FREQ = 500;
const int PWM_RESOLUTION = 8; // 8-bit resolution (0-255)

void setup() {
    Serial.begin(115200);
    
    pinMode(peltier_1_pin, OUTPUT);
    pinMode(peltier_2_pin, OUTPUT);
    pinMode(peltier_3_pin, OUTPUT);
    pinMode(peltier_4_pin, OUTPUT);

    WiFi.begin(ssid, password);

    Serial.print("Connecting to WiFi");
    while (WiFi.status() != WL_CONNECTED) {
        delay(1000);
        Serial.print(".");
    }
    Serial.println("\nConnected to WiFi");
    Serial.print("ESP32 IP: ");
    Serial.println(WiFi.localIP());

    server.begin();  // Start the server
}

void run_peltiers(int p1_power, int p2_power, int p3_power, int p4_power) {
    Serial.printf("Running Peltiers: P1=%d, P2=%d, P3=%d, P4=%d\n", p1_power, p2_power, p3_power, p4_power);
    setPeltierPower(peltier_1_pin, p1_power);
    setPeltierPower(peltier_2_pin, p2_power);
    setPeltierPower(peltier_3_pin, p3_power);
    setPeltierPower(peltier_4_pin, p4_power);
}

void setPeltierPower(int pin, int power) {
    int duty_cycle = map(abs(power), 0, 100, 0, 255); // Map power (-100 to 100) to PWM range
    Serial.printf("Setting pin %d to duty cycle %d\n", pin, duty_cycle);
    analogWrite(pin, duty_cycle);
}


WiFiClient client;

void loop() {
    if (!client || !client.connected()) {
        client = server.available();  // Accept new client connection only if not already connected
        if (client) {
            Serial.println("Client connected");
        }
    }

    if (client && client.connected()) {
        if (client.available()) {
            String message = client.readStringUntil('\n');  // Read message until newline
            Serial.print("Received: ");
            Serial.println(message);

            // Split the message by space and store the values
            int spaceIndex1 = message.indexOf(' ');
            int spaceIndex2 = message.indexOf(' ', spaceIndex1 + 1);
            int spaceIndex3 = message.indexOf(' ', spaceIndex2 + 1);

            int p1_power = message.substring(0, spaceIndex1).toInt();
            int p2_power = message.substring(spaceIndex1 + 1, spaceIndex2).toInt();
            int p3_power = message.substring(spaceIndex2 + 1, spaceIndex3).toInt();
            int p4_power = message.substring(spaceIndex3 + 1).toInt();

            // Debug: print out the split values
            Serial.print("Peltier Power Values: ");
            run_peltiers(p1_power, p2_power, p3_power, p4_power);

            client.println("ACK");  // Send acknowledgment
        }
    }
}


