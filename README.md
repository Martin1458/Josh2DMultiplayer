# Josh2DMultiplayer
In this project I tried to train two agents to fight against each other in the scene ```AgentDuel```. 
When I first started this project, I wasn't very sure about how to go about it. I made some big decisions about the game's structure that, I later realized, weren't the best. Now, I see that I need to take a different approach to make the training process work. So, I'm going to start over in another repo.

<img src="https://github.com/Martin1458/Josh2DMultiplayer/assets/91092809/f99fa895-2040-4351-9b01-93fd39ba7282" width=40% height=40%>

# How to run?
I created this game with ml-agents 2.0.1 in Unity 2022.3. For the Python environment, I used Python 3.9.13 with the packages from requirements.txt.
To train the agents use this command:

```mlagents-learn --torch-device="cuda:0" --time-scale=20 --run-id=Test```

Make sure you are using the Python virtual environment, where you installed all the packages from requirements.txt.
  
- ```--torch-device="cuda:0"``` will set the 'Interface'/device to GPU instead of CPU which will speed up the process a lot.
- ```--time-scale=20``` changes the Unity.timeScale parameter, so that the game plays faster (default is 20). You can also set it to 1 while testing the agents, to help you debug any problems in the code.
- ```--run-id=Test``` sets the name of the onnx output file (the neural network model file).

# Trubbleshooting:
| Error | What happened | Quick fix |
| -------- | -------- | -------- |
| ```'python' is not recognized as an internal or external command, operable program or batch file.```  | You didn't install Python, or haven't set it to PATH. | Go to https://www.python.org/downloads/ and install Python 3.9.13. And make sure to check the "Add Python 3.9 to PATH" checkbox in the Python installation wizard. |
| ```ERROR: No matching distribution found for torch==2.0.1+cu117```  | Pip was unable to find the Torch package, but don't worry you can get the correct pip command on the official pyTorch site: https://pytorch.org/get-started/locally/  | ```pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu117```  |
|  ```'mlagents-learn' is not recognized as an internal or external command, operable program or batch file. ```  | The mlagents package wasn't installed correctly, you can try to install mlagents directly or check if all of your required packages are installed. | ```python -m pip install mlagents==0.30.0``` ```python -m pip install -r requirements.txt ```  |
