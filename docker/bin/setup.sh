#!/bin/bash
create_folder()
{
    echo "create directory structure: ${1}"
    mkdir "${1}"
    mkdir "${1}/home";
    mkdir "${1}/home/.ssh";
    mkdir "${1}/home/AccountSync";
    mkdir "${1}/home/Finance";
    mkdir "${1}/home/Bursar";
    mkdir "${1}/ssh";
    mkdir "${1}/ssh/private";
    mkdir "${1}/ssh/public";
    touch "${1}//sshd_config";
}

generate_keys()
{
    # Generate keys
    echo "create ssh host keys"
    ssh-keygen -f "${1}/ssh/private/ssh_host_key-rsa" -N '' -t rsa
    ssh-keygen -f "${1}/ssh/private/ssh_host_key-ecdsa" -N '' -t ecdsa -b 521
    mv "${1}/ssh/private/ssh_host_key-ecdsa.pub" "${1}/ssh/public/ssh_host_key-ecdsa.pub"
    mv "${1}/ssh/private/ssh_host_key-rsa.pub" "${1}/ssh/public/ssh_host_key-rsa.pub"
}

if [ -z $1 ]
then
    echo "Base folder required."
elif [ -d $1 ]
then
    path=`realpath "${1}/sftp-export"`;
    create_folder "${path}";
    generate_keys "${path}";

    path=`realpath "${1}/sftp-import"`;
    create_folder "${path}";
    generate_keys "${path}";

    # add example pickup files.
    touch "${path}/home/AccountSync/banner.sif";
    touch "${path}/home/Finance/finance.xml";
    touch "${path}/home/Bursar/bursar.xml";
else
    echo "Invalid base folder specified." 
fi