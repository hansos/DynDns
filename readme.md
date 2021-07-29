# DynDns 

A tool for updating AWS Route 53 Hosted Zone records with a computer's global IP.
The tool can be useful if you are running a globally available server on a network without a statist IP address.

## Requirements

- The solution can be installed and executed on Linux and Windows systems only.

## Installation

### On Linux servers

The executable(s) must be installed in */opt/dyndns* together with the *dyndns.dat* and *zones.dat* files.

### On Windows servers

The executable *dyndns.exe* can be installed in any directory.  
The data files *dyndns.dat* and *zones.dat* files must be installed in the same directory as the executable.

## Version history

### 2021-07-29

- Initial version.

### Roadmap

- Submit path to data file directory as command line arguments.
- Submit path to log file directory as command line arguments.
