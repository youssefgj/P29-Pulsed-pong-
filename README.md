🏓 Pulsed Pong

Pulsed Pong is a Unity-based game that combines classic Pong mechanics with real-time heart rate tracking and adaptive gameplay.

The game dynamically adjusts its difficulty based on the player’s physiological state:
when the heart rate increases, the game slows down to reduce stress, and when it decreases, the game speeds up to increase challenge.

# Features
Classic Pong gameplay (Player vs BOT)
Real-time heart rate tracking during gameplay
Adaptive difficulty based on heart rate
High heart rate → slower gameplay
Low heart rate → faster gameplay
Graph visualization of heart rate over time
Display of session date and time
Switch between the last 3 sessions
Pause system with synchronized background music
Tech Stack
Unity 6
C#
Unity UI (Canvas, TextMeshPro)
JSON for local data storage

# Data Storage
Session data is stored locally in:
Application.persistentDataPath/pulspong_sessions.json
Each session includes:
Date & time of the session
Heart rate values over time
Gameplay duration

# How to Run
Open the project in Unity
Load the MainMenu scene
Press Play
Start a session
Open the Stats Scene to view the heart rate graph



# Future Improvements:
Improve graph performance
Add session filtering by date
Enhance UI/UX animations
Integrate real heart rate sensors