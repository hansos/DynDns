# DynDns

A tool for updating AWS Route 53 Hosted Zone records with a computer's global IP.
The tool can be useful if you are running a globally available server on a network without a statist IP address.

## Requirements

- The solution can be installed and executed on Linux and Windows systems only.
- A valid AWS account is needed.
- A Zone record for the domain and necessary zone records must be defined using the aws client or the aws web console.
- The aws client must be installed on the machine, and should be initiated with Access Key and a secret key.

## Installation

### On Linux servers

The executable(s) must be installed in **/opt/dyndns** together with the **dyndns.dat** and **zones.dat** files.

### On Windows servers

The executable **dyndns.exe** can be installed in any directory.  
The data files **dyndns.dat** and *zones.dat* files must be installed in the same directory as the executable.

## Data files

### dyndns.dat

### zones.dat

A semicolon separated data file with all zone records that should be updated with the current IP of the computer.  
The file must contain the AWS as well as the zone record name.

```csharp
Z053464332EXGH6TGNB6C; myservice.myserver.net.;
Z053464332EXGH6TGNB6C; esogame.myserver.net.;
```

A line starting with a Hash (#) will be treated as a comment line.

> [!NOTE]
> Ensure that each zone name ends with a dot!

## Command line parameters

### Command line switches

*This functionality is currently under development.*

| Switch     |Purpose                               |
| ---------- | ------------------------------------ |
| -q         | Quiet mode.                          |
| -t *n*     | Set Trace level to *n*.              |
| --help     | Display this help and exit.          |
| --version  | Output version information and exit. |

#### Trace levels

| Level | Code  | Description              |
| ----  | ----- | ------------------------ |
| 0     | NONE  | No trace                 |
| 1     | ERR   | Error messages           |
| 2     | SUCC  | Success Messages         |
| 3     | WARN  | Warning messages         |
| 4     | TRACE | Trace messages (default) |

## Version history

### 2021-08-04 (V1.0.0)

- Writing a log line to **ipchanges.log** when dyndns updates the DNS server.

### 2021-07-29 (V0.9.0)

- Initial version.

### Roadmap

- Submit path to data file directory as command line arguments.
- Submit path to log file directory as command line arguments.
- Add the final dot to zone record names automatically if not specified in the zones.dat file.
- Create the dyndns.dat file if it doesn't exists.
- Log version number on startup trace message ([Fabric.Fabric] Program started).
- Create flag to limit trace line
- Improve Trace Line status (distinguish between SUCCESS and TRACE with more).
