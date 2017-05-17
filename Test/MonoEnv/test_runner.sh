#!/bin/bash

./nunit-console4 ../Test.exe

mono ./nunit-results.exe ./TestResult.xml
