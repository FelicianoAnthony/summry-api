#!/bin/bash

function new_server_init()
{
    echo -e "getting updates..."
    sudo apt-get update

    echo -e "doing upgrade..."
    sudo apt-get upgrade -y    
}

# $1= package to install from package_handler
function install_package()
{
    IS_PACKAGE_INSTALLED=$(dpkg -l | awk -v pkg="$1" ' $2 == pkg  ' | wc -l)
    if [[ IS_PACKAGE_INSTALLED -eq 0 ]]; then
        echo -e "installing package '${1}'\n"
        sudo apt-get install $1 -y
    else
        echo -e "'${1}' package already installed on server\n"
    fi
}


#$1=PACKAGES
function package_handler() 
{
    INSTALL_OR_REMOVE=$1
    echo -e "'${INSTALL_OR_REMOVE}' option selected"
    shift # shifts args to the left -- i.e. string arg needs to be passed BEFORE array arg...

    PACKAGES=("$@") # treats remaining args as an array...
    for PACKAGE in "${PACKAGES[@]}"; do
        
        if [[ $INSTALL_OR_REMOVE ==  "install" ]] ; then 
            install_package $PACKAGE

        elif [[ $INSTALL_OR_REMOVE == "remove" ]] ; then
            remove_package $PACKAGE

        else 
            echo -e "'${INSTALL_OR_REMOVE}' option not supported... run 'echo \$?' for script exit code"
            exit 1
        fi
    done
}


# $1=name of unit file to copy. assumes file is in same dir as script.
function copy_unit_file()
{
    UNIT_FILE_DIR="/etc/systemd/system"

    UNIT_FILE_SRC="${PWD}/${1}"
    UNIT_FILE_DEST="${UNIT_FILE_DIR}/${1}"

    sudo cp $UNIT_FILE_SRC $UNIT_FILE_DEST
    sudo chown root:root $UNIT_FILE_DEST
    sudo chmod 644  $UNIT_FILE_DEST
}


# $1=WINE_UNIT_FILE_CP
# $2=SUMMRY_API_UNIT_FILE
function change_service_status()
{
    sudo systemctl daemon-reload
    sudo systemctl $1 $2
}


# https://unix.stackexchange.com/questions/6345/how-can-i-get-distribution-name-and-version-number-in-a-simple-shell-script
# https://stackoverflow.com/questions/9733338/shell-script-remove-first-and-last-quote-from-a-variabl
# https://stackoverflow.com/questions/11392189/how-can-i-convert-a-string-from-uppercase-to-lowercase-in-bash
function dotnet_publish()
{    
    BITNESS=$(getconf LONG_BIT)
    RELEASE_NUM=$(lsb_release -rs)
    RELEASE_NAME=$(awk -F "=" '$1 == "NAME" {print $2}' /etc/os-release| tr -d '"' | tr '[:upper:]' '[:lower:]')

    # dotnet publish -c release -r ubuntu.22.04-x64
    PUBLISH_OS="${RELEASE_NAME}.${RELEASE_NUM}-x${BITNESS}"
    dotnet publish -c release -r $PUBLISH_OS
}






# init functions args
SUMMRY_API_UNIT_FILE="summry-api.service"
PACKAGES=("dotnet-sdk-6.0")
INSTALL="install"
STOP="stop"
START="start"
# REMOVE="remove"


# call functions
new_server_init
package_handler $INSTALL "${PACKAGES[@]}"

change_service_status $STOP $SUMMRY_API_UNIT_FILE
copy_unit_file $SUMMRY_API_UNIT_FILE

dotnet_publish
change_service_status $START $SUMMRY_API_UNIT_FILE