import socket

ESP_IP = "192.168.4.1"  # Default SoftAP IP of ESP32
ESP_PORT = 80           # Port defined in the ESP32 code

def send_command(p1, p2, p3, p4, p5, p6):
    command = f"{p1} {p2} {p3} {p4} {p5} {p6}\n"  # Ensure newline termination
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((ESP_IP, ESP_PORT))
        s.sendall(command.encode())
        response = s.recv(1024).decode()
        print("ESP Response:", response)

# Example: Set peltier power levels
send_command(50, 0, -30, 100, -10, 20)
