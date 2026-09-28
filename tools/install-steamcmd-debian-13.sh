#!/usr/bin/env bash

#
# install-steamcmd.sh
#
# Installs SteamCMD and required dependencies on Debian 13 (Trixie).
#
# Usage:
#   sudo ./install-steamcmd.sh
#
# Options:
#   --user USERNAME     Create/use a dedicated SteamCMD user.
#   --no-user           Do not create a dedicated user.
#
# Example:
#   sudo ./install-steamcmd.sh --user steam
#

set -Eeuo pipefail

readonly SCRIPT_NAME="${0##*/}"
readonly REQUIRED_DEBIAN_VERSION="13"
readonly DEFAULT_STEAM_USER="steam"

STEAM_USER="${DEFAULT_STEAM_USER}"
CREATE_USER=true

log() {
    printf '[%s] %s\n' "${SCRIPT_NAME}" "$*"
}

error() {
    printf '[%s] ERROR: %s\n' "${SCRIPT_NAME}" "$*" >&2
}

die() {
    error "$*"
    exit 1
}

usage() {
    cat <<EOF
Usage:
  sudo ${SCRIPT_NAME} [OPTIONS]

Options:
  --user USERNAME   SteamCMD service account to create/use.
                    Default: ${DEFAULT_STEAM_USER}

  --no-user         Install SteamCMD without creating a dedicated user.

  -h, --help        Show this help.
EOF
}

require_root() {
    if [[ "${EUID}" -ne 0 ]]; then
        die "This script must be run as root or with sudo."
    fi
}

check_debian() {
    [[ -r /etc/os-release ]] ||
        die "Unable to determine operating system."

    # shellcheck disable=SC1091
    source /etc/os-release

    [[ "${ID:-}" == "debian" ]] ||
        die "This script supports Debian only."

    if [[ "${VERSION_ID:-}" != "${REQUIRED_DEBIAN_VERSION}" ]]; then
        die "Debian ${REQUIRED_DEBIAN_VERSION} is required. Detected: ${VERSION_ID:-unknown}"
    fi

    log "Detected Debian ${VERSION_ID} (${VERSION_CODENAME:-unknown})."
}

check_architecture() {
    local arch
    arch="$(dpkg --print-architecture)"

    case "${arch}" in
        amd64)
            log "Detected amd64 architecture."

            if ! dpkg --print-foreign-architectures | grep -qx 'i386'; then
                log "Enabling i386 multiarch support..."
                dpkg --add-architecture i386
            else
                log "i386 multiarch support is already enabled."
            fi
            ;;

        i386)
            log "Detected native i386 architecture."
            ;;

        *)
            die "Unsupported architecture: ${arch}. SteamCMD Debian package requires i386 compatibility."
            ;;
    esac
}

enable_non_free_repository() {
    log "Checking Debian repositories for non-free..."

    local changed=false

    #
    # Debian 13 normally uses deb822 sources:
    #   /etc/apt/sources.list.d/debian.sources
    #
    if [[ -f /etc/apt/sources.list.d/debian.sources ]]; then
        if grep -Eq '^Components:.*\bnon-free\b' /etc/apt/sources.list.d/debian.sources; then
            log "non-free repository component is already enabled."
            return
        fi

        log "Adding non-free to Debian deb822 repository configuration..."

        cp \
            /etc/apt/sources.list.d/debian.sources \
            /etc/apt/sources.list.d/debian.sources.bak

        sed -Ei \
            '/^Components:/ {
                /\bnon-free\b/! s/$/ non-free/
            }' \
            /etc/apt/sources.list.d/debian.sources

        changed=true
    fi

    #
    # Support traditional /etc/apt/sources.list installations as well.
    #
    if [[ -f /etc/apt/sources.list ]]; then
        if grep -Eq \
            '^[[:space:]]*deb .*trixie.*[[:space:]]non-free([[:space:]]|$)' \
            /etc/apt/sources.list; then

            log "non-free repository component is already enabled in sources.list."
        else
            log "Adding non-free to traditional Debian repository entries..."

            cp \
                /etc/apt/sources.list \
                /etc/apt/sources.list.bak

            sed -Ei \
                '/^[[:space:]]*deb .*trixie/ {
                    /\bnon-free\b/! s/$/ non-free/
                }' \
                /etc/apt/sources.list

            changed=true
        fi
    fi

    if [[ "${changed}" == false ]]; then
        if ! grep -RqsE \
            '^Components:.*\bnon-free\b|^[[:space:]]*deb .*[[:space:]]non-free([[:space:]]|$)' \
            /etc/apt/sources.list /etc/apt/sources.list.d 2>/dev/null; then

            die "Could not locate a Debian repository configuration to enable non-free."
        fi
    fi
}

accept_steam_license() {
    #
    # SteamCMD's Debian package uses Debconf for Valve's licence agreement.
    # Preseeding allows unattended installation.
    #
    log "Accepting SteamCMD licence agreement through debconf..."

    echo 'steamcmd steam/question select I AGREE' |
        debconf-set-selections

    echo 'steamcmd steam/license note' |
        debconf-set-selections
}

install_dependencies() {
    log "Updating package indexes..."
    apt-get update

    log "Installing SteamCMD and supporting packages..."

    DEBIAN_FRONTEND=noninteractive \
        apt-get install -y \
            ca-certificates \
            curl \
            lib32gcc-s1 \
            lib32stdc++6 \
            steamcmd
}

create_steam_user() {
    [[ "${CREATE_USER}" == true ]] || return

    if id "${STEAM_USER}" &>/dev/null; then
        log "User '${STEAM_USER}' already exists."
        return
    fi

    log "Creating dedicated SteamCMD user '${STEAM_USER}'..."

    useradd \
        --create-home \
        --shell /bin/bash \
        "${STEAM_USER}"
}

verify_installation() {
    local steamcmd_path

    steamcmd_path="$(command -v steamcmd || true)"

    #
    # Debian installs steamcmd into /usr/games, which is not always in
    # root's PATH.
    #
    if [[ -z "${steamcmd_path}" && -x /usr/games/steamcmd ]]; then
        steamcmd_path="/usr/games/steamcmd"
    fi

    [[ -n "${steamcmd_path}" ]] ||
        die "SteamCMD installation completed, but the executable could not be found."

    log "SteamCMD installed successfully:"
    log "  ${steamcmd_path}"

    if [[ "${CREATE_USER}" == true ]]; then
        log ""
        log "Run SteamCMD as the dedicated user with:"
        log "  sudo -u ${STEAM_USER} -H ${steamcmd_path}"
    else
        log ""
        log "Run SteamCMD with:"
        log "  ${steamcmd_path}"
    fi
}

parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case "$1" in
            --user)
                [[ $# -ge 2 ]] ||
                    die "--user requires a username."

                STEAM_USER="$2"
                CREATE_USER=true
                shift 2
                ;;

            --no-user)
                CREATE_USER=false
                shift
                ;;

            -h|--help)
                usage
                exit 0
                ;;

            *)
                die "Unknown option: $1"
                ;;
        esac
    done
}

main() {
    parse_arguments "$@"

    require_root
    check_debian
    check_architecture
    enable_non_free_repository
    accept_steam_license
    install_dependencies
    create_steam_user
    verify_installation

    log "Installation complete."
}

main "$@"
