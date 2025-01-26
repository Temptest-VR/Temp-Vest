import socket
from pythonosc import dispatcher
from pythonosc import osc_server
import threading
import argparse
import time

class HapticsState:
    def __init__(self, debug_mode=False):
        self.temp_intensity = 1.0  # Default multiplier
        self.cold_haptics = False  # Default no inversion
        self.left_hand_touch = 0.0
        self.right_hand_touch = 0.0
        self.sock = None  # TCP socket
        self.debug_mode = debug_mode
        self.running = True  # Control flag for update thread

    def send_tcp_message(self):
        """Send a TCP message with the current state of both hands"""
        base_value = 100  # Base value for touch events
        
        # Calculate values using current state
        left_value = base_value * self.left_hand_touch
        right_value = base_value * self.right_hand_touch
        
        # Apply TempIntensity multiplier
        left_value = left_value * self.temp_intensity
        right_value = right_value * self.temp_intensity

        # Apply ColdHaptics multiplier (-1 if True)
        if self.cold_haptics:
            left_value = -left_value
            right_value = -right_value

        # Format message with newline
        message = f"{int(left_value)} {int(right_value)}\n"

        if self.debug_mode:
            print(f"DEBUG MODE - Values: {message.strip()}")
            return

        if self.sock is None:
            return

        try:
            self.sock.send(message.encode('utf-8'))
            print(f"Sent message: {message.strip()}")
        except socket.error as e:
            print(f"Socket error occurred: {e}")

    def update_loop(self):
        """Run continuous update loop at 10Hz"""
        while self.running:
            self.send_tcp_message()
            time.sleep(0.1)  # Sleep for 100ms (10Hz)

def temp_intensity_handler(address, *args):
    if args and len(args) > 0:
        state.temp_intensity = float(args[0])
        print(f"TempIntensity updated to: {state.temp_intensity}")

def cold_haptics_handler(address, *args):
    if args and len(args) > 0:
        state.cold_haptics = bool(args[0])
        print(f"ColdHaptics updated to: {state.cold_haptics}")

def hand_touch_handler(address, *args):
    if not args or len(args) == 0:
        return

    value = float(args[0])

    if address == "/avatar/parameters/LeftHandTouch":
        state.left_hand_touch = value
    elif address == "/avatar/parameters/RightHandTouch":
        state.right_hand_touch = value

def setup_osc_server():
    # Create dispatcher
    disp = dispatcher.Dispatcher()
    
    # Register handlers
    disp.map("/avatar/parameters/TempIntensity", temp_intensity_handler)
    disp.map("/avatar/parameters/ColdHaptics", cold_haptics_handler)
    disp.map("/avatar/parameters/RightHandTouch", hand_touch_handler)
    disp.map("/avatar/parameters/LeftHandTouch", hand_touch_handler)

    # Start OSC server
    server = osc_server.ThreadingOSCUDPServer(
        ("127.0.0.1", 9001),
        disp
    )
    print("OSC Server listening on 127.0.0.1:9001")
    return server

def main():
    global state
    
    # Parse command line arguments
    parser = argparse.ArgumentParser()
    parser.add_argument('--debug', action='store_true', help='Run in debug mode (print values instead of sending TCP)')
    args = parser.parse_args()

    # Initialize state with debug mode
    state = HapticsState(debug_mode=args.debug)
    
    # Predefined target IP and port for TCP
    TARGET_IP = '192.168.4.1'
    TARGET_PORT = 80
    
    try:
        # Create TCP socket only if not in debug mode
        if not args.debug:
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.connect((TARGET_IP, TARGET_PORT))
            state.sock = sock
            print(f"Connected to TCP server at {TARGET_IP}:{TARGET_PORT}")
        else:
            print("Running in DEBUG MODE - Values will be printed to terminal")
        
        # Start update thread
        update_thread = threading.Thread(target=state.update_loop)
        update_thread.daemon = True  # Thread will exit when main program exits
        update_thread.start()
        
        # Setup and start OSC server
        server = setup_osc_server()
        
        print("Server is running. Press Ctrl+C to stop.")
        server.serve_forever()

    except ConnectionRefusedError:
        print(f"TCP Connection to {TARGET_IP}:{TARGET_PORT} was refused.")
    except socket.error as e:
        print(f"Socket error occurred: {e}")
    except KeyboardInterrupt:
        print("\nProgram interrupted. Shutting down...")
    finally:
        # Stop the update loop
        state.running = False
        if state.sock:
            state.sock.close()

if __name__ == '__main__':
    main()
