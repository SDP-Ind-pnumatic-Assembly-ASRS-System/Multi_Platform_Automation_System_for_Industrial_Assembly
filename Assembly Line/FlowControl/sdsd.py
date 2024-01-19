import json
import tkinter as tk
from tkinter import messagebox

# Function to create a new node
def create_node():
    name = name_entry.get()
    num_variables = int(num_variables_entry.get())
    
    node = {"name": name, "variables": {}}
    
    for i in range(num_variables):
        variable_name = variable_names_entries[i].get()
        variable_value = variable_values_entries[i].get()
        node["variables"][variable_name] = variable_value
    
    nodes_listbox.insert(tk.END, name)
    nodes.append(node)

    # Clear input fields
    name_entry.delete(0, tk.END)
    num_variables_entry.delete(0, tk.END)
    for i in range(num_variables):
        variable_names_entries[i].delete(0, tk.END)
        variable_values_entries[i].delete(0, tk.END)

    # Save the updated nodes to the JSON file
    with open(json_file, "w") as file:
        json.dump(nodes, file, indent=4)

    messagebox.showinfo("Node Created", f"Node '{name}' created successfully!")

# Function to display selected node's details
def show_node_details(event):
    selected_node_index = nodes_listbox.curselection()[0]
    selected_node = nodes[selected_node_index]
    
    details_text.config(state=tk.NORMAL)
    details_text.delete(1.0, tk.END)
    details_text.insert(tk.END, json.dumps(selected_node, indent=4))
    details_text.config(state=tk.DISABLED)

# Load existing nodes from the JSON file if it exists
json_file = "nodes.json"
try:
    with open(json_file, "r") as file:
        nodes = json.load(file)
except FileNotFoundError:
    nodes = []

# Create the main window
root = tk.Tk()
root.title("Node Manager")

# Labels and Entry fields for creating nodes
name_label = tk.Label(root, text="Node Name:")
name_label.pack()
name_entry = tk.Entry(root)
name_entry.pack()

num_variables_label = tk.Label(root, text="Number of Variables:")
num_variables_label.pack()
num_variables_entry = tk.Entry(root)
num_variables_entry.pack()

variable_names_labels = []
variable_names_entries = []
variable_values_labels = []
variable_values_entries = []

# Create Entry fields for variable names and values dynamically based on the user's input
def create_variable_fields():
    num_variables = int(num_variables_entry.get())

    for i in range(num_variables):
        var_name_label = tk.Label(root, text=f"Variable Name {i + 1}:")
        var_name_label.pack()
        var_name_entry = tk.Entry(root)
        var_name_entry.pack()
        variable_names_labels.append(var_name_label)
        variable_names_entries.append(var_name_entry)

        var_value_label = tk.Label(root, text=f"Variable Value {i + 1}:")
        var_value_label.pack()
        var_value_entry = tk.Entry(root)
        var_value_entry.pack()
        variable_values_labels.append(var_value_label)
        variable_values_entries.append(var_value_entry)

create_fields_button = tk.Button(root, text="Create Variable Fields", command=create_variable_fields)
create_fields_button.pack()

create_node_button = tk.Button(root, text="Create Node", command=create_node)
create_node_button.pack()

# Listbox to display node names
nodes_listbox = tk.Listbox(root)
nodes_listbox.pack()
nodes_listbox.bind("<ButtonRelease-1>", show_node_details)

# Text widget to display node details
details_text = tk.Text(root, height=10, width=40, state=tk.DISABLED)
details_text.pack()

# Start the Tkinter main loop
root.mainloop()
