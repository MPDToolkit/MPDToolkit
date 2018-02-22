import sys

class ExceptionHandler:
    def __init__(self):
       self.default_retrys = 5

    def watch(self, callback, args=None):
        try:
            if args is not None:
                return callback(args)
            else:
                return callback()
        except OSError as e:
            print("OS error: {0}".format(e))
        except ValueError as e:
            print("Value error: {0}".format(e))
        except TypeError as e:
            print("Type error: {0}".format(e))
        except:
            print("Unexpected error:", sys.exc_info()[0])
        
            
    def ignore(self, callback, args=None):
        try:
            if args is not None:
                return callback(args)
            else:
                return callback()
        except:
            return
            #print("Unexpected error:", sys.exc_info()[0])
    
    def retry(self, callback, args=None, num_retrys=None): #Refactor
        if num_retrys is None:
            num_retrys = self.default_retrys

        for i in range(0, num_retrys):    
            try:
                if args is not None:
                    return callback(args)
                else:
                    return callback('')
            except:
                #print("Attempt {0} failed...".format(i+1))
                continue
                

