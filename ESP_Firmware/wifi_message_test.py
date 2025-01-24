import socket
import random
import time


class PeltierClient:
    def __init__(self, host, port):
        self.host = host
        self.port = port
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.connect((host, port))

    def send_command(self, p1, p2, p3, p4, p5, p6):
        try:
            command = f"{p1} {p2} {p3} {p4} {p5} {p6}\n"
            self.socket.sendall(command.encode())
            response = self.socket.recv(1024).decode().strip()
            print("ESP32 Response:", response)
        except Exception as e:
            print("Error:", e)

    def close(self):
        self.socket.shutdown(socket.SHUT_RDWR)
        self.socket.close()


if __name__ == "__main__":
    esp_host = "192.168.36.174"  # Change this to your ESP32's IP address
    esp_port = 80  # Change if your ESP32 server uses a different port
    client = PeltierClient(esp_host, esp_port)

    try:
        while True:
            p1 = random.randint(-0, 100)
            p2 = random.randint(0, 100)
            p3 = random.randint(0, 100)
            p4 = random.randint(0, 100)
            p5 = random.randint(0, 100)
            p6 = random.randint(0, 100)

            print(f"Sending: {p1}, {p2}, {p3}, {p4}, {p5}, {p6}")
            client.send_command(p1, p2, p3, p4, p5, p6)

         #   time.sleep(0.1)  # Adjust delay as needed
    except KeyboardInterrupt:
        print("Closing connection...")
    finally:
        client.close()
