#!/bin/bash

dotnet Program.dll &
service nginx start
