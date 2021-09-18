<h1>Rat server (Remote Administration Tool server)</h1>

This is a rat server written in C# for Windows-only platforms, it works by waiting for client to connect thus giving it accsess to client's command line, desktop view, files.
Right know you are not able to upload files to client's machine tho you can download them.

<h2> How it works </h2>
<ol>
        <li> You need to have ports <b>1420</b>(CMD and file port) and <b>1421</b>(Screen share port) forwarded.</li>
        <li> The console that boots up is for the commands below V and for the windows cmd commands ( needs a client selected )
        <li> Once you boot into the app there will be a list box of connected clients on the right where you will select the client you want to interact with, ONCE YOU CLICK ON THE CLIENT IF THE PREVIES CLIENT SHARED HIS SCREEN IF IS TURNED OFF AUTOMATTICLY.
        <li> 
                Console commands:
                <ul>
                        <li> <h2>download FILEPATH</h2>(Needs a client selected): Downloads the FILEPATH file from select client's machine </li>
                        <li> <h2>clear</h2> </li>
                        <li> <h2>clearbuffer</h2>(Needs a client selected): Clears the tcp buffer on port 1420 (in case of some error)</li> 
                        <li> <h2>clearbuffers</h2> Clears all 1420 port buffers</li> 
                        <li> <h2>screenon</h2>  (Needs a client selected): Turns on screen share for selected client</li> 
                        <li> <h2>screenoff</h2>  (Needs a client selected): Turns off screen share for selected client</li> 
                </ul>
        </li>
</ol>
![ScreenShot](https://i.postimg.cc/nzzh492Y/Test.png?raw=true)
