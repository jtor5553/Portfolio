from bosdyn.client import create_standard_sdk
from bosdyn.client.robot_command import RobotCommandClient, RobotCommandBuilder
from bosdyn.client.lease import LeaseClient, LeaseKeepAlive
import sys
import time

def main():
    if len(sys.argv) != 4:
        print("Usage: python sit.py <ROBOT_IP> <USERNAME> <PASSWORD>")
        return

    ip = sys.argv[1]
    username = sys.argv[2]
    password = sys.argv[3]

    sdk = create_standard_sdk("SpotSit")
    robot = sdk.create_robot(ip)
    robot.authenticate(username, password)
    robot.time_sync.wait_for_sync()

    lease_client = robot.ensure_client(LeaseClient.default_service_name)
    command_client = robot.ensure_client(RobotCommandClient.default_service_name)

    with LeaseKeepAlive(lease_client, must_acquire=True, take_lease=True):
        if not robot.is_powered_on():
            robot.power_on(timeout_sec=20)

        sit_command = RobotCommandBuilder.synchro_sit_command()
        command_client.robot_command(sit_command)
        print("Spot is now sitting.")
        time.sleep(3)

if __name__ == "__main__":
    main()
