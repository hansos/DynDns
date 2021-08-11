# DynDns and MyIp

**DynDns** and **MyIp** is a client / server pair of tools that can be used to update an **AWS Route 53 Hosted Zone record** with a machine's global IP address.  
The tools can be a solution in cases where you want to use your own domain name to reach an internet server,  
but where the server is in a network where the public IP address changes over time.

Using the included server is optianal, the client can access any internet service returning the client's public IP as a string.

It can thus be a replacement for other Dynamic DNS solutions in those cases where being able to use your own domain name is necessary.

- [See the **DynDns** Readme](readme_dyndns.md) for more information about the DynDns Client.
- [See the **MyIp** Readme](readme_myip.md) for more information about the simple REST server.

