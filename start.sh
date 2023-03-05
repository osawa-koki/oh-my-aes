#!/bin/bash

nohup dotnet Program.dll &
service nginx start
