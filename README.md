# Server Client BacgroundApp

This is the next step in developing a program to collect data about the client's PC and send it to the server. 

You can view the first stage of the program at this [link](https://github.com/bodunch/Build-Background-System-Reporter). 

In this version, in addition to collecting user data in the background, the program was implemented to send this data to a server on the local network. The data is sent to open port 5000. Accordingly, the server accepts this data only from this port and is not available for other requests. 
The mechanism for the server to receive data was implemented using the POST method. The data itself is sent from the client’s PC in JSON format in the request body.

Additionally, information was included not only about the processes running on the PC but also about the programs themselves, to make it easier for the administrator to retrieve the data. 
