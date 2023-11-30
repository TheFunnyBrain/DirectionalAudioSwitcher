# DirectionalAudioSwitcher
A small bit of Unity code to crossfade snapshots based on which audiosource the player is facing.

![image](https://github.com/TheFunnyBrain/DirectionalAudioSwitcher/assets/32782486/78203dd4-ab25-4e38-9762-828807a07869)

## How to use:

### Audio Source and Mixer Groups 
- This script uses Camera.Main (got once in the start method). You'll need a camera tagged as "MainCamera".
- Make Mixer groups for each audio source you'll be controlling.
- Assign these mixer groups in the audio source's inspector.
  ![image](https://github.com/TheFunnyBrain/DirectionalAudioSwitcher/assets/32782486/e86fa5cf-f800-44c0-8379-0ec49448bf99)
- Position your sources in different places, I did mine north, south, east, and west.

### Mixer Snapshots
- Make snapshots for each source, where everything except its group are turned down fully.
![image](https://github.com/TheFunnyBrain/DirectionalAudioSwitcher/assets/32782486/7c4a5308-05b5-4f84-927c-a41bc29e33d4)

### Component Setup
- Add a DirectionalAudioSnapshotSwitcher
- I've made a right click context menu on the DirectionalAudioSnapshotSwitcher component, this will set up 4 snapshot entries for North, South, East, and West. Totally optional, but a timesaver for this example use case.
![image](https://github.com/TheFunnyBrain/DirectionalAudioSwitcher/assets/32782486/742527a5-7e0d-4962-952d-3effe9e05ab2)
- Assign the fields for each AudioSnapShotData in the inspector (e.g.the transform for the north audio source, and north snapshot)
![image](https://github.com/TheFunnyBrain/DirectionalAudioSwitcher/assets/32782486/be34cf6f-4122-4e3e-a250-9007aef69b10)

Hopefully press play, and you'll see changes when you turn the camera (either through a player script, VR, or grabbing the transform in scene view.
