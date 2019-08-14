# FlagDaemon

Under active development - creating the agent portion of a CTF system designed to score defensive security changes and forensic research on Windows (and eventually Linux) machines.

Goals include (but not limited to):
1. Declarative JSON syntax for:
A. Configuring system according to scenario
B. Determining system configuration relative to scenario flags
2. Interop with an API for:
A. Registering machine with API/DB in order to retrieve applicable scenario JSON
B. Reporting changes in machine-state pertinent to score
C. Receiving updates on rank for display to user
