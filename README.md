## Example .Net Core MicroService Architecture
- Example Consumer
- Example Producer
- RabbitMQ
- MassTransit
- Docker / Docker Compose

## Important note about Security
Be mindful that this is an example project and contains logins for RabbitMQ that are public. If you adapt this project for your own use make sure to generate unique logins by following these steps:
1. Update the **name** and **password_hash** (see below) for each user in `services/rabbitmq/config/definitions.json`.
1. Update the RabbitMQ host login in the resolver files:
   - `consumers/microservice.example.consumer/MicroService.Example.Resolver/Resolvers/MessageBrokerResolver.cs`
   - `producers/microservice.example.producer/MicroService.Example.Resolver/Resolvers/MessageBrokerResolver.cs`  
   NOTE: It is strongly recommended not to hardcode the username and password into these files and instead to implement a secure credential manager which is currently out of the scope of this example project.
```
Generate a RabbitMQ password hash:

1. Install the Docker plugin for VSCode.
2. In the Docker menu, Right-Click and Start the 'masstransit-rabbitmq-debug' container.
3. Once started, Right-Click it again and 'Attach Shell'.
4. In the terminal window that opens, enter the command: rabbitmqctl hash_password <your_unencrypted_password>
5. The password hash will be written to the output window for you to copy.
```
## Prerequisites
These should be installed/configured if they are not already.
1.	Install the [Linux kernel update package](https://wslstorestorage.blob.core.windows.net/wslblob/wsl_update_x64.msi).
2.	Setup WS2 on Windows:  
    <sub><ins>Run in PowerShell:</ins></sub>  
    <sub>Enable the Virtual Machine Platform optional feature required to use WS2.</sub>  
    <code>dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart</code>  
    <sub>Set the default version for new installations to use WS2.</sub>  
    <code>wsl --set-default-version 2</code>  

## Install Ubuntu on WS2
<ins>Run in PowerShell</ins>
- Show a list of any distributions that are already installed.  
<code>wsl -l -v</code>
- If you already have a distribution installed on WS1 you can convert it to WS2.  
<code>wsl --set-version Ubuntu-20.04 2</code>
- To install the latest build of Ubuntu (you can find available versions on the Microsoft Store).  
<code>wsl --install -d Ubuntu-20.04</code>

## Manually install Docker.exe for Windows
1. Download Docker.exe from [GitHub: StefanScherer/docker-cli-builder](https://github.com/StefanScherer/docker-cli-builder/releases).  
   Note: Expand the Assets accordion of the latest Docker CLI release to find the exe.
1. Create the directory `C:\bin` and copy the exe into it.
1. Linux WSL will keep taking all the system memory. To limit this create the following file:

> C:\bin\\.wslconfig
```
# Settings apply across all Linux distros running on WSL 2
[wsl2]
memory=12GB
```
## Install Docker & Docker Compose on Ubuntu
1. Open the **Ubuntu on Windows** application from the start menu.
2. Install Docker (run each of the following commands in turn):
```
# sudo apt-get update
# sudo apt-get upgrade
# sudo apt install --no-install-recommends apt-transport-https ca-certificates curl gnupg2
# cd ~
# curl -fsSL https://get.docker.com -o get-docker.sh
# sudo sh get-docker.sh
# sudo usermod -aG docker $USER
# echo "export DOCKER_HOST=tcp://localhost:2375" >> ~/.bash_profile
```
3. Install Docker Compose:
- Check to see if it is already installed.  
<code>docker compose version</code>
- Install using the package handler.  
<code>sudo apt-get install docker-compose-plugin</code>

## Windows Environment Variables
Open the System Environment Variables dialog in Windows and add the following entries:
| Action | System Variable | Value | Description |
| ------ | --------------- | ----- | ----------- |
| Edit | PATH | C:\bin | Register location of Docker.exe for Windows. |
| Add | DOCKER_HOST | tcp://localhost:2375 | Exposes the url that Docker is listening to in Ubuntu. |

## Run Docker
This process is the same every time you want to start docker on your environment:
1. Open the Ubuntu on Windows application from the start menu.
1. Enter the following command in the Ubuntu terminal:
<code>sudo dockerd -H localhost</code>

If the Docker Daemon fails to start you can try checking for and killing existing processes:  
<sub>Get PID of running Docker service(s).</sub>  
<code>ps -ef | grep docker</code>  
<sub>Kill process with the PID.</sub>  
<code>kill %pid%</code>

## Repository Configuration
1. Clone the repository and open in VSCode.
1. Run through the following commands. These should only need to be done once.  
<sub><ins>Run in a VSCode Terminal scoped to the root of the repository</ins></sub>  
<sub>Install the required vsdbg storage for the debugger to attach to.</sub>  
<code>docker-compose up install.vsdbg</code>

## Debug from VSCode
1. Load the project in VSCode.
1. Switch to the Run and Debug window.
1. Choose a Launch configuration in the dropdown list.
1. Click the play icon to run the project.

## Testing
[Swagger]
1. Open the Swagger URL for the Producer via http://localhost:5000/swagger.

[Postman]
1. Example GET: http://localhost:5000/api/Simple/CheckEndpoint
1. Example POST: http://localhost:5000/api/Simple/GetResponse?value=test

[Unit Tests]
1. There are UnitTests in the Consumer and Producer.
