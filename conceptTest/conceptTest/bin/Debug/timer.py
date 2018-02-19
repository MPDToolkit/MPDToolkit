import time

class Timer:
    def __init__(self):
        self.t_start = float(0)
        self.t_end = float(0)

    def start(self):
        self.t_start = float(time.perf_counter())

    def end(self):
        self.t_end = float(time.perf_counter())
    
    def get_time(self):
        return float(self.t_end - self.t_start)
