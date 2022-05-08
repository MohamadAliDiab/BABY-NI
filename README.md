# BABY-NI

When first launching the program, be aware to put a break point in the loadFileWatcher (as indicated in the image below)
![image](https://user-images.githubusercontent.com/59798108/167297862-860b675e-8ede-4cd5-884f-38c5d9b71b5e.png)

This is due because the loadFileWatcher is selecting data from the Database which may have not been written yet (the insert command was sent at the end of the parser API
but it takes time, so to avoid getting a null value, a break point is put here to give the program time to finish inserting the data, multiple solutions were available 
but weren't implemented due to time concerns)

Any files already existent in the directories where the fileWatchers are watching WON'T be detected, a solution was also found but didn't have enough time to implement it

