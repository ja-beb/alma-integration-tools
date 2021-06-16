# alma-intergration-tools

## Setup SFTP docker containers 

1. Create folder structure & generate ssh keys
To create the docker container structures use the setup script in the docker-sftp directory to setup the initial file structure..

`user@host $ ./bin/setup.sh ./`

2. Load default sshd config:
The following commands will load the default ssh config settings to both sftp servers. Change as needed.

`user@host $ cat ssh-config-base > sftp-import/sshd_config`

`user@host $ cat ssh-config-base > sftp-export/sshd_config`

3. Setup user password
If you need password access to the docker containers to install SSH identiy keys use from another client the following command:

`user@host $ docker exec -it sftp-export ash`

`user@host $ docker exec -it sftp-import ash`

Once in shell you can use the passwd command to update the sftp user's password.

`/ # passwd sftp`

4. Start containers:

`user@host $ docker-compose build && docker-compose up -d`

5. Copy the respective files into sftp-import/home/* directories

## Setup SFTP server name
Edit host file to include custom SFTP name sftp-test.local or change the app.config for Bursar, Fianance, and AccountSync programs to reflect docker SFTP host name.

## Building dotnet container
Run the following command to build the dotnet containers:

`user@host $ docker build --pull -t alma-integration .`


## Run Sync Programs

### Bursar 
`user@host $ docker run --rm --network=host alma-integration dotnet /app/AlmaIntegrationTools.Bursar.dll`

### Finance
`user@host $ docker run --rm --network=host alma-integration dotnet /app/AlmaIntegrationTools.Finance.dll`

### Account Sync
`user@host $ docker run --rm --network=host alma-integration dotnet /app/AlmaIntegrationTools.AccountSync.dll`


