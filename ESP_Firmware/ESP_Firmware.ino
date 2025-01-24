#include <Arduino.h>

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

void loop() {
  run_peltiers(20, 15, 100, 0);
}
