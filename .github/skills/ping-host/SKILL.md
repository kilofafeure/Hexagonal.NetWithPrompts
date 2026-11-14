---
name: ping-host
description: "Pings a host to verify if it is reachable."
---
# Ping a host

## Input data

- IP address or hostname.
  If you don't have the IP address or hostname, ask the user before proceeding. Do not attempt to guess it.

## When to use this skill

Use it when the user wants to check if a server, device, or website responds to network requests.

## Instructions

1. Verify that you have an IP address or hostname.
2. If not available, ask the user which one they want to check and wait for their response.
3. Run the appropriate command for the operating system.

   - Windows:
     ```bash
     ping <host>
     ```
   - Linux/macOS:
     ```bash
     ping -c 4 <host>
     ```
4. Display the ping result
5. Clearly indicate whether the host responded correctly or could not be reached.
6. If there was packet loss or high response times, mention it to the user.
