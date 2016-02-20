11/7/15
Starting on the project.  Server handles all game mechanics like eating?  Just working on what we need to send.

11/17/2015, 1:32 AM, Morning of Assignment Deadline
For AgCubio, the start screen requires you to enter in a value for the name and server. Once done, pressing enter will
begin connection with the input server name. If the server cannot be found, a warning will be given and the user may
try again.

If the server connects and loses connection afterwards, the user will be sent back to the starting screen.

During AgCubio, the player's cursor is slightly off to give the player an extra obstacle in navigating the world. This
is a feature designed to simulate a drunken state or dizziness and adds a level of difficulty.

To further this, when splitting the world collapses and the player's cubes will be surrounded until "regaining consciousness" which in this
case would be merging back together.

Upon being consumed, a reward screen is displayed that gives the player a title as well as shows off the stats accumulated
during gameplay. Closing the stat window will create two start windows to be created to prompt the player to use a
competitive but albeit more difficult advantage with the ability to use two cubes at the same time.

##Note
For some reason, after logging in, the game won't start painting until you move the window around.

-Implementation
+We used stringbuilder for the data from the server.
+We employed two delegates.
+We overrided OnPaint.
+We utilized cases for KeyPresses which allowed splitting.
+We stored connection exceptions in the state object and checked it everytime the delegate is called.
+We chose the ViewPort window to be the size of 600 as it was smaller than the actual world size but allowed a lot of visibility.
+FPS should trend around 35. This is the maximum value that we could process.


-This code was inspired by and not limited to the following:

+Jim's Code
+Agario
+Cody's Food
+Fall Out 4
+Table Flipping
+StackOverFlow
+MSDN
+Matthew