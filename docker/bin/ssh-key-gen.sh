#!/bin/bash
if [ -d $1 ]
then
    echo "create ssh host keys"
    ssh-keygen -f $1/ssh_host_key-rsa -N '' -t rsa
    ssh-keygen -f $1/ssh_host_key-dsa -N '' -t dsa
    ssh-keygen -f $1/ssh_host_key-ecdsa -N '' -t ecdsa -b 521
else
    echo "output file not found"
fi