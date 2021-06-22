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

`user@host $ docker exec -it sftp-export passwd sftp`

`user@host $ docker exec -it sftp-import passwd sftp`

4. Start containers:

`user@host $ docker-compose build && docker-compose up -d`

5. Copy the respective files into sftp-import/home/* directories

`ssh-copy-id -i ~/.ssh/id_ed25519.pub -p 2222 sftp@hostname`

`ssh-copy-id -i ~/.ssh/id_ed25519.pub -p 2221 sftp@hostname`

## Setup SFTP server name
Edit host file to include custom SFTP name sftp-test.local or change the app.config for Bursar, Fianance, and AccountSync programs to reflect docker SFTP host name.

## Building dotnet container

First edit the respective appsettings.json file to reflect the proper SFTP settings for Bursar, Finance & AccountSync.

### Bursar
`user@host $ docker build --rm --pull -t alma-intergration-bursar -f docker/Dockerfile/bursar .`

### Finance
`user@host $ docker build --rm --pull -t alma-intergration-finance -f docker/Dockerfile/finance .`

### Account Sync
`user@host $ docker build --rm --pull -t alma-intergration-account-sync -f docker/Dockerfile/account-sync .`

## Running containers

### Bursar
`user@host $ docker build --rm --pull -t alma-intergration-bursar -f docker-app/Dockerfile-Bursar .`

### Finance
`user@host $ docker build --rm --pull -t alma-intergration-finance -f docker-app/Dockerfile-finance .`

### Account Sync
`user@host $ docker build --rm --pull -t alma-intergration-account-sync -f docker-app/Dockerfile-AccountSync .`
