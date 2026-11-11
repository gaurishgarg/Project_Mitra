# Project Mitra

**An Embodied Tangible Learning Environment for Spatial Reasoning**

Mitra is an educational technology prototype that combines a physical Micro:bit controller with a virtual 3D environment developed in Unity. It is designed to support spatial reasoning activities through tangible interaction, allowing learners to manipulate virtual objects by physically rotating a handheld controller.

The project was developed as part of **ET-606: Educational Technology Tools Lab, IIT Bombay**.

## Project Overview

Mitra integrates hardware and software components to provide three sets of spatial reasoning activities:

1. **Maze Navigation:** Learners physically rotate the Micro:bit to control a virtual maze and guide a ball towards the exit.
2. **Object Rotation:** Learners rotate virtual 3D objects to match a target orientation using physical rotations of the Micro:bit.
3. **Spatial Projections:** Learners predict the 2D projections of 3D objects, observe the projections from different orientations, and explain their predictions using a Predict–Observe–Explain (POE) approach.

The system also includes interaction logging, session replay, assessment interfaces, and a dashboard for reviewing interaction data.

## Repository Structure

| Directory | Description |
|---|---|
| [`Unity Source Files/`](./Unity%20Source%20Files/) | Unity project source files, including scenes, scripts, prefabs, and assets. |
| [`Activtiy_EXE/`](./Activtiy_EXE/) | Standalone executable build of the Mitra learning environment. |
| [`Replay_EXE/`](./Replay_EXE/) | Standalone application for replaying recorded interaction sessions. |
| [`Microbit Code/`](./Microbit%20Code/) | Compiled Micro:bit firmware (`.hex`) for transmitting orientation data. |
| [`Dashboard/`](./Dashboard/) | Standalone HTML dashboards for visualising interaction logs. |
| [`Pre and Post Google AI Studio/`](./Pre%20and%20Post%20Google%20AI%20Studio/) | Standalone HTML application for pre- and post-assessment activities. |
| [`LA-Reflect/`](./LA-Reflect/) | Student and teacher access shortcuts for LA-Reflect. |

## Getting Started

### 1. Running the Mitra Application

1. Open the [`Activtiy_EXE/`](./Activtiy_EXE/) directory.
2. Run the application executable.
3. Connect the Micro:bit to the computer using USB.
4. Use the Micro:bit as the physical controller to interact with the virtual environment.

The executable and its accompanying Unity runtime files should remain together in the same directory.

### 2. Micro:bit Setup

The compiled firmware is available in [`Microbit Code/`](./Microbit%20Code/).

1. Connect the Micro:bit to a computer.
2. Flash the supplied `.hex` firmware onto the device.
3. Connect the device to the Mitra application through USB serial communication.

### 3. Assessment Interface

The standalone assessment application is available in [`Pre and Post Google AI Studio/`](./Pre%20and%20Post%20Google%20AI%20Studio/).

Open `index.html` in a compatible web browser to access the assessment interface.

### 4. Interaction Dashboard

The dashboard files are available in [`Dashboard/`](./Dashboard/):

- [`index.html`](./Dashboard/index.html)
- [`spatial_reasoning_dashboard_timestamps.html`](./Dashboard/spatial_reasoning_dashboard_timestamps.html)

These standalone HTML interfaces are intended for visualising and reviewing recorded interaction data.

### 5. Session Replay

The standalone replay application is available in [`Replay_EXE/`](./Replay_EXE/).

It is intended to reconstruct recorded sessions using the interaction data captured by the Unity application.

## Tools and Technologies

- **Unity 6.3:** Development of the 3D learning environment, activities, interaction logging, and session replay.
- **BBC Micro:bit:** Capturing physical orientation and transmitting controller input.
- **3D Printing:** Fabrication of a handheld enclosure for the Micro:bit.
- **LA-Reflect:** Organisation of pre- and post-assessment questions.
- **HTML, CSS and JavaScript:** Development of standalone assessment and dashboard interfaces.
- **Google AI Studio:** Assistance in developing the interaction-data dashboard.

## Interaction Logging

The Unity application records interaction data in CSV format, including timestamps, activity states, object transformations, camera transformations, and task completion information.

The recorded data is intended to support subsequent analysis and session reconstruction through the replay application and dashboard.

## Third-Party Assets

The project incorporates selected 3D models and visual assets sourced from CGTrader and obtained under royalty-free licensing terms. The original assets remain the property of their respective creators and are subject to their applicable licence conditions.

## Project Status

Mitra is a working educational technology prototype developed for the ET-606 Tools Lab project. It provides an integrated environment for exploring tangible interaction and spatial reasoning activities.

---

**Developed at:** Centre for Educational Technology, IIT Bombay  
**Course:** ET-606 – Educational Technology Tools Lab  
**Project:** Mitra