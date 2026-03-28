# Solar-Explorer-AR
Ever wonder how small Earth actually is compared to the Sun? Most people can’t visualize the true scale of space from a textbook. Solar Explorer AR brings the solar system into your physical environment using cutting-edge Augmented Reality.

# 🌌 Solar Explorer AR
**A Spatial UX Case Study & AR Foundation Project**

[![Unity 2022.3+](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)](https://unity.com/)
[![ARFoundation](https://img.shields.io/badge/Platform-ARFoundation-green.svg)](https://unity.com/platforms/ar-vr)

## 📌 Project Overview
The **Solar Explorer AR** is an educational tool designed to solve the "Cognitive Scale Gap"—the human inability to conceptualize the massive proportions of our solar system. By translating astronomical data into a room-scaled AR experience, users can physically perceive the size difference between the Sun and its planets.

## 🧠 The UX Problem
Standard diagrams fail to show scale because Earth would be a microscopic speck if the Sun were drawn to size. 
* **Challenge:** How to make the solar system interactive without overwhelming the user?
* **Solution:** A static "Gallery Mode" that prioritizes accessibility and size comparison over complex orbital mechanics.

## ✨ Key Features & UX Engineering
* **Guided Onboarding:** A clear instructional UI ensures users understand how to scan their environment before the experience begins.
* **Intentional Interactions:** * **Single Tap:** Precise placement of the solar system on detected planes.
  * **Long Press:** Deliberate "Inspect" action to trigger the Info Panel, preventing accidental UI pop-ups.
  * **Double Tap:** An "Emergency Exit" reset logic to reposition the system.
* **Pinch-to-Zoom:** Dynamic management of the world scale (0.01x to 2.0x) to fit any environment, from a desk to a park.
* **Accessibility-First:** Removed orbital movement to ensure users of all motor skill levels can easily interact with planetary targets.

## 🛠️ Technical Stack
* **Engine:** Unity 2022.3+
* **Framework:** AR Foundation (ARCore/ARKit)
* **APIs Used:** `ARAnchorManager` (Modern `AddComponent<ARAnchor>` implementation for 2026 standards), `ARRaycastManager`.
* **UI:** TextMeshPro with custom space-themed sprites.


## 👩‍💻 Developer
**Haneen** *UXUI Engineer & Women Techmakers Ambassador* 📍 Alexandria, Egypt

---
*Created for the 2026 UX/UI Engineering.*
