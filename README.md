# CSCE482
Missing Person Detection Tool


## Python Setup
* Python 3.6.4
  * https://www.python.org
* OpenCV
  * pip install opencv-python
  * pip install opencv-contrib-python
  * https://opencv.org
  * Tutorial
    * http://opencv-python-tutroals.readthedocs.io/en/latest/py_tutorials/py_tutorials.html
* Numpy
  * pip install numpy
  * http://www.numpy.org
* SciPy
  * pip install scipy
  * https://www.scipy.org
* Scikit-Learn
  * pip install scikit-learn
  * http://scikit-learn.org/stable/
* PyInstaller
  * pip install pyinstaller
  * http://www.pyinstaller.org/
* Spectral (SPy)
  * pip install spectral
    * http://www.spectralpython.net
* Argparse
  * pip install argparse
* Matplotlib
  * pip install matplotlib
  * https://matplotlib.org


## C++ OpenCV Setup (Currently Not Developed For)
* Windows
  * https://docs.opencv.org/master/d3/d52/tutorial_windows_install.html
* Linux
  * https://docs.opencv.org/master/d7/d9f/tutorial_linux_install.html
* MacOS
  * The easiest way is to use Homebrew, then follow the linux installation instructions.
    * With Homebrew: replace 'sudo apt-get' with 'brew'
  * Once installed you can compile and run as descrided in the linux instructions

  * Or use: "g++ $(pkg-config --cflags --libs opencv) <file_name>.cpp -o <executable_name>"
