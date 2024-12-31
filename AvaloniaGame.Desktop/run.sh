#!/usr/bin/env bash

set -euxo pipefail

BIN_MAME="AvaloniaGame.Desktop"
DOTNET_VER="net8.0"

function run_release () {
    (cd "$(dirname "$0")" && dotnet build --configuration Release && cd "./bin/Release/$DOTNET_VER" && "./$BIN_MAME")
}

function run_debug () {
    (cd "$(dirname "$0")" && dotnet build --configuration Debug && cd "./bin/Debug/$DOTNET_VER" && "./$BIN_MAME")
}

# Ручная сборка и запуск программы из директории с исполняемым файлом
# если аргумент не указан - собрать и запустить в конфигурации для отладки
if [ $# -eq 0 ]; then
    run_debug
    exit 0
elif [ $# -gt 1 ]; then
    echo "Ожидается один аргумент, либо нисколько. Получено: $#"
    exit 1
fi

if [[ "$1" == "--debug" ]]; then
    run_debug
elif [[ "$1" == "--release" ]]; then
    run_release
fi

exit 0
