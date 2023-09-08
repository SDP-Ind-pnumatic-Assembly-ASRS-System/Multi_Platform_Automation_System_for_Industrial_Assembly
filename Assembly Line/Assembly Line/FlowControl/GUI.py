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
        self.title("Assembly Line Control")
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

        self.label_SystemConfiguration = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text="System Configuration", font = custom_font)
        self.label_SystemConfiguration.grid(row = 0, column = 0, sticky = "W")

        self.label_AMSNETID_Head = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text="AMSNETID: ",)
        self.label_AMSNETID_Head.grid(row = 1, column = 0, sticky = "W")

        self.label_AMSNETID = customtkinter.CTkLabel(self.tabview.tab("Manual Control"), text=AMSNETID,)
        self.label_AMSNETID.grid(row = 1, column = 0, sticky = "W")
        # self.optionmenu_1 = customtkinter.CTkOptionMenu(self.tabview.tab("Tab 1"), dynamic_resizing=False,
        #                                                 values=["Value 1", "Value 2", "Value Long Long Long"])
        # self.optionmenu_1.grid(row=0, column=0, padx=20, pady=(20, 10))
        # self.combobox_1 = customtkinter.CTkComboBox(self.tabview.tab("Tab 1"),
        #                                             values=["Value 1", "Value 2", "Value Long....."])
        # self.combobox_1.grid(row=1, column=0, padx=20, pady=(10, 10))
        # self.string_input_button = customtkinter.CTkButton(self.tabview.tab("Tab 1"), text="Open CTkInputDialog",
        #                                                    command=self.open_input_dialog_event)
        # self.string_input_button.grid(row=2, column=0, padx=20, pady=(10, 10))
        # self.label_tab_2 = customtkinter.CTkLabel(self.tabview.tab("Tab 2"), text="CTkLabel on Tab 2")
        # self.label_tab_2.grid(row=0, column=0, padx=20, pady=20)

        # self.optionmenu_1.set("CTkOptionmenu")
        # self.combobox_1.set("CTkComboBox")


        def establishConnection():
            plc = pyads.Connection(AMSNETID, pyads.PORT_TC3PLC1)
            plc.open()
            print(f"Connected?: {plc.is_open}") #debugging statement, optional
            print(f"Local Address? : {plc.get_local_address()}") #debugging statement, optional
        
        
       

if __name__ == "__main__":
    app = App()
    app.mainloop()