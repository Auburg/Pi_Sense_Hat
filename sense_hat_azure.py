import time
from sense_hat import SenseHat
import json
from dotenv import load_dotenv
import os
import asyncio
from azure.iot.device.aio import IoTHubDeviceClient
from pynput import keyboard
from threading import Event 

sense = SenseHat()
sense.set_imu_config(True, True, True) 

quit_event = Event()

def on_press(key):
    print('{0} pressed'.format(key))    

def on_release(key):    
    if key == keyboard.Key.esc:
        # Stop listener
        quit_event.set()        
        return False
    else:
        return True


async def main():    
    
    load_dotenv()  
    
    # Fetch the connection string from an environment variable
    conn_str = os.environ.get("AZURE_CONN")
    
    if conn_str == None:
        print("AZURE_CONN variable not set, exiting")
        return
    
    print("Starting data collection, press 'esc' to quit")

    # Create instance of the device client using the authentication provider
    device_client = IoTHubDeviceClient.create_from_connection_string(conn_str)

    # Connect the device client.
    await device_client.connect()
    
    listener = keyboard.Listener(
    on_press=on_press,
    on_release=on_release)
    listener.start()
    
    orientation = {
      "roll" : 0.0,
      "pitch" : 0.0,
      "yaw" : 0.0
    }
    
    sense_hat_data = {
      "temp" : 0.0,
      "pressure" : 0.0,
      "humidity" : 0.0,
      "mag" : 0.0,
      "orientation" : orientation,
      "gyro" : orientation,
      "accel" : orientation
    }   
    
    
    while quit_event.is_set() == False:
        
        sense_hat_data["temp"] = sense.temperature
        sense_hat_data["pressure"] = sense.pressure
        sense_hat_data["humidity"] = sense.humidity
        sense_hat_data["mag"] = sense.compass
        
        sense_hat_data["orientation"] = sense.orientation
        sense_hat_data["gyro"] = sense.gyro
        sense_hat_data["accel"] = sense.accel
        
        jsonData = json.dumps(sense_hat_data)        
        
        print("Sending message...")
        await device_client.send_message(jsonData)
        print("Message successfully sent!")
        
        time.sleep(1)
        
    print("Shutting down client")   
    await device_client.shutdown()


if __name__ == "__main__":
    asyncio.run(main())

    


