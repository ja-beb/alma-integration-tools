#!/bin/bash

ssh-copy-id -i ~/.ssh/id_ed25519.pub -p 2222 sftp@localhost
ssh-copy-id -i ~/.ssh/id_ed25519.pub -p 2221 sftp@localhost
