# PeDistance Mod
It uses GTA V mod to teleport the player into random controlled location, spawns a pedestrian, gets the distance between the camera point and the pedestrian, saves it into a csv file and loops until it gets to set sample number.

[Demo](https://youtu.be/QHK2PYEUhGo)

# Requirements

* Grand Theft Auto V (Epic or Steam Version)
* PC with Windows Installed and minimum GTA V requirement


# How to install
- Download the latest released ```dist.zip``` (or download the repo and get [dist](dist) content)
- Go to your main GTA V folder and create a ```scripts``` folder
- Unpack ```dist.zip``` into ```scripts``` folder
- Start your GTA V single player

# How to Start

* Start GTA V and enter in Story Mod
* Deactivate all in game notifications
* Press F5 to Open Menu
* Open settings menu and chose your favorite setting to collect data or leave default values
* Come back to main menu and press ```Start Collecting Data``` to begin collecting data
* Leave mod woring until it finishes to collect data (it can take a while)
* You will find collected data in  ```scripts/MachineLearningProject/data/Dataset.csv```

### Structure MachineLearningProject Folder
- Data 
    - Location: where the player can spawn, you can expand it yourself using ```Utils Menu```
	- Dataset: it contains all data of ped in relative images
- Images
	- It will contains all screenshot (only if you checked ```Save Screenshot Locally``` in Settings Menu)

# Main menu

<img src="data/Assets/MainMenu.png" width="50%">

- Use ```Start Collecting Data``` to begin collecting
- Whenever you are in trouble, use ```Reset``` menu voice to get out of it
- Use ```Clear Data``` to clean dataset and images folder data. **NB:** be carefull when you use it, you will lose all your collected data.
# Settings

In the settings menu you can manage:

- MaxCollected data: set number of data to collect
- Camera Fov: set value of Field Of View of the camera while collecting
- Camera Angle: set desired camera pitch value
- Camera Fixed Height: set camera height from ground
- Image Format: choose to salve image in JPEG or PNG format
- Save Screenshot Locally: set to save image locally in the  ```scripts/MachineLearningProject/images```
- Print Box: set to print bounding box into final collected image
- Randomize Time
- Randomize Weather
- Various Delay: change delay based on your platform performance. Lower you can get, more data you can collect in a time period.

Relative to your PC Specific, you should set various delays in the settings menu to manage the renderization of the environment and all other stuff.
If your pc performances are low you should increment delays. 
On the other hand you should decrement delays.

<img src="data/Assets/SettingsMenu.png">

<img src="data/Assets/GTAmod_high.gif">

# Utils Menu
In this regard, there is an additional menu called "Utils Menu" that allows you to perform actions that can help the user to make measurements, move easily and quickly in the game map. It is also possible to save your own spawn zones that will be added to the locations dataset.

**NB:** It is advisable to press the save button only after making sure that the new position fits the purpose of the pedestrian spawning.


