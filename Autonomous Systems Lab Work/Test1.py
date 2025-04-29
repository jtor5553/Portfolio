from openai import OpenAI  # OpenAI needs Wi-Fi
import speech_recognition as sr  # Speech recognition by Google needs Wi-Fi
import sys
from playsound import playsound  # For playing audio files
import os  # For file existence check

# My key for the OpenAI API (be mindful when using it; the more we use it, the more it costs me)
client = OpenAI(api_key="")

# Gets the vocal input through the mic
def get_vocal_input():
    recognizer = sr.Recognizer()
    with sr.Microphone() as source:
        print("Speak...")
        audio = recognizer.listen(source)  # Captures audio from the mic

    try:
        text = recognizer.recognize_google(audio)  # Uses Google Web Speech API to convert audio to text
        print(f"You said: {text}")
        return text
    except sr.UnknownValueError:  
        print("Sorry, I could not understand the audio.")
        return None
    except sr.RequestError as e:  
        print(f"Could not request results from Google Web Speech API; {e}")
        return None

# Uses TTS to create speech from text and stores it in an MP3 file
def text_to_speech(text, output_file="output_audio.mp3"):
    try:
        with client.audio.speech.with_streaming_response.create(
            model="tts-1",  
            voice="alloy",  
            input=text
        ) as response:
            # Saves the response content to a file
            with open(output_file, "wb") as f:
                for chunk in response.iter_bytes():
                    f.write(chunk)
        print(f"Audio response saved to {output_file}")
    except Exception as e:
        print(f"Error during TTS: {e}")  

try:
    # Gets vocal input from the user
    user_input = get_vocal_input()

    if not user_input:  # Error detection
        print("No valid input received. Exiting.")
        sys.exit(1)

    # Sends the vocal input to GPT-3.5-turbo
    completion = client.chat.completions.create(
        model="gpt-3.5-turbo",  
        messages=[
            {"role": "system", "content": "You are a happy robotic dog named Spot made by Boston Dynamics."},  
            {"role": "user", "content": user_input}
        ]
    )
    spot_response = completion.choices[0].message.content

    # Converts Spot's response to speech using TTS API
    tts_output_file = "output_audio.mp3"
    text_to_speech(spot_response, tts_output_file)

    # Plays the generated audio
    print("Playing Spot's response...")
    if os.path.exists(tts_output_file):
        playsound(tts_output_file)
    else:
        print(f"Error: File {tts_output_file} not found.")

except Exception as e:  
    print(f"An error occurred: {e}")
    sys.exit(1)