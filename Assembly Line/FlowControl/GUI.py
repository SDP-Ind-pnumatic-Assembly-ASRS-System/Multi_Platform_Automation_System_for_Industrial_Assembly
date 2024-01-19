import tkinter
import tkinter.messagebox
import customtkinter
import pyads

AMSNETID = "10.1.2.54.1.1"

customtkinter.set_appearance_mode("System")  # Modes: "System" (standard), "Dark", "Light"
customtkinter.set_default_color_theme("blue")  # Themes: "blue" (standard), "green", "dark-blue"


class App(customtkinter.CTk):
    def __init__(self):
        super().__init__()

        custom_font = ("Helvetica", 16)
        # configure window
        self.title("Robot Client Control")
        self.geometry(f"{1100}x{580}")

        # configure grid layout (4x4)
        self.grid_columnconfigure(1, weight=1)
        self.grid_columnconfigure((2, 3), weight=0)
        self.grid_rowconfigure((0, 1, 2), weight=1)

        self.textbox = customtkinter.CTkTextbox(self, height = 150, width=250)
        self.textbox.grid(row=1, column=1, padx=(20, 20), pady=(20, 0), sticky="nsew")
        self.textbox.insert("0.0", "")

        # create tabview
        self.tabview = customtkinter.CTkTabview(self, height = 500, width=250)
        self.tabview.grid(row=0, column=1, padx=(20, 20), pady=(20, 0), sticky="nsew")
        self.tabview.add("Manual Control")
        self.tabview.add("Tab 2")
        self.tabview.add("Tab 3")
        self.tabview.tab("Manual Control").grid_columnconfigure((0,1), weight=1)  # configure grid of individual tabs
        self.tabview.tab("Tab 2").grid_columnconfigure(0, weight=1)

        self.label_ServerConfiguration = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text="Server Configuration", font = custom_font)
        self.label_ServerConfiguration.place(relx=0.0, rely=0.1, anchor=tkinter.W)
        self.label_StatusHead = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text="Status:")
        self.label_StatusHead.place(relx=0.0, rely=0.15, anchor=tkinter.W)
        self.label_StatusTxt = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text="Disconnected")
        self.label_StatusTxt.place(relx=0.05, rely=0.15, anchor=tkinter.W)
        self.Box_opcLink = customtkinter.CTkEntry(self.tabview.tab("Manual Control"), width = 300, placeholder_text="Server Link (Ex: opc.tcp://ip:port)")
        self.Box_opcLink.place(relx=0.0, rely=0.225, anchor=tkinter.W)
        self.AutoFetchIPvar = tkinter.BooleanVar(value = True)

        def printdetails():
            if self.AutoFetchIPvar.get():
                self.Box_opcLink.delete(0, self.Box_opcLink.index("end"))
                self.Box_opcLink.configure(state = "disabled")
            else : self.Box_opcLink.configure(state = "normal")

        self.radio_switch_AutoFetchIP = customtkinter.CTkSwitch(self.tabview.tab("Manual Control"), text= "Auto fetch IP Address", variable = self.AutoFetchIPvar, command = printdetails)
        self.radio_switch_AutoFetchIP.place(relx=0.15, rely=0.15, anchor=tkinter.W)
        self.Button_StartServer = customtkinter.CTkButton(self.tabview.tab("Manual Control"), width = 300, text="Start Server")
        self.Button_StartServer.place(relx=0.0, rely=0.3, anchor=tkinter.W)

        
        self.Box_opcLink.configure(state = "disabled")
        def establishConnection():
            plc = pyads.Connection(AMSNETID, pyads.PORT_TC3PLC1)
            plc.open()
            print(f"Connected?: {plc.is_open}") #debugging statement, optional
            print(f"Local Address? : {plc.get_local_address()}") #debugging statement, optional
        

if __name__ == "__main__":
    app = App()
    app.mainloop()