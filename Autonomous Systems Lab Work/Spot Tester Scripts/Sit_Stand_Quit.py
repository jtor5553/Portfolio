from bosdyn.client import create_standard_sdk
from bosdyn.client.robot_command import RobotCommandClient, RobotCommandBuilder, blocking_stand
from bosdyn.client.lease import LeaseClient, LeaseKeepAlive
import speech_recognition as sr
import sys
import time

ROBOT_IP = ""
USERNAME = ""
PASSWORD = ""

def connect_to_robot():
    sdk = create_standard_sdk("SpotVoiceControl")
    robot = sdk.create_robot(ROBOT_IP)
    robot.authenticate(USERNAME, PASSWORD)
    robot.time_sync.wait_for_sync()
    lease_client = robot.ensure_client(LeaseClient.default_service_name)
    command_client = robot.ensure_client(RobotCommandClient.default_service_name)
    return robot, lease_client, command_client

def spot_stand():
    robot, lease_client, command_client = connect_to_robot()
    lease_client.take()  # forcibly grabs the lease
    with LeaseKeepAlive(lease_client, must_acquire=True):

        if not robot.is_powered_on():
            robot.power_on(timeout_sec=20)
        blocking_stand(command_client, timeout_sec=10)
        print("Spot is now standing.")

def spot_sit():
    robot, lease_client, command_client = connect_to_robot()
    lease_client.take()  # forcibly grabs the lease
    with LeaseKeepAlive(lease_client, must_acquire=True):

        if not robot.is_powered_on():
            robot.power_on(timeout_sec=20)
        sit_command = RobotCommandBuilder.synchro_sit_command()
        command_client.robot_command(sit_command)
        print("Spot is now sitting.")
        time.sleep(3)

def listen_for_command():
    recognizer = sr.Recognizer()
    mic = sr.Microphone()
    print("Voice control started. Say 'stand', 'sit', or 'quit'.")

    while True:
        with mic as source:
            recognizer.adjust_for_ambient_noise(source)
            print("Listening...")
            audio = recognizer.listen(source, timeout=5, phrase_time_limit=5)
        try:
            command = recognizer.recognize_google(audio).lower()
            print(f"You said: {command}")

            if "stand" in command:
                spot_stand()
            elif "sit" in command:
                spot_sit()
            elif "quit" in command or "exit" in command:
                print("Exiting voice control.")
                break
            else:
                print("Command not recognized.")

        except sr.UnknownValueError:
            print("Could not understand the audio.")
        except sr.RequestError as e:
            print(f"Speech recognition error: {e}")
        except Exception as e:
            print(f"Error executing command: {e}")

if __name__ == "__main__":
    listen_for_command()
