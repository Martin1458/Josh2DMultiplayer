# Josh2DMultiplayer
Another Unity project to play with ml-agents.
# How to run?
I created this game with ml-agents 2.0.1 in Unity 2022.3. For the Python environment, I used Python 3.9.13 with the packages from requirements.txt.
To train the agents use this command:

```mlagents-learn --torch-device="cuda:0" --time-scale=20 --run-id=Test```

Make sure you are using the Python virtual environment, where you installed all the packages from requirements.txt.
  
- ```--torch-device="cuda:0"``` will set the 'Interface'/device to GPU instead of CPU which will speed up the process a lot.
- ```--time-scale=20``` changes the Unity.timeScale parameter, so that the game plays faster (default is 20). You can also set it to 1 while testing the agents, to help you debug any problems in the code.
- ```--run-id=Test``` sets the name of the onnx output file (the neural network model file).
