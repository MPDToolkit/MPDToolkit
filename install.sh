#!/bin/bash

#Need some stuff here to determine which installer to use for python and pip (apt-get vs apt vs brew, etc.)


#Install the python libraries with pip
#openCV
pip3 -q install opencv-python
pip3 -q install opencv-contrib-python
echo "installed opencv"
#NumPy
pip3 -q install numpy
echo "installed numpy"
#SciPy
pip3 -q install scipy
echo "installed scipy"
#Scikit-Learn (Not currently used)
pip3 -q install scikit-learn
echo "installed scikit-learn"
#PyInstaller
pip3 -q install pyinstaller
echo "installed PyInstaller"
#Spectral Python
pip3 -q install spectral
echo "installed spectral"
#Argument Parsing
pip3 -q install argparse
echo "installed arg parser"
#Matplotlib
pip3 -q install matplotlib
echo "installed matplotlib"
