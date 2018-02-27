# CSCE482
Missing Person Detection Tool


## Python Setup
* Python 3.6.4
* Numpy
* SciPy
* Scikit-Learn
  * pip install scikit-learn
* PyInstaller
  * pip install pyinstaller
* Spectral (SPy)
  * pip install spectral
* Argparse
  * pip install argparse

## C++ OpenCV Setup
* Windows
  * https://docs.opencv.org/master/d3/d52/tutorial_windows_install.html
  1. TODO Reiterate steps here

* Linux
  * https://docs.opencv.org/master/d7/d9f/tutorial_linux_install.html
  1. TODO Reiterate steps here

* MacOS
  * The easiest way is to use Homebrew, then follow the linux installation instructions.
    * With Homebrew: replace 'sudo apt-get' with 'brew'
  * Once installed you can compile and run as descrided in the linux instructions

  * Or use: "g++ $(pkg-config --cflags --libs opencv) <file_name>.cpp -o <executable_name>"
