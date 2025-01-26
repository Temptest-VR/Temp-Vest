import socket
import random
import time

# Define the server address and port
SERVER_ADDRESS = '192.168.4.1'
PORT = 80

# Flag to toggle flip-flop mode
flip_flop_mode = True  # Set to False for random behavior

# Flip-flop state tracking
flip_flop = [False] * 2

# Function to send power values to the server
def send_power_data(client_socket):
    global flip_flop

    if flip_flop_mode:
        # Flip-flop between -100 and 100
        power_values = [100 if state else 100 for state in flip_flop]
        # Toggle the states for next iteration
        flip_flop = [not state for state in flip_flop]
    else:
        # Generate random numbers for power values
        power_values = [random.randint(-100, 100) for _ in range(2)]

    # Format and send the message
    message = ' '.join(map(str, power_values)) + '\n'
    client_socket.sendall(message.encode())
    print(f"Sent: {message.strip()}")

# Main loop to send data every second
if __name__ == "__main__":
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as client_socket:
        client_socket.connect((SERVER_ADDRESS, PORT))
        while True:
            send_power_data(client_socket)
            time.sleep(0.1)  # Wait for 20 seconds before sending again
