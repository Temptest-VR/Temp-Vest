import socket
import random
import time

ESP_IP = "192.168.4.1"  # Default SoftAP IP of ESP32
ESP_PORT = 80  # Port defined in the ESP32 code


def send_command():
    p1, p2, p3, p4, p5, p6 = [random.randint(0, 100) for _ in range(6)]
    command = f"{p1} {p2} {p3} {p4} {p5} {p6}\n"

    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((ESP_IP, ESP_PORT))
            s.sendall(command.encode())
            response = s.recv(1024).decode()
            print(f"Sent: {command.strip()} | ESP Response: {response}")
    except Exception as e:
        print("Connection error:", e)


while True:
    send_command()

