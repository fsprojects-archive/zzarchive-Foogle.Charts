#!/bin/bash
if [ ! -e packages/FAKE/tools/FAKE.exe ]; then 
  mono .nuget/NuGet.exe install FAKE -OutputDirectory packages -ExcludeVersion -version "3.2.1"
fi
mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx 
