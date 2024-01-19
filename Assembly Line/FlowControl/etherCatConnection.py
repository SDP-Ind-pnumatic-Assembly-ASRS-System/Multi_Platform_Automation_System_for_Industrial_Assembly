import pyads
import settings

class EtherCat:

    def __init__(self) -> None:
        self.AMSNETID = settings.get_setting_value("AMSNETID")
        self.port = int(settings.get_setting_value("Port"))
        self.plc: pyads.Connection = None

    def establish_connection(self):
        self.plc = pyads.Connection(self.AMSNETID, self.port)
        self.plc.open()
        return [self.plc.is_open, self.plc.get_local_address()]
    
    def read(self, variable: str, value: int):
        self.plc.read_by_name(variable, value)

    def write(self, variable: str, value: int):
        self.plc.write_by_name(variable, value)

    def close_connection(self):
        self.plc.close()