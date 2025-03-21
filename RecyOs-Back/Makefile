configure:
	ifeq test "witch docker"
	  ifeq test "uname -a | grep -i darwin"
	    brew install docker
      else ifeq test "uname -a | grep -i ubuntu"
		sudo apt-get install docker
    else ifeq test "uname -a | grep -i centos"
        sudo yum install docker
        else
          echo "OS not supported"

