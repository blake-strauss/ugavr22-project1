# Blake Strauss

<img src="https://user-images.githubusercontent.com/32816567/188712382-84af37a9-a3a0-4b65-a8cf-a4591406968e.jpg" width="214" height="289">

# Description

This project is my first VR game created in Unity. The game introduces basic locomotion methods along with a task for the player to complete. The player must find 3 pieces of a key hidden around a maze. The player must construct the key and use it to unlock the exit and escape. The key parts are randomly spawned around a list of given points and emit a short "bling" sound to help the player locate them. Once all 3 pieces are set in place, the game rewards the user with the completed key.

# Issues

- My biggest problem was when the player placed each key part in the specific "hologram" location, the complex mesh collider did not provide accurate collision detection and therefore the target location would flip between "isFound = true" and "isFound = false". This is similar to button bouncing in the real world.
- The cause of this problem was the game setting the position of the key part while the player was still "grabbing" the key part, resulting in the key updating to 2 different positions.
- The solution arrived at was to delete the key part's rigidbody as to prevent the user from interacting with the key part, and removing the key part as a "grabbable object".
- The accuracy of the hand manipulation of the object could be improved. When holding an object and moving your hand, the object jitters and does not provide a smooth motion in sync with the hand. This is probably due to the complex rigidbody as a result from creating the object in Blender.

# Video
