# DynDns

DynDns is a tool that updates an AWS Route 53 Hosted Zone record with a machine's global IP address.  
The tool can be a solution in cases where you want to use your own domain name to reach an internet server,  
but where the server is in a network where the IP address changes over time.

## Requirements

- The solution can be installed and executed on Linux and Windows systems only.
- A valid AWS account is needed.
- A Zone record for the domain and necessary zone records must be defined using the aws client or the aws web console.
- The aws client must be installed on the machine, and should be initiated with Access Key and a secret key.

## Installation

### On Linux servers

The executable(s) must be installed in */opt/dyndns* together with the *zones.dat* files.

### On Windows servers

The executable *dyndns.exe* can be installed in any directory.  
The data file *zones.dat* files must be installed or created in the execution directory.

## Data files

### IP Buffer

The IP buffer file contains the latest valid global IP address. The file is updated each time the DNS is updated with new IP's.  
As default, this file will be created in the execution directory and named  *ipbuffer.dat*.
This path and name can be changed by using the *--ip-buffer-path*.

### Zone file

A semicolon separated data file with all zone records that should be updated with the current IP of the computer.  
The file must contain the AWS Zone ID  as well as the zone record name.
The path and name can be changed by setting a value for the  *--run* or *--test-run* argument.

Comment lines can be included at any place by starting the line with a hash (#). Comments cannot be added to record lines.

If the zone file is empty or not found, the application will terminate.

```csharp
#This is a comment
Z053464332EXGH6TGNB6C; myservice.myserver.net
Z053464332EXGH6TGNB6C; esogame.myserver.net
```

## Command line aguments

| Short      | Long                  | Purpose                                       |
| ---------- | --------------------- | --------------------------------------------- |
|            | --help                | Display this help and exit.                   |
|            | --version             | Output version information and exit.          |
| -q         | --quiet               | Quiet mode. No output to console.             |
| -r         | --run=[PATH]          | Run the DNS update engine. Unless PATH to a data file is spesified,zones are read from the 'zones.dat' data file located together with the program file. |
|            | --test-run=[PATH]     | Run the DNS update engine, bot don't write the IP address to the DNS zone record.Unless PATH to a data file is spesified, zones are read from the 'zones.dat' data file located together with the program file. |
| -t         | --trace-level=LEVEL   | Set trace level for trace file. 0=nothing, 4=full trace. Values and codes accepted.                                   |
| -l         | --log-path=PATH       | Path to the log directory. If not submitted, log files are created in '/var/log/dyndns'.                              |
| -i         | --ip-buffer-path=PATH | Path to the IP buffer file. Unless PATH to a zone file is spesified, the IP buffer is created 'as ipbuffer.dat' located together with the program file. |

### Trace levels

| Level | Code  | Description              |
| ----  | ----- | ------------------------ |
| 0     | NONE  | No trace                 |
| 1     | ERR   | Error messages           |
| 2     | SUCC  | Success Messages         |
| 3     | WARN  | Warning messages         |
| 4     | TRACE | Trace messages (default) |

## Version history

### 2021-08-10 (V0.9.3)

- Documentation page improved.
- Default name for the Ip-buffer file renamed from *dyndns.dat* renamed to *ipbuffer.dat*.
- The IP-buffer file is now created if it don't exists.
- Format for *zones.dat* is simplified.

### 2021-08-09 (V0.9.2)

- Version command implemented.
- Help page implemented.
- Startup arguments for Run and Test Run implemented.
- Trace level argument implemented.
- Trace to console implemented.
- Quiet mode implemented to prevent console output.
- Setting path and file name to IP address buffer is now possible using the -i argument.

### 2021-08-04 (V0.9.1)

- Writing a log line to *ipchanges.log* when dyndns updates the DNS server.

### 2021-07-29 (V0.9.0)

- Initial version.

### Roadmap

- ~~Submit path to data file directory as command line arguments.~~
- ~~Submit path to log file directory as command line arguments.~~
- ~~Add the final dot to zone record names automatically if not specified in the zones.dat file.~~
- ~~Create the ipbuffer.dat file if it doesn't exists.~~
- ~~Create flag to limit trace line~~
- ~~Improve Trace Line status (distinguish between SUCCESS and TRACE with more).~~
