
import socket
import random
import time

# Define the server address and port
SERVER_ADDRESS = '192.168.4.1'
PORT = 80


# Function to send random numbers to the server
def send_random_data():
    # Create a socket and connect to the server
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as client_socket:
        client_socket.connect((SERVER_ADDRESS, PORT))

        # Generate random numbers for power values
        power_values = [random.randint(-100, 100) for _ in range(6)]

        # Format the message as a string with space-separated values
        message = ' '.join(map(str, power_values)) + '\n'

        # Send the message to the server
        client_socket.sendall(message.encode())
        print(f"Sent: {message.strip()}")


# Main loop to send random data every second
if __name__ == "__main__":
    while True:
        send_random_data()
        time.sleep(0.1)  # Wait for 1 second before sending again


