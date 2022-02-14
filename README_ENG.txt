This project was created as a solution to the trial task of the Mind Games hackathon by the "zachem" team.

- CONTENTS:

1) Repository structure
2) Technologies used
3) Implemented functionality
4) API interaction
5) System architecture

1) REPOSITORY STRUCTURE

Repository contains:

1.1) Application source code (folder: TableParse)
1.2) Technical demo (file: demo.mp4)
1.3) Diplomas confirming the successful completion of the 20th Chin test by all team members (folder: Certificates)
1.4) This very file you are reading right now (file: README_ENG.txt)
1.5) The same file, but in Russian (file: README_RUS.txt)

2) DESCRIPTION OF THE TECHNOLOGIES USED (AND LIBRARIES)

* The application was written on Windows Forms .NET Core 3.1 (C#)
* All functions of the application have been implemented through the standard System library
* For correct work with JSON, the third-party library Newtonsoft.Json was used

3) IMPLEMENTED FUNCTIONALITY

The application has two workspaces in separate windows:

3.1) Workspace "Leaderboard"

* Parses TOP100 players from gokgs.com (position, name, rating)
* Uses the API to get data on the results of the last two games of each player
* Collects the loaded data into one table for more comfortable work with it
* Allows you to open a preview of any of the two games in the workspace "Game" by clicking on the corresponding result
* (Additional functionality) allows you to sort data by each column

3.2) Workspace "Game"

* Uses API to get information about the game and every move in it (color, position, score, grid size, time remaining)
* Displays this information graphically on a board
* Displays side information about each move in the text box at the bottom of the screen
* Allows you to switch between moves by clicking on the arrows on the left and right
* (Additional functionality) correctly displays information when any of the players goes overtime

4) API INTERACTION

API interaction scenario:

4.1) Sending a POST LOGIN request in the prescribed form
4.2) GET request
4.3) For each player from the TOP100, send a POST request JOIN_ARCHIVE_REQUEST with the name parameter - player's name
4.4) GET request
4.5) Sending a POST LOGOUT request in the prescribed form
4.6) GET request

* Requests are formatted and processed using JsonConvert from Newtonsoft
* Server requests are sent sequentially
* A request of the UNJOIN_REQUEST type is not used, because it unreasonably lowers performance

5) SYSTEM ARCHITECTURE

The ParseTable source code contains the following entities:

5.1) Form1.cs - GUI workspace "Leaderboard"
5.2) Form2.cs - GUI workspace "Game"
5.3) Program.cs - Program entry point
5.4) Player.cs - Class describing a player from TOP100
5.5) Stone.cs - Class describing one single game move
5.6) API.cs - Class for implementing the API correctly
5.7) JsonFormsUpstream - Class for sending requests to the server
5.8) JsonFormsDownstream - Class for receiving responses from the server