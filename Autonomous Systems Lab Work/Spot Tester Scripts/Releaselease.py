# take_lease.py
from bosdyn.client import create_standard_sdk
from bosdyn.client.robot import Robot
from bosdyn.client.lease import LeaseClient, LeaseKeepAlive

username = "user2"
password = "simplepassword"
IP = "192.168.80.3"

def take_lease():
    print("[INFO] Creating SDK...")
    sdk = create_standard_sdk("LeaseTakingApp")
    
    print(f"[INFO] Connecting to robot at {IP}...")
    robot = sdk.create_robot(IP)
    
    print(f"[INFO] Authenticating as {username}...")
    robot.authenticate(username, password)
    
    print("[INFO] Getting lease client...")
    lease_client = robot.ensure_client(LeaseClient.default_service_name)

    lease = None
    try:
        print("[INFO] Taking lease...")
        lease = lease_client.take()
        print("✅ Lease successfully taken.")
        
        # Do stuff with Spot here

    finally:
        if lease:
            lease_client.return_lease(lease)
            print("🔁 Lease returned.")

take_lease()