# MyIp

**MyIp** is a simple REST server returning the connected client's RemoteIpAddress.  
The tool can be a solution in cases where you want to use your own domain name to reach an internet server,  
but where the server is in a network where the public IP address changes over time.

It can thus be a replacement for other Dynamic DNS solutions in those cases where being able to use your own domain name is necessary.

## Requirements

- The solution can be installed and executed on Linux and Windows systems only.

## Limitations

- The Only IPv4 are tested..

## Installation

The executable should be installed using Nginx or another suitable web server in front.

## Version history

### 2021-08-11 (V0.9.0)

- Initial version.

### Roadmap

- Add an optional Access Key mechanism to limit access. 
