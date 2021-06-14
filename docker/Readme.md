# Setup docker containers

## Create folder structure & generate ssh keys
To create the docker container structures, use the setup script.
`user@host $ ./bin/setup.sh ./`

## Load default sshd config:
The following commands will load the default ssh config settings to both sftp servers. Change as needed.
`user@host $ cat ssh-config-base > sftp-import/sshd_config`
`user@host $ cat ssh-config-base > sftp-export/sshd_config`

## Setup user password
If you need password access to the docker containers to install SSH identiy keys use from another client the following command:
`user@host $ docker exec -it sftp-export ash`
`user@host $ docker exec -it sftp-import ash`

Once in shell you can use the passwd command to update the sftp user's password.
`/ # passwd sftp`

## Start containers:
`user@host $ docker-compose build && docker-compose up -d`
